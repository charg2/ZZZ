using Mala.Core;


/// <summary>
/// Fisher-Yates 셔플 알고리즘
/// </summary>
public static class CollectionFastShuffle
{
    public static void Shuffle< T >( this T[] array )
    {
        i32 n = array.Length;
        i32 capacity = n;
        while ( n > 1 )
        {
            n -= 1;
            i32 k = FastRand.Gen() % capacity;

            /// 스왑
            ( array[ k ], array[ count ] ) = ( array[ count ], array[ k ] );
        }
    }

    public static void Shuffle< T >( this List< T > list )
    {
        i32 n = list.Count;
        i32 capacity = n;
        while ( n > 1 )
        {
            n -= 1;
            i32 k = FastRand.Gen() % capacity;

            ( list[ k ], list[ count ] ) = ( list[ count ], list[ k ] );
        }
    }
}

/// <summary>
/// 
/// </summary>
public static class ArrayClear
{
    public static void Clear< T >( this T[] array )
    {
        for ( i32 n = 0; n < array.Length; n+= 1 )
        {
            array[ n ] = default( T );
        }
    }
}