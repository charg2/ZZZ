using Mala.Core;

/// <summary>
/// 
/// </summary>
public class WorkerThreadManager : Singleton< WorkerThreadManager >
{
    /// <summary>
    /// 스레드 컬렉션
    /// </summary>
    List< Thread > threads;

    /// <summary>
    /// 초기화한다.
    /// </summary>
    public void Initialize( int n )
    {
        for ( int i = 0; i < n; i += 1 )
        {
            threads.Add( Create() );
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static Thread Create()
    {
        return new Thread( () => 
        { 
            for ( ;; )
            {
                /// 다른 로직들을 처리....

                /// 존 로직 처리
                UpdateZoneLogic();
            }

        } );
    }

    /// <summary>
    /// 존을 갱신한다.
    /// </summary>
    private static void UpdateZoneLogic()
    {
        ZoneManager zoneManager = ZoneManager.Instance;

        // Zone 로직 처리
        for ( ;; )
        {
            if ( zoneManager.IsCompleted )
                return;

            var zone = zoneManager.GetZone();
            if ( zone is null )
                continue;

            zone.Update();

            zoneManager.Return( zone );
        }
    }
}
