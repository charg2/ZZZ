using Mala.Core;
using System.Threading;

/// <summary>
/// 
/// </summary>
public class WorkerThreadManager : Singleton< WorkerThreadManager >
{
    /// <summary>
    /// 스레드 컬렉션
    /// </summary>
    List< Thread > threads = new();

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
    /// 시작한다.
    /// </summary>
    public void Start()
    {
        foreach ( var thread in threads )
        {
            thread.Priority = ThreadPriority.Highest;
            thread.Start();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static Thread Create()
    {
        return new( () => 
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
        var zoneManager = ZoneManager.Instance;

        // Zone 로직 처리
        for ( ;; )
        {
            if ( !zoneManager.TryPop( out var zone ) )
                return;

            zone.Update();

            zone.UnmarkNearZones();

            if ( zoneManager.Push( zone ) )
            {
                zoneManager.Reset();
                return;
            }
        }
    }
}
