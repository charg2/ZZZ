Console.WriteLine( "ㅎ2ㅎ2" );


ZoneManager.Instance.Initialize( 4, 4 );

WorkerThreadManager.Instance.Initialize( 4 );
WorkerThreadManager.Instance.Start();

for ( ;; )
{
    Thread.Sleep( 1000 );
}
