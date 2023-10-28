using Mala.Math;

/// <summary>
/// 클러스터 경계
/// </summary>
public struct ClusterBorder
{
    /// <summary>
    /// 마크됨 플래그
    /// </summary>
    public const i32 Marked = 1;

    /// <summary>
    /// 마크 해제 상태 플래그
    /// </summary>
    public const i32 Unmarked = 0;

    /// <summary>
    /// 주변 클러스터 접근 정책
    /// </summary>
    static readonly ValueTuple< i32, i32 >[] _nearZoneAccessPolicy =
    [
        new( -1, -1 ), new( -1, 0 ), new( -1, 1 ),
        new(  0, -1 ), new(  0, 0 ), new(  0, 1 ),
        new(  1, -1 ), new(  1, 0 ), new(  1, 1 ),
    ];

    /// <summary>
    /// 마크 플래그
    /// </summary>
    i64 _markFlag = Unmarked;

    /// <summary>
    /// 주변 클러스터 목록
    /// </summary>
    List< Cluster > _nearZones = new( 3 * 3 );

    /// <summary>
    /// 생성자
    /// </summary>
    public ClusterBorder()
    {
    }

    /// <summary>
    /// 마크 시도한다.
    /// </summary>
    bool TryMark() => Interlocked.Exchange( ref _markFlag, Marked ) == Unmarked;

    /// <summary>
    /// 경계의 락을 획득한다.
    /// </summary>
    public bool TryLock()
    {
        i32 markCount = 0;
        foreach ( var zone in _nearZones ) 
        {
            /// 주변 클러스터의 마킹을 시도한다.
            if ( !zone.Border.TryMark() )
            {
                UnmarkNearZones( markCount );

                return false;
            }

            markCount += 1;
        }

        return true;
    }

    /// <summary>
    /// 초기화한다.
    /// </summary>
    public bool Initialize( Vector2Int index )
    {
        foreach ( var i in _nearZoneAccessPolicy )
        {
            Cluster? zone = ClusterManager.Instance[ index.y + i.Item1, index.x + i.Item2 ];
            if ( zone != null )
            {
                _nearZones.Add( zone );
            }
        }

        return true;
    }

    /// <summary>
    /// 주변 클러스터의 마킹을 해제한다.
    /// </summary>
    private void UnmarkNearZones( i32 markedZoneCount )
    {
        for ( i32 n = 0; n < markedZoneCount; n += 1 )
        {
            _nearZones[ n ].Border.Unmark();
        }
    }

    /// <summary>
    /// 잠금을 해제한다.
    /// </summary>
    public bool Unlock()
    {
        foreach ( var zone in _nearZones )
        {
            zone.Border.Unmark();
        }

        return true;
    }

    /// <summary>
    /// 마킹을 해제한다.
    /// </summary>
    private void Unmark()
    {
        _markFlag = Unmarked;
    }
}