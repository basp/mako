namespace Mako;

using System.Numerics;

public static class Vector
{
    private static readonly Random random = new();
    
    public static Vector2 FromAngle(
        float angle, 
        float length = 1, 
        AngleMode angleMode = AngleMode.Radians)
    {
        angle = angleMode switch
        {
            AngleMode.Degrees => Utils.ToRadians(angle),
            _ => angle,
        };

        var x = length * MathF.Cos(angle);
        var y = length * MathF.Sin(angle);
        
        return new Vector2(x, y);
    }

    public static Vector2 Random2D()
    {
        var angle = Vector.random.NextSingle() * 2 * MathF.PI;
        return Vector.FromAngle(angle);
    }
}