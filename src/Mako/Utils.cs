namespace Mako;

using System.Numerics;

using Raylib_CSharp;

public static class Utils
{
    private static readonly Random random = new();

    public static float ToDegrees(float radians) =>
        radians * RayMath.Rad2Deg;

    public static float ToRadians(float degrees) =>
        degrees * RayMath.Deg2Rad;
    
    public static float GetRandomSingle(float max) =>
        max * Utils.random.NextSingle();
    
    public static float GetRandomSingle(float min, float max)
    {
        var t = Utils.random.NextSingle();
        return RayMath.Lerp(min, max, t);
    }

    public static Vector2 Vector2FromAngle(float angle, float length = 1f) =>
        new Vector2(
            length * MathF.Cos(angle),
            length * MathF.Sin(angle));
}