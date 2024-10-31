using System.Runtime.CompilerServices;

namespace Mako;

using System.Numerics;

using Raylib_CSharp;

public static class Extensions
{
    public static float HeadingInDegrees(this Vector2 u) =>
        MathF.Atan2(u.Y, u.X) * RayMath.Rad2Deg;

    public static Vector2 Limit(this Vector2 u, float max)
    {
        var magSq = u.LengthSquared();
        if (magSq > max * max)
        {
            return Vector2.Divide(u, MathF.Sqrt(magSq)) * max;
        }

        return u;
    }

    public static Vector2 NormalizeMultiply(this Vector2 u, float s) =>
        Vector2.Normalize(u) * s;
}
