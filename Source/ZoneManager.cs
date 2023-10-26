
using Mala.Core;
using Mala.Math;
using System.Collections.Concurrent;


/// <summary>
/// 존 관리자
/// </summary>
public class ZoneManager : Singleton< ZoneManager >
{
    /// <summary>
    /// Zone 최대 갯수
    /// </summary>
    i32 _zoneCapacity;

    /// <summary>
    /// 업데이트 횟수
    /// </summary>
    i64 _zoneUpdateCount = -1;

    /// <summary>
    /// 존 반환 횟수
    /// </summary>
    i64 _zonePushCount = 0;

    /// <summary>
    /// 존 보관 컬렉션
    /// </summary>
    Zone[ , ] _zones;

    /// <summary>
    /// \
    /// </summary>
    Zone[] _subZones;

    /// <summary>
    /// 
    /// </summary>
    public ConcurrentStack< Zone > _zoneCollection = new();

    /// <summary>
    /// 초기화한다.
    /// </summary>
    public void Initialize( i32 y, i32 x )
    {
        InitZones( y, x );

        _zoneCollection.PushRange( _subZones, 0, _zoneCapacity );
    }

    /// <summary>
    /// 존들을 초기화한다.
    /// </summary>
    private void InitZones( i32 y, i32 x )
    {
        u64 id = 0;
        _zoneCapacity = y * x;

        _zones = new Zone[ y, x ];
        _subZones = new Zone[ _zoneCapacity ];

        for ( i32 yIndex = 0; yIndex < y; yIndex += 1 )
        {
            for ( i32 xIndex = 0; xIndex < x; xIndex += 1 )
            {
                _zones[ yIndex, xIndex ] = new Zone();
                _zones[ yIndex, xIndex ].Initialize( id, new( yIndex, xIndex ) );

                _subZones[ id ] = _zones[ yIndex, xIndex ];

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
    public Zone? this[ Vector2Int index ] => this[ index.y, index.x ];

    /// <summary>
    /// 인덱서
    /// </summary>
    public Zone? this[ i32 y, i32 x ]
    {
        get
        {
            if ( !IsValidIndex( y, x ) )
                return null;

            return _zones[ y, x ];
        }
    }

    /// <summary>
    /// 존을 획득한다
    /// </summary>
    public bool TryPop( out Zone zone )
    {
        for ( ;; )
        {
            if ( _zoneCollection.TryPop( out zone ) )
            {
                if ( !zone.TryMarkNearZones() )
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
    /// 처리완료중, 딴거나 하셈
    /// </summary>
    public bool IsCompleted => _zoneCapacity == _zonePushCount;

    /// <summary>
    /// 존을 반환한다.
    /// </summary>
    public bool Push( Zone zone )
    {
        var nextIndex = Interlocked.Increment( ref _zoneUpdateCount );
        _subZones[ nextIndex ] = zone;
        var pushedCount = Interlocked.Increment( ref _zonePushCount );

        /// 최종 삽입 완료한 친구가 책임지고 정리
        return pushedCount == _zoneCapacity;
    }

    /// <summary>
    /// 초기 상태로 되돌린다.
    /// </summary>
    public void Reset()
    {
        _subZones.Shuffle();

        _zoneUpdateCount = -1;

        _zonePushCount = 0;

        // ShowRst();

        _zoneCollection.PushRange( _subZones );
    }


    /// <summary>
    /// 리셋 디버깅용 카운트
    /// </summary>
    i64 _rstDebugCount = 0;
    bool flag = false;

    private void ShowRst()
    {
        var resetCount = _rstDebugCount++;
        if ( ( resetCount % 10000 ) == 0 )
        {
            flag ^= true;

            Console.WriteLine( flag ? "rst 0" : "rst 1"  );
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void RePush( Zone zone )
    {
        /// 경합을 줄이기 위해선..
        _zoneCollection.Push( zone );
    }
}


