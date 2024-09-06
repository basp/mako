namespace Mako;

using System.Numerics;

using Raylib_CSharp;

public static class Extensions
{
    public static float HeadingInDegrees(this Vector2 u) =>
        MathF.Atan2(u.Y, u.X) * RayMath.Rad2Deg;
}

