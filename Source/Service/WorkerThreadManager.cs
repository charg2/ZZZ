using Mala.Core;

/// <summary>
/// 워커 스레드 관리자
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
    public void Initialize( i32 n )
    {
        for ( i32 i = 0; i < n; i += 1 )
        {
            threads.Add( new( WorkerThread.Execute() ) );
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

}
