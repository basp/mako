using System.Numerics;
using Raylib_CSharp;
using Raylib_CSharp.Camera.Cam2D;
using Raylib_CSharp.Collision;
using Raylib_CSharp.Colors;
using Raylib_CSharp.Interact;
using Raylib_CSharp.Rendering;
using Raylib_CSharp.Transformations;

namespace Mako;

internal class Mover
{
    public Vector2 Position { get; set; }

    public Vector2 Velocity { get; set; }

    public Vector2 Acceleration { get; set; }

    public Vector2 TopSpeed { get; } = new(4f, 4f);

    public Mover(float x, float y)
    {
        this.Velocity = Vector2.Zero;
        this.Acceleration = Vector2.Zero;
        this.Position = new Vector2(x, y);
    }
    
    public void Update()
    {
        var mouse = Input.GetMousePosition();
        var dir = Vector2.Subtract(mouse, this.Position);
        dir = Vector2.Normalize(dir);
        dir = 0.5f * dir;
        this.Acceleration = dir;
        this.Velocity += this.Acceleration;
        this.Velocity = Vector2.Normalize(this.Velocity) * this.TopSpeed;
        this.Position += this.Velocity;
    }

    public void Show()
    {
        var angle = MathF.Atan2(this.Velocity.Y, this.Velocity.X);
        var rect = new Rectangle(-15, -5, 30, 10);
        var camera = new Camera2D(
            this.Position, 
            Vector2.Zero, 
            angle * RayMath.Rad2Deg, 
            1f);
        Graphics.BeginMode2D(camera);
        Graphics.DrawRectangleLinesEx(rect, 2f, Color.Black);
        Graphics.EndMode2D();
    }

    public void CheckEdges(int width, int height)
    {
        if (this.Position.X > width)
        {
            this.Position = new Vector2(0, this.Position.Y);
        }
        else if (this.Position.X < 0)
        {
            this.Position = new Vector2(width, this.Position.Y);
        }

        if (this.Position.Y > height)
        {
            this.Position = new Vector2(this.Position.X, 0);
        }
        else if (this.Position.Y < 0)
        {
            this.Position = new Vector2(this.Position.X, height);
        }
    }
}