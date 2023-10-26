using Mala.Math;

/// <summary>
/// 
/// </summary>
public class Zone
{
    /// <summary>
    /// 식별자
    /// </summary>
    public u64 Id;

    /// <summary>
    /// 주변 존 접근 정책
    /// </summary>
    static ValueTuple< i32, i32 >[] nearZoneAccessPolicy =
    [
        new ( -1, -1 ), new ( -1, 0 ), new ( -1, 1 ),
        new (  0, -1 ), new (  0, 0 ), new (  0, 1 ),
        new (  1, -1 ), new (  1, 0 ), new (  1, 1 ),
    ];

    /// <summary>
    /// 마크됨 플래그
    /// </summary>
    public const int Marked = 1;

    /// <summary>
    /// 
    /// </summary>
    public const int Unmarked = 0;

    /// <summary>
    /// 
    /// </summary>
    long _markFlag = 0;

    /// <summary>
    /// 
    /// </summary>
    List< Zone > _nearZone = new( 3 * 3 );

    /// <summary>
    /// 초기화한다.
    /// </summary>
    public bool Initialize( u64 id, Vector2Int index )
    {
        Id = id;

        foreach ( var i in nearZoneAccessPolicy )
        {
            Zone? zone = ZoneManager.Instance[ index.y + i.Item1, index.x + i.Item2 ];
            if ( zone != null )
            {
                _nearZone.Add( zone );
            }
        }

        return true;
    }

    /// <summary>
    /// 마크 시도한다.
    /// </summary>
    bool TryMark() => Interlocked.Exchange( ref _markFlag, Marked ) == Unmarked;

    /// <summary>
    /// 주변 존에 대해 마크를 시도한다.
    /// </summary>
    public bool TryMarkNearZones()
    {
        i32 markCount = 0;
        foreach ( var zone in _nearZone ) 
        {
            if ( !zone.TryMark() )
            {
                for ( i32 n = 0; n < markCount; n += 1 )
                {
                    _nearZone[ n ].Unmark();
                }

                return false;
            }

            markCount += 1;
        }

        return true;
    }

    /// <summary>
    /// 주변 존에 대해 마크해제를 시도한다.
    /// </summary>
    public bool UnmarkNearZones()
    {
        foreach ( var zone in _nearZone ) 
        {
            zone.Unmark();
        }

        return true;
    }

    /// <summary>
    /// 마크를 해제한다.
    /// </summary>
    private void Unmark()
    {
        _markFlag = Unmarked;
    }

    /// <summary>
    /// 
    /// </summary>
    public void Update()
    {
        /// Updating Zone Logic
    }
}