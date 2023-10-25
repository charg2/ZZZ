using Mala.Core;


/// <summary>
/// Fisher-Yates 셔플 알고리즘
/// </summary>
public static class CollectionFastShuffle
{
    public static void Shuffle< T >( this T[] array )
    {
        int n = array.Length;
        int max = n;
        while ( n > 1 )
        {
            n -= 1;
            int k = FastRand.Gen() % max + 1;

            /// 스왑
            ( array[ k ] ) = ( array[ n ] );
        }
    }

    public static void Shuffle< T >( this List< T > list )
    {
        int n = list.Count;
        int max = n;
        while ( n > 1 )
        {
            n -= 1;
            int k = FastRand.Gen() % max + 1;

            ( list[ k ] ) = ( list[ n ] );
        }
    }
}