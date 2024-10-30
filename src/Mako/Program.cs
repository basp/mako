namespace Mako;

using System.Numerics;
using Raylib_CSharp;
using Raylib_CSharp.Colors;
using Raylib_CSharp.Images;
using Raylib_CSharp.Interact;
using Raylib_CSharp.Rendering;
using Raylib_CSharp.Textures;
using Raylib_CSharp.Transformations;
using Raylib_CSharp.Windowing;

internal class Mover
{
    public Vector2 Position { get; set; } = Vector2.Zero;

    public Vector2 Velocity { get; set; } = Vector2.Zero;

    public Vector2 Acceleration { get; set; } = Vector2.Zero;

    public float Mass { get; set; } = 8;

    public Color Color { get; set; } = Color.White;

    public float HMargin { get; set; } = 140f;

    public float VMargin { get; set; } = 20f;
    
    public void ApplyForce(Vector2 force)
    {
        this.Acceleration += (force / this.Mass);
    }

    public void Update(float dt)
    {
        this.Velocity += this.Acceleration;
        this.Position += this.Velocity;
        this.Acceleration = Vector2.Zero;
    }
    
    public void Draw()
    {
        var radius = this.Mass * 1.5f;
        Graphics.DrawCircleV(this.Position, radius, this.Color);
        // Graphics.DrawCircleLinesV(this.Position, radius, Color.Black);
    }

    // private bool AtLeftEdge(float xMin)
    // {
    //     return         
    // }
    //
    // private bool AtRightEdge(float xMax)
    // {
    //     
    // }
    //
    // private bool AtTopEdge(float yMin)
    // {
    //     
    // }
    //
    // private bool AtBottomEdge(float yMax)
    // {
    //     
    // }

    public void Bounce()
    {
        var radius = this.Mass * 2;
        var height = Window.GetRenderHeight();
        var width = Window.GetRenderWidth();
        var yMax = height - radius - this.VMargin;
        var yMin = radius + this.VMargin;
        var xMax = width - radius - this.HMargin;
        var xMin = radius + this.HMargin;
        
        if (this.Position.Y > yMax)
        {
            this.Position = new Vector2(this.Position.X, yMax);
            this.Velocity = new Vector2(this.Velocity.X, -this.Velocity.Y);
        }

        if (this.Position.Y < yMin)
        {
            this.Position = new Vector2(this.Position.X, yMin);
            this.Velocity = new Vector2(this.Velocity.X, -this.Velocity.Y);
        }

        if (this.Position.X > xMax)
        {
            this.Position = new Vector2(xMax, this.Position.Y);
            this.Velocity = new Vector2(-this.Velocity.X, this.Velocity.Y);
        }

        if (this.Position.X < xMin)
        {
            this.Position = new Vector2(xMin, this.Position.Y);
            this.Velocity = new Vector2(-this.Velocity.X, this.Velocity.Y);
        }
    }
}

internal static class Program
{
    public static void Main(string[] _)
    {
        // var sketch = Program.Sandbox();
        // sketch.Run();

        const int marginH = 480;
        const int marginV = 128;
        
        const int width = 1280;
        const int height = 960;

        const int N = 200;
        
        var t = 0f;
        var random = new Random();

        var movers = Enumerable
            .Range(0, N)
            .Select(_ =>
            {
                var x = RayMath.Remap(
                    random.NextSingle(),
                    0f,
                    1f,
                    0,
                    640);
                var y = RayMath.Remap(
                    random.NextSingle(),
                    0f,
                    1f,
                    30,
                    180);
                
                var dx = RayMath.Remap(
                    random.NextSingle(),
                    0,
                    1,
                    -2,
                    2);
                var dy = RayMath.Remap(
                    random.NextSingle(),
                    0,
                    1,
                    -2,
                    2);
                
                var mass = RayMath.Remap(
                    random.NextSingle(),
                    0f,
                    1f,
                    1,
                    8);
                
                const byte r = 0;
                var g = (byte)RayMath.Remap(
                    random.NextSingle(),
                    0f,
                    1f,
                    63,
                    255);
                // var b = (byte)RayMath.Remap(
                //     random.NextSingle(),
                //     0f,
                //     1f,
                //     160,
                //     210);
                const byte b = 140;
                // var a = (byte)RayMath.Remap(
                //     mass,
                //     8,
                //     1,
                //     120,
                //     255);
                const byte a = 255;
                
                return new Mover
                {
                    Position = new Vector2(x, y),
                    Mass = mass,
                    Color = new Color(r, g, b, a),
                    Velocity = new Vector2(dx, dy),
                    HMargin = marginH,
                    VMargin = marginV,
                };
            })
            .ToArray();
        
        Window.Init(width, height, "Sketch");
        Time.SetTargetFPS(60);
        
        var spriteImage = Image.Load(@"C:\temp\assets\PNG\Default\ship_F.png");
        var sprite = Texture2D.LoadFromImage(spriteImage);
        spriteImage.Unload();
        
        var canvas = RenderTexture2D.Load(width, height);
        var source = new Rectangle(
            0,
            0,
            canvas.Texture.Width,
            -canvas.Texture.Height);

        Graphics.BeginTextureMode(canvas);
        Graphics.ClearBackground(Color.White);
        Graphics.EndTextureMode();
        
        while (!Window.ShouldClose())
        {
            var dt = Time.GetFrameTime();

            t += dt * 0.1f;
            
            // Update
            foreach (var mover in movers)
            {
                var gravity = new Vector2(0, 0.1f) * mover.Mass;
                mover.ApplyForce(gravity);
                
                if (Input.IsMouseButtonDown(MouseButton.Left))
                {
                    var mouseX = Input.GetMouseX();
                    var mouseY = Input.GetMouseY();
                    var windX = mouseX > (width / 2)
                        ? MathF.Cos(t)
                        : -MathF.Cos(t);
                    var windY = mouseY > (height / 2)
                        ? MathF.Sin(t)
                        : -MathF.Sin(t);
                    var wind = new Vector2(windX, windY);
                    mover.ApplyForce(wind * 0.1f);
                }

                mover.Update(dt);
                mover.Bounce();
            }
            
            // Draw
            Graphics.BeginTextureMode(canvas);
            Graphics.BeginBlendMode(BlendMode.Alpha);
            Graphics.DrawRectangle(
                0, 
                0, 
                width, 
                height, 
                new Color(0, 0, 0, 25));
            Graphics.EndBlendMode();

            foreach (var mover in movers)
            {
                mover.Draw();
            }

            const int r = 64;
            var g = (byte)RayMath.Remap(
                MathF.Cos(t),
                0,
                1,
                63,
                255);
            var b = (byte)RayMath.Remap(
                MathF.Sin(t),
                0,
                1,
                63,
                255);

            const float thick = 10f;
            var frameColor = new Color(r, g, b, 255);
            Graphics.DrawLineEx(
                new Vector2(marginH, marginV),
                new Vector2(marginH, height - marginV),
                thick,
                frameColor);
            Graphics.DrawLineEx(
                new Vector2(width - marginH, marginV),
                new Vector2(width - marginH, height - marginV),
                thick,
                frameColor);
            Graphics.DrawLineEx(
                new Vector2(marginH, marginV),
                new Vector2(width - marginH, marginV),
                thick,
                frameColor);
            Graphics.DrawLineEx(
                new Vector2(marginH, height - marginV),
                new Vector2(width - marginH, height - marginV),
                thick,
                frameColor);
            Graphics.EndTextureMode();
            
            // Draw to screen buffer
            Graphics.BeginDrawing();
            Graphics.DrawTextureRec(
                canvas.Texture,
                source,
                Vector2.Zero,
                Color.White);
            Graphics.DrawFPS(10, 10);
            Graphics.EndDrawing();
        }
        
        Window.Close();
    }
    
    private static ISketch Sandbox()
    {
        const int width = 640;
        const int height = 240;
        
        return new Sketch(width, height)
        {
            Draw = (s, dt) =>
            {
                s.Background(Color.White);
                s.Stroke(Color.Black);
                s.StrokeWeight(5f);
                s.Fill(Color.RayWhite);
                s.Ellipse(320, 120, 120, 80);
            },
        };
    }    
}
