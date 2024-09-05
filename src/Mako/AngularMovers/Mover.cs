using System.Numerics;
using Raylib_CSharp;
using Raylib_CSharp.Camera.Cam2D;
using Raylib_CSharp.Colors;
using Raylib_CSharp.Rendering;

namespace Mako.AngularMovers;

internal class Mover
{
    private readonly Random random = new();
    
    private float Radius { get; set; }
    
    public Vector2 Position { get; set; }

    public Vector2 Velocity { get; set; }
    
    public Vector2 Acceleration { get; set; }

    public float Mass { get; set; }

    public float Angle { get; set; }
    
    public float AngleVelocity { get; set; }
    
    public float AngleAcceleration { get; set; }

    public Mover(float x, float y, float mass)
    {
        this.Mass = mass;
        this.Radius = this.Mass * 8;
        this.Angle = 0;
        this.AngleVelocity = 0;
        this.AngleAcceleration = 0;
        this.Velocity = new Vector2(
            Utils.GetRandomSingle(-1, 1),
            Utils.GetRandomSingle(-1, 1));
        this.Acceleration = Vector2.Zero;
        this.Position = new Vector2(x, y);
    }

    public void ApplyForce(Vector2 force)
    {
        this.Acceleration += Vector2.Divide(force, this.Mass);
    }
    
    public void Update()
    {
        this.Velocity += this.Acceleration;
        this.Position += this.Velocity;
        this.AngleAcceleration = this.Acceleration.X * 5;
        this.AngleVelocity += this.AngleAcceleration;
        this.AngleVelocity = 
            RayMath.Clamp(this.AngleVelocity, -6f, 6f);
        this.Angle += this.AngleVelocity;
        this.Acceleration = Vector2.Zero;
    }

    public void Show()
    {
        var camera = new Camera2D(
            this.Position, 
            Vector2.Zero, 
            this.Angle, 
            1f);
        Graphics.BeginMode2D(camera);
        Graphics.DrawCircle(0, 0, this.Radius, Color.Gray);
        Graphics.DrawLineEx(
            Vector2.Zero,
            new Vector2(this.Radius, 0),
            2f,
            Color.Black);
        Graphics.EndMode2D();
    }
}