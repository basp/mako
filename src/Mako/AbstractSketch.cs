namespace Mako;

using System.Numerics;

using Raylib_CSharp;
using Raylib_CSharp.Camera.Cam2D;
using Raylib_CSharp.Colors;
using Raylib_CSharp.Rendering;
using Raylib_CSharp.Textures;
using Raylib_CSharp.Transformations;
using Raylib_CSharp.Windowing;

internal abstract class AbstractSketch : ISketch
{
    private Context context = new(
        new Color(0, 0, 0, 0),
        Vector2.Zero,
        0f,
        Color.White,
        Color.Black,
        2f,
        RectMode.CORNER);

    protected AbstractSketch(int width, int height)
    {
        this.Width = width;
        this.Height = height;
    }
    
    private readonly Stack<Context> stack = new();

    public int Width { get; }

    public int Height { get; }

    public void Clear()
    {
        Graphics.ClearBackground(new Color(0, 0, 0, 0));
    }
    
    public void Push()
    {
        this.stack.Push(this.context);
    }

    public void Pop()
    {
        this.context = this.stack.Pop();
    }

    public void Background(Color color)
    {
        // Graphics.ClearBackground(color);
        var rect = new Rectangle(0, 0, this.Width, this.Height);
        Graphics.DrawRectangleRec(rect, color);
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

    public void NoFill()
    {
        this.context = this.context with
        {
            Fill = new Color(0, 0, 0, 0),
        };
    }

    public void NoStroke()
    {
        this.context = this.context with
        {
            Stroke = new Color(0, 0, 0, 0),
        };
    }
    
    public void Circle(float x, float y, float radius)
    {            
        var camera = new Camera2D(
            this.context.Translation, 
            Vector2.Zero, 
            this.context.Rotation, 
            1f);
        Graphics.BeginMode2D(camera);
        var center = new Vector2(x, y);
        Graphics.DrawCircleV(
            center, 
            radius, 
            this.context.Stroke);
        Graphics.DrawCircleV(
            center, 
            radius - this.context.StrokeWeight, 
            this.context.Fill);
        Graphics.EndMode2D();
    }

    public void Line(float x1, float y1, float x2, float y2)
    {
        var camera = new Camera2D(
            this.context.Translation, 
            Vector2.Zero, 
            this.context.Rotation, 
            1f);
        Graphics.BeginMode2D(camera);
        var startPos = new Vector2(x1, y1);
        var endPos = new Vector2(x2, y2);
        Graphics.DrawLineEx(
            startPos, 
            endPos, 
            this.context.StrokeWeight, 
            this.context.Stroke);
        Graphics.EndMode2D();
    }

    public void Rect(float x, float y, float w, float h)
    {
        var camera = new Camera2D(
            this.context.Translation, 
            Vector2.Zero, 
            this.context.Rotation, 
            1f);
        Graphics.BeginMode2D(camera);        
        var rectangle = new Rectangle(x, y, w, h);
        var origin = this.context.RectMode switch
        {
            RectMode.CENTER => new Vector2(w / 2f, h / 2f),
            _ => Vector2.Zero,
        };
        Graphics.DrawRectanglePro(
            rectangle, 
            origin, 
            0f, 
            this.context.Fill);
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
            Graphics.EndDrawing();
        }
        
        Window.Close();
    }
    
    protected virtual void InternalSetup() {}

    protected abstract void InternalDraw(float dt);
    
    private record Context(
        Color Background,
        Vector2 Translation,
        float Rotation,
        Color Fill,
        Color Stroke,
        float StrokeWeight,
        RectMode RectMode);
}