namespace Mako;

using System.Numerics;

using Raylib_CSharp;
using Raylib_CSharp.Camera.Cam2D;
using Raylib_CSharp.Colors;
using Raylib_CSharp.Rendering;
using Raylib_CSharp.Textures;
using Raylib_CSharp.Transformations;
using Raylib_CSharp.Windowing;

public abstract class AbstractSketch : ISketch
{
    private Context context = new(
        Vector2.Zero,
        0f,
        1f,
        Color.White,
        Color.Black,
        2f,
        RectMode.Corner,
        AngleMode.Radians);

    protected AbstractSketch(int width, int height)
    {
        this.Width = width;
        this.Height = height;
    }
    
    private readonly Stack<Context> stack = new();

    public int FrameCount { get; private set; }
    
    /// <inheritdoc cref="ISketch.Width"/> 
    public int Width { get; }

    /// <inheritdoc cref="ISketch.Height"/>
    public int Height { get; }

    public float Radians(float degrees) => degrees * RayMath.Deg2Rad;

    public float Degrees(float radians) => radians * RayMath.Rad2Deg;

    /// <inheritdoc cref="ISketch.Clear"/>
    public void Clear()
    {
        Graphics.ClearBackground(new Color(0, 0, 0, 0));
    }

    public float Lerp(float a, float b, float t) =>
        RayMath.Lerp(a, b, t);
    
    /// <inheritdoc cref="ISketch.Push"/>
    public void Push()
    {
        this.stack.Push(this.context);
    }
    
    /// <inheritdoc cref="ISketch.Pop"/>
    public void Pop()
    {
        this.context = this.stack.Pop();
    }

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
    
    /// <inheritdoc cref="ISketch.Background"/>
    public void Background(Color color)
    {
        var rect = new Rectangle(0, 0, this.Width, this.Height);
        Graphics.DrawRectangleRec(rect, color);
    }

    public void Zoom(float value)
    {
        this.context = this.context with
        {
            Zoom = value,
        };
    }

    /// <inheritdoc cref="ISketch.Translate"/>
    public void Translate(float dx, float dy)
    {
        this.context = this.context with
        {
            Translation = new Vector2(dx, dy),
        };
    }

    /// <inheritdoc cref="ISketch.Rotate"/>
    public void Rotate(float angle)
    {
        this.context = this.context with
        {
            Rotation = angle,
        };
    }

    /// <inheritdoc cref="ISketch.Fill"/>
    public void Fill(Color color)
    {
        this.context = this.context with
        {
            Fill = color,
        };
    }

    /// <inheritdoc cref="ISketch.Stroke"/>
    public void Stroke(Color color)
    {
        this.context = this.context with
        {
            Stroke = color,
        };
    }

    /// <inheritdoc cref="ISketch.StrokeWeight"/>
    public void StrokeWeight(float value)
    {
        this.context = this.context with
        {
            StrokeWeight = value,
        };
    }

    /// <inheritdoc cref="ISketch.SetRectMode"/>
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

    /// <inheritdoc cref="ISketch.NoFill"/>
    public void NoFill()
    {
        this.context = this.context with
        {
            Fill = new Color(0, 0, 0, 0),
        };
    }

    /// <inheritdoc cref="ISketch.NoStroke"/>
    public void NoStroke()
    {
        this.context = this.context with
        {
            Stroke = new Color(0, 0, 0, 0),
        };
    }
    
    /// <inheritdoc cref="ISketch.Circle"/>
    public void Circle(float x, float y, float radius)
    {            
        var camera = this.CreateCamera();
        var center = new Vector2(x, y);
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
    }

    public void Line(float x1, float y1, float x2, float y2)
    {
        var camera = this.CreateCamera();
        var startPos = new Vector2(x1, y1);
        var endPos = new Vector2(x2, y2);
        Graphics.BeginMode2D(camera);
        Graphics.DrawLineEx(
            startPos, 
            endPos, 
            this.context.StrokeWeight, 
            this.context.Stroke);
        Graphics.EndMode2D();
    }

    public void Ellipse(
        float x, 
        float y, 
        float w,
        float h)
    {
        var radiusH = w / 2f;
        var radiusV = h / 2f;
        var camera = this.CreateCamera();
        Graphics.BeginMode2D(camera);
        Graphics.DrawEllipse(
            (int)x,
            (int)y,
            radiusH,
            radiusV,
            this.context.Fill);
        for (var i = 0f; i < this.context.StrokeWeight; i += 0.5f)
        {
            Graphics.DrawEllipseLines(
                (int)x,
                (int)y,
                radiusH - i,
                radiusV - i,
                this.context.Stroke);
        }
        Graphics.EndMode2D();
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
        Graphics.BeginMode2D(camera);
        Graphics.DrawRectangleRec(
            rect,
            this.context.Fill);
        Graphics.DrawRectangleLinesEx(
            rect,
            this.context.StrokeWeight,
            this.context.Stroke);
        Graphics.EndMode2D();
    }

    /// <summary>
    /// Initializes a window and runs the sketch.
    /// </summary>
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
            this.InternalDraw(dt);
            Graphics.EndTextureMode();
            
            Graphics.BeginDrawing();
            Graphics.DrawTextureRec(
                canvas.Texture,
                source,
                Vector2.Zero,
                Color.White);
            // Graphics.DrawFPS(10, 10);
            Graphics.EndDrawing();

            this.FrameCount += 1;
        }
        
        Window.Close();
    }

    public void SetEllipseMode(EllipseMode mode)
    {
        throw new NotImplementedException();
    }

    protected virtual void InternalSetup() {}

    protected abstract void InternalDraw(float dt);

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
    
    private record Context(
        Vector2 Translation,
        float Rotation,
        float Zoom,
        Color Fill,
        Color Stroke,
        float StrokeWeight,
        RectMode RectMode,
        AngleMode AngleMode);
}