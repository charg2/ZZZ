namespace Mala.Core;

/// <summary>
/// 
/// </summary>
public static class FastRand
{
    static ThreadLocal< i32 > seed = new( () => new Random().Next() );

    public static i32 Gen()
    {
        seed.Value = 214013 * seed.Value + 2531011;

        return seed.Value >> 16 & 0x7FFF;
    }

}

