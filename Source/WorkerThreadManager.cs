
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
    /// 
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
            ZoneManager zoneManager = ZoneManager.Instance;

            for ( ;; )
            {
                // Zone 로직 처리
                for ( ;; )
                {
                    var zone = zoneManager.GetZone();
                    if ( zone is null )
                        continue;

                    zone.Update();

                    zoneManager.Return( zone );
                }
            }

        } );
    }
}
