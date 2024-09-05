// See https://aka.ms/new-console-template for more information

using System.Numerics;

using Raylib_CSharp;
using Raylib_CSharp.Colors;
using Raylib_CSharp.Rendering;
using Raylib_CSharp.Textures;
using Raylib_CSharp.Transformations;
using Raylib_CSharp.Windowing;

const int Width = 640;
const int Height = 240;

var bodyA = new Body(320, 40)
{
    Velocity = new Vector2(1, 0),
};

var bodyB = new Body(320, 200)
{
    Velocity = new Vector2(-1, 0),
};

Window.Init(Width, Height, "Mako");
Time.SetTargetFPS(60);

var canvas = RenderTexture2D.Load(Width, Height);
var source = new Rectangle(
    0,
    0,
    canvas.Texture.Width,
    canvas.Texture.Height);

while (!Window.ShouldClose())
{
    bodyA.Attract(bodyB);
    bodyB.Attract(bodyA);
    
    bodyA.Update();
    bodyB.Update();

    Graphics.BeginTextureMode(canvas);
    Graphics.ClearBackground(Color.White);
    bodyA.Show();
    bodyB.Show();
    Graphics.EndTextureMode();
    
    Graphics.BeginDrawing();
    Graphics.DrawTextureRec(
        canvas.Texture,
        source,
        Vector2.Zero,
        Color.White);
    Graphics.EndDrawing();
}

Window.Close();

internal class Body
{
    public Body(float x, float y, float mass = 8f)
    {
        this.Position = new Vector2(x, y);
        this.Velocity = Vector2.Zero;
        this.Acceleration = Vector2.Zero;
        this.Mass = mass;
        this.Radius = MathF.Sqrt(this.Mass) * 2f;
    }
    
    public Vector2 Position { get; set; }

    public Vector2 Velocity { get; set; }

    public Vector2 Acceleration { get; set; }

    public float Mass { get; set; }
    
    public float Radius { get; set; }

    public void Attract(Body other)
    {
        const float G = 1f;
        var force = Vector2.Subtract(
            this.Position,
            other.Position);
        var d = RayMath.Clamp(
            force.Length(), 
            5, 
            25);
        var mag = (G * this.Mass * other.Mass) / (d * d);
        force = mag * Vector2.Normalize(force);
        other.ApplyForce(force);
    }

    public void ApplyForce(Vector2 force)
    {
        this.Acceleration += Vector2.Divide(force, this.Mass);
    }

    public void Update()
    {
        this.Velocity += this.Acceleration;
        this.Position += this.Velocity;
        this.Acceleration = Vector2.Zero;
    }

    public void Show()
    {
        var x = (int)this.Position.X;
        var y = (int)this.Position.Y;
        var fill = new Color(127, 127, 127, 100);
        Graphics.DrawCircle(x, y, 4 * this.Radius, fill);
    }
}