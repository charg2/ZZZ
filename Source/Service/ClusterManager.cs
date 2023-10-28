using Mala.Collection;
using Mala.Core;
using Mala.Math;

using System.Collections.Concurrent;

/// <summary>
/// 클러스터 관리자
/// </summary>
public class ClusterManager : Singleton< ClusterManager >
{
    /// <summary>
    /// 클러스터 최대 갯수
    /// </summary>
    i32 _zoneCapacity;

    /// <summary>
    /// 업데이트 횟수
    /// </summary>
    i64 _zoneUpdateCount = -1;

    /// <summary>
    /// 클러스터 반환 횟수
    /// </summary>
    i64 _zonePushCount = 0;

    /// <summary>
    /// 클러스터 보관 컬렉션
    /// </summary>
    Cluster[ , ] _zones;

    /// <summary>
    /// 
    /// </summary>
    Cluster[] _zoneArray;

    /// <summary>
    /// 클러스터 스택
    /// </summary>
    public ConcurrentStack< Cluster > _zoneCollection = new();

    /// <summary>
    /// 버전
    /// </summary>
    public u64 _version { get; set; } = 0;

    /// <summary>
    /// 스레드별 버전
    /// </summary>
    public ThreadLocal< u64 > LocalVersion = new( () => 0 );

    /// <summary>
    /// 스레드별 현재 바인드된 클러스터
    /// </summary>
    [ThreadStatic]
    public static Cluster? DispatchingClusterOnThisThread;

    /// <summary>
    /// 초기화한다.
    /// </summary>
    public void Initialize( i32 y, i32 x )
    {
        InitZones( y, x );

        _zoneCollection.PushRange( _zoneArray, 0, _zoneCapacity );
    }

    /// <summary>
    /// 클러스터들을 초기화한다.
    /// </summary>
    private void InitZones( i32 y, i32 x )
    {
        u64 id = 0;
        _zoneCapacity = y * x;

        _zones = new Cluster[ y, x ];
        _zoneArray = GC.AllocateArray< Cluster >( _zoneCapacity, true );

        for ( i32 yIndex = 0; yIndex < y; yIndex += 1 )
        {
            for ( i32 xIndex = 0; xIndex < x; xIndex += 1 )
            {
                _zones[ yIndex, xIndex ] = new Cluster();
                _zones[ yIndex, xIndex ].Initialize( id, new( yIndex, xIndex ) );

                _zoneArray[ id ] = _zones[ yIndex, xIndex ];

                id += 1;
            }
        }
    }

    /// <summary>
    /// 유효한 인덱스 여부를 반환한다.
    /// </summary>
    private bool IsValidIndex( i32 y, i32 x ) 
        => y >= 0 && y < _zones.GetLength( 0 ) && x >= 0 && x < _zones.GetLength( 1 );

    /// <summary>
    /// 인덱서
    /// </summary>
    public Cluster? this[ Vector2Int index ] => this[ index.y, index.x ];

    /// <summary>
    /// 인덱서
    /// </summary>
    public Cluster? this[ i32 y, i32 x ]
    {
        get
        {
            if ( !IsValidIndex( y, x ) )
                return null;

            return _zones[ y, x ];
        }
    }

    /// <summary>
    /// 클러스터를 획득 시도한다
    /// </summary>
    public bool TryPop( out Cluster zone )
    {
        for ( ;; )
        {
            if ( _zoneCollection.TryPop( out zone ) )
            {
                if ( !zone.Border.TryLock() )
                {
                    RePush( zone );
                    continue;
                }

                return true;
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary>
    /// 삽입한다.
    /// </summary>
    public void Push( Cluster zone )
    {
        /// 클러스터 경계의 마킹을 해제한다
        zone.Border.Unlock();

        var nextIndex = Interlocked.Increment( ref _zoneUpdateCount );
        _zoneArray[ nextIndex ] = zone;
        var pushedCount = Interlocked.Increment( ref _zonePushCount );

        /// 마지막 처리한 친구가 초기 상태로 되돌린다
        if ( pushedCount == _zoneCapacity )
            Reset();
        
        return;
    }

    /// <summary>
    /// 재삽입 처리한다.
    /// </summary>
    public void RePush( Cluster zone )
    {
        /// 경합을 줄이기 위해선..
        _zoneCollection.Push( zone );
    }

    /// <summary>
    /// 초기 상태로 되돌린다.
    /// </summary>
    public void Reset()
    {
        _version += 1;

        _zoneArray.Shuffle();

        _zoneUpdateCount = -1;

        _zonePushCount = 0;
        
        // ShowRst();

        _zoneCollection.PushRange( _zoneArray );
    }

    /// <summary>
    /// 
    /// </summary>
    public bool CheckAndUpdateVersion()
    {
        if ( _version > LocalVersion.Value )
        {
            LocalVersion.Value = _version;
            return false;
        }

        return true;
    }

    /// <summary>
    /// 리셋 디버깅용 카운트
    /// </summary>
    i64 _rstDebugCount = 0;
    bool flag = false;

    private void ShowRst()
    {
        var resetCount = _rstDebugCount++;
        if ( ( resetCount % 1_0000 ) == 0 )
        {
            flag ^= true;

            Console.WriteLine( flag ? "rst 0" : "rst 1"  );
        }
    }

}


