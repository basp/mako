using System.Numerics;
using Raylib_CSharp;
using Raylib_CSharp.Colors;
using Raylib_CSharp.Rendering;

namespace Mako;

internal class Attractor
{
    public Vector2 Position { get; set; }

    public float Mass { get; set; }

    public float G { get; set; } = 1f;

    public Attractor(float x, float y, float mass)
    {
        this.Position = new Vector2(x, y);
        this.Mass = mass;
    }

    public Vector2 Attract(Mover mover)
    {
        var force = Vector2.Subtract(
            this.Position, 
            mover.Position);
        var distance =
            RayMath.Clamp(
                RayMath.Vector2Length(force),
                5,
                25);
        var strength = 
            (this.G * this.Mass * mover.Mass) / (distance * distance);
        return strength * Vector2.Normalize(force);
    }

    public void Display()
    {
        Graphics.DrawCircle(
            (int)this.Position.X, 
            (int)this.Position.Y, 
            this.Mass,
            new Color(175, 175, 175, 200));
    }
}