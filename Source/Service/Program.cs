Console.WriteLine( "ㅎ2ㅎ2" );

ZoneManager.Instance.Initialize( 4, 4 );
WorkerThreadManager.Instance.Initialize( 4 );
WorkerThreadManager.Instance.Start();

/// 메인 스레드도 실행
WorkerThread.Execute();
