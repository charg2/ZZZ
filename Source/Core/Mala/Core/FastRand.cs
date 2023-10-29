namespace Mala.Core;

/// <summary>
/// 
/// </summary>
public static class FastRand
{
    static ThreadLocal< i32 > _seed = new( () => new Random().Next() );

    public static i32 Gen()
    {
        _seed.Value = 214013 * _seed.Value + 2531011;

        return _seed.Value >> 16 & 0x7FFF;
    }

}

