using Mala.Core;


/// <summary>
/// Fisher-Yates 셔플 알고리즘
/// </summary>
public static class CollectionFastShuffle
{
    public static void Shuffle< T >( this T[] array )
    {
        int n = array.Length;
        int capacity = n;
        while ( n > 1 )
        {
            n -= 1;
            int k = FastRand.Gen() % capacity;

            /// 스왑
            ( array[ k ] ) = ( array[ n ] );
        }
    }

    public static void Shuffle< T >( this List< T > list )
    {
        i32 n = list.Count;
        i32 capacity = n;
        while ( n > 1 )
        {
            n -= 1;
            int k = FastRand.Gen() % capacity;

            ( list[ k ] ) = ( list[ n ] );
        }
    }
}