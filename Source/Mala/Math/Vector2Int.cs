namespace Mala.Math;

public struct Vector2Int
{
    public i32 x;
    public i32 y;

    public static readonly Vector2Int Zero     = new(  0,  0 );
    public static readonly Vector2Int One      = new(  1,  1 );
    public static readonly Vector2Int MinusOne = new( -1, -1 );
    public static readonly Vector2Int Right    = new(  1,  0 );
    public static readonly Vector2Int Left     = new( -1,  0 );
    public static readonly Vector2Int Up       = new(  0,  1 );
    public static readonly Vector2Int Down     = new(  0, -1 );

    public Vector2Int( i32 x, i32 y )
    {
        this.x = x;
        this.y = y;
    }

    public static Vector2Int operator +( Vector2Int p1, Vector2Int p2 )
        => new( p1.x + p2.x, p1.y + p2.y );

    public static Vector2Int operator -( Vector2Int p1, Vector2Int p2 )
        => new( p1.x - p2.x, p1.y - p2.y );

    public static Vector2Int operator *( Vector2Int p1, Vector2Int p2 )
        => new( p1.x * p2.x, p1.y * p2.y );

    public static Vector2Int operator *( Vector2Int p1, i32 scala )
        => new( p1.x * scala, p1.y * scala );

    public static Vector2Int operator *( i32 scala, Vector2Int p2 )
        => new( scala * p2.x, scala * p2.y );

    public static Vector2Int operator /( Vector2Int p1, Vector2Int p2 )
    => new( p1.x / p2.x, p1.y / p2.y );

    public static Vector2Int operator /( Vector2Int p1, i32 scala )
        => new( p1.x / scala, p1.y / scala );

    public static Vector2Int operator /( i32 scala, Vector2Int p2 )
        => new( scala / p2.x, scala / p2.y );
    public static bool operator ==( Vector2Int p1, Vector2Int p2 )
        => p1.x == p2.x && p1.y == p2.y;
    public static bool operator !=( Vector2Int p1, Vector2Int p2 )
        => p1.x != p2.x || p1.y != p2.y;
}
