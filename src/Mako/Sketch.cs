namespace Mako;

using System.Numerics;
using Raylib_CSharp.Camera.Cam2D;
using Raylib_CSharp.Rendering;
using Raylib_CSharp.Textures;
using Raylib_CSharp.Transformations;
using Raylib_CSharp;
using Raylib_CSharp.Colors;
using Raylib_CSharp.Windowing;

public class Sketch
{
    private Context context = new(
        Vector2.Zero,
        0f,
        1f,
        Color.White,
        Color.Black,
        2f,
        RectMode.Center,
        AngleMode.Radians,
        BlendMode.Alpha);
    
    private readonly Random random = new();
    
    private readonly Stack<Context> stack = new();
    
    public Sketch(int width, int height)
    {
        this.Width = width;
        this.Height = height;
    }

    public bool DrawFPS { get; set; } = false;
    
    public long FrameCount { get; private set; }
    
    public int Width { get; }
    
    public int Height { get; }

    public BlendMode GlobalBlendMode { get; init; } = BlendMode.Alpha;

    public Action<Sketch> Setup { get; init; } = _ => { };

    public Action<Sketch, float> Draw { get; init; } = (_, _) => { };
    
    public float Lerp(float a, float b, float t) =>
        RayMath.Lerp(a, b, t);
    
    public float Map(
        float value,
        float start1,
        float stop1,
        float start2,
        float stop2) =>
        RayMath.Remap(
            value, 
            start1, 
            stop1, 
            start2, 
            stop2);  
    
    public void Push()
    {
        this.stack.Push(this.context);
    }
    
    public void Pop()
    {
        this.context = this.stack.Pop();
    }
    
    public void Zoom(float value)
    {
        this.context = this.context with
        {
            Zoom = value,
        };
    }

    public void Translate(float dx, float dy)
    {
        this.context = this.context with
        {
            Translation = new Vector2(dx, dy),
        };
    }

    public void Rotate(float angle)
    {
        this.context = this.context with
        {
            Rotation = angle,
        };
    }
  
    public void Background(Color color)
    {
        var rect = new Rectangle(0, 0, this.Width, this.Height);
        
        Graphics.BeginBlendMode(this.context.BlendMode);
        Graphics.DrawRectangleRec(rect, color);
        Graphics.EndBlendMode();
    }
    
    public void Clear()
    {
        Graphics.ClearBackground(new Color(0, 0, 0, 0));
    }
    
    public void Fill(Color color)
    {
        this.context = this.context with
        {
            Fill = color,
        };
    }

    public void Stroke(Color color)
    {
        this.context = this.context with
        {
            Stroke = color,
        };
    }

    public void StrokeWeight(float value)
    {
        this.context = this.context with
        {
            StrokeWeight = value,
        };
    }

    public void SetRectMode(RectMode mode)
    {
        this.context = this.context with
        {
            RectMode = mode,
        };
    }
    
    public void SetAngleMode(AngleMode mode)
    {
        this.context = this.context with
        {
            AngleMode = mode,
        };
    }

    public void SetBlendMode(BlendMode mode)
    {
        this.context = this.context with
        {
            BlendMode = mode,
        };
    }
    
    public void Circle(float x, float y, float radius)
    {            
        var camera = this.CreateCamera();
        var center = new Vector2(x, y);
        
        Graphics.BeginBlendMode(this.context.BlendMode);
        Graphics.BeginMode2D(camera);
        Graphics.DrawCircleV(
            center, 
            radius, 
            this.context.Fill);
        Graphics.DrawRing(
            center, 
            radius - this.context.StrokeWeight,
            radius,
            0,
            360,
            0,
            this.context.Stroke);
        Graphics.EndMode2D();
        Graphics.EndBlendMode();
    }

    public void Line(float x1, float y1, float x2, float y2)
    {
        var camera = this.CreateCamera();
        var startPos = new Vector2(x1, y1);
        var endPos = new Vector2(x2, y2);
        
        Graphics.BeginBlendMode(this.context.BlendMode);
        Graphics.BeginMode2D(camera);
        Graphics.DrawLineEx(
            startPos, 
            endPos, 
            this.context.StrokeWeight, 
            this.context.Stroke);
        Graphics.EndMode2D();
        Graphics.EndBlendMode();
    }    
    
    public void Rect(float x, float y, float w, float h)
    {
        var camera = this.CreateCamera();
        var origin = this.context.RectMode switch
        {
            RectMode.Center => new Vector2(x - (w / 2f), y - (h / 2f)),
            _ => new Vector2(x, y),
        };
        
        var rect = new Rectangle(origin.X, origin.Y, w, h);
        
        Graphics.BeginBlendMode(this.context.BlendMode);
        Graphics.BeginMode2D(camera);
        Graphics.DrawRectangleRec(
            rect,
            this.context.Fill);
        Graphics.DrawRectangleLinesEx(
            rect,
            this.context.StrokeWeight,
            this.context.Stroke);
        Graphics.EndMode2D();
        Graphics.EndBlendMode();
    }

    public void DrawScoped(Action<Sketch> sketch)
    {
        this.Push();
        sketch(this);
        this.Pop();
    }
        
    public void Run()
    {
        Window.Init(this.Width, this.Height, "Sketch");
        Time.SetTargetFPS(60);
        
        var canvas = RenderTexture2D.Load(this.Width, this.Height);
        var source = new Rectangle(
            0,
            0,
            canvas.Texture.Width,
            -canvas.Texture.Height);
        
        Graphics.BeginTextureMode(canvas);
        this.InternalSetup();
        Graphics.EndTextureMode();
        
        while (!Window.ShouldClose())
        {
            var dt = Time.GetFrameTime();
            
            Graphics.BeginTextureMode(canvas);
            Graphics.BeginBlendMode(BlendMode.Alpha);
            this.InternalDraw(dt);
            Graphics.EndBlendMode();
            Graphics.EndTextureMode();
            
            Graphics.BeginDrawing();
            Graphics.BeginBlendMode(this.GlobalBlendMode);
            Graphics.DrawTextureRec(
                canvas.Texture,
                source,
                Vector2.Zero,
                Color.White);
            Graphics.EndBlendMode();
            if (this.DrawFPS)
            {
                Graphics.DrawFPS(10, 10);
            }
            
            Graphics.EndDrawing();

            this.FrameCount += 1;
        }
        
        Window.Close();
    }

    protected virtual void InternalSetup()
    {
        this.Setup(this);
    }

    protected virtual void InternalDraw(float dt)
    {
        this.Draw(this, dt);
    }
    
    private Camera2D CreateCamera()
    {
        var rotation = this.context.AngleMode switch
        {
            AngleMode.Radians => Utils.ToDegrees(this.context.Rotation),
            _ => this.context.Rotation,
        };
            
        return new Camera2D(
            this.context.Translation, 
            Vector2.Zero, 
            rotation,
            this.context.Zoom);   
    }
    
    private record struct Context(
        Vector2 Translation,
        float Rotation,
        float Zoom,
        Color Fill,
        Color Stroke,
        float StrokeWeight,
        RectMode RectMode,
        AngleMode AngleMode,
        BlendMode BlendMode);
}