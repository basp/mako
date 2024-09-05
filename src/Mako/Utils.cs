namespace Mako;

using Raylib_CSharp;

public static class Utils
{
    private static readonly Random random = new();

    public static float GetRandomSingle(float max) =>
        max * Utils.random.NextSingle();
    
    public static float GetRandomSingle(float min, float max)
    {
        var t = Utils.random.NextSingle();
        return RayMath.Lerp(min, max, t);
    }
}