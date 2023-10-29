using Mala.Core;

/// <summary>
/// 워커 스레드
/// </summary>
public class WorkerThread
{
    /// <summary>
    /// 실행한다.
    /// </summary>
    public static ThreadStart Execute()
    {
        return () =>
        {
            for ( ;; )
            {
                /// 다른 로직들을 처리....
                /// Do Logic

                /// 클러스터 처리
                DispatchCluster();
            }
        };
    }

    /// <summary>
    /// 클러스터을 처리한다.
    /// </summary>
    private static void DispatchCluster()
    {
        var clusterManager = ClusterManager.Instance;

        for ( ;; )
        {
            /// 스레드별로 클러스터매니저 리셋 후 다른 로직 처리를 보장하기 위해 버전을 체크한다.
            if ( !clusterManager.CheckAndUpdateVersion() )
                return;

            /// 클러스터을 할당 받는다.
            if ( !clusterManager.TryPop( out var cluster ) )
                return;

            /// 현재 처리 중인 클러스터를 설정한다.
            ClusterManager.DispatchingClusterOnThisThread = cluster;

            /// 클러스터 로직 처리
            cluster.Update();

            /// 현재 처리 중인 클러스터를 제거한다.
            ClusterManager.DispatchingClusterOnThisThread = null;

            /// 작업을 끝낸 클러스터을 반환한다
            clusterManager.Push( cluster );
        }
    }

}
