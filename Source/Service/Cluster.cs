using Mala.Math;

/// <summary>
/// 월드 공간 분할 단위
/// </summary>
public class Cluster
{
    /// <summary>
    /// 식별자
    /// </summary>
    public ClusterId ClusterId { get; set; }

    /// <summary>
    /// 클러스터 경계
    /// </summary>
    public ClusterBorder Border = new();

    /// <summary>
    /// 초기화한다.
    /// </summary>
    public bool Initialize( u64 id, Vector2Int index )
    {
        ClusterId = id;

        return Border.Initialize( index );
    }

    /// <summary>
    /// 갱신한다.
    /// </summary>
    public void Update()
    {
        /// Updating Zone Logic
    }
}