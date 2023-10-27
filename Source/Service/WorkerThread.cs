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

                /// 존 로직 처리
                UpdateZoneLogic();
            }
        };
    }

    
    /// <summary>
    /// 존을 갱신한다.
    /// </summary>
    private static void UpdateZoneLogic()
    {
        var zoneManager = ZoneManager.Instance;

        for ( ;; )
        {
            /// 존을 꺼낸다
            if ( !zoneManager.TryPop( out var zone ) )
                return;

            /// Zone 로직 처리
            zone.Update();

            /// 주변 존의 마킹을 해제한다
            zone.UnmarkNearZones();

            /// 작업을 끝낸 존을 반한한다
            if ( zoneManager.Push( zone ) )
            {
                /// 마지막 존 작업을 끝낸 후 초기 상태로 되돌린다
                zoneManager.Reset();

                /// 다른 로직을 처리하기 위해 종료처리
                return;
            }
        }
    }

}
