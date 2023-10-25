
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
    int _zoneCapacity = 0;

    /// <summary>
    /// 업데이트 횟수
    /// </summary>
    long _zoneUpdateCount = 0;

    /// <summary>
    /// 
    /// </summary>
    long _zoneAddCount = 0;

    /// <summary>
    /// 존 보관 컬렉션
    /// </summary>
    Zone[ , ] _zones;

    /// <summary>
    /// 
    /// </summary>
    Zone[] _subZones;

    /// <summary>
    /// 
    /// </summary>
    public ConcurrentStack< Zone > _zoneCollection = new();

    /// <summary>
    /// 
    /// </summary>
    public void Initialize( int y, int x )
    {
        _zoneCapacity = y * x;

        _subZones = new Zone[ _zoneCapacity ];
        InitZones( y, x );

        _zoneCollection.PushRange( _subZones, 0, _zoneCapacity );
    }

    /// <summary>
    /// 
    /// </summary>
    private void InitZones( int y, int x )
    {
        int i = 0;

        _zones = new Zone[ y, x ];
        for ( int yIndex = 0; yIndex < y; yIndex += 1 )
        {
            for ( int xIndex = 0; xIndex < x; xIndex += 1 )
            {
                _zones[ yIndex, xIndex ] = new Zone();
                _zones[ yIndex, xIndex ].Initialize( new( yIndex, xIndex ) );

                _subZones[ i ] = _zones[ yIndex, xIndex ];
                i += 1;
            }
        }
    }

    /// <summary>
    /// 유효한 인덱스 여부를 반환한다.
    /// </summary>
    private bool IsValidIndex( int row, int column ) 
        => row >= 0 && row < _zones.GetLength( 0 ) && column >= 0 && column < _zones.GetLength( 1 );

    /// <summary>
    /// 인덱서
    /// </summary>
    public Zone? this[ Vector2Int index ] => this[ index.x, index.y ];

    /// <summary>
    /// 인덱서
    /// </summary>
    public Zone? this[ int y, int x ]
    {
        get
        {
            if ( IsValidIndex( y, x ) )
                return null;

            return _zones[ y, x ];
        }
    }

    /// <summary>
    /// 존을 획득한다
    /// </summary>
    public Zone? GetZone()
    {
        for ( ;; )
        {
            /// 처리중, 
            if ( _zoneCollection.TryPop( out var zone ) )
            {
                if ( !zone.TryMarkNearZones() )
                {
                    Push( zone );
                    continue;
                }
            }

            return zone;
        }
    }

    /// <summary>
    /// 처리완료중, 딴거나 하셈
    /// </summary>
    public bool IsCompleted => _zoneCapacity == _zoneAddCount;

    /// <summary>
    /// 존을 반환한다.
    /// </summary>
    public void Return( Zone zone )
    {
        var nextIndex = Interlocked.Increment( ref _zoneUpdateCount );
        _subZones[ nextIndex ] = zone;
        var addedCount = Interlocked.Increment( ref _zoneAddCount );

        /// 최종 삽입 완료한 친구가 책임지고 정리
        if ( addedCount == _zoneCapacity )
        {
            _subZones.Shuffle();

            _zoneCollection.PushRange( _subZones );

            _zoneUpdateCount = 0;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal void Push( Zone zone )
    {
        /// 경합을 줄이기 위해선..
        _zoneCollection.Push( zone );
    }
}


