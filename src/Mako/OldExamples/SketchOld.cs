namespace Mako.OldExamples;

using System.Numerics;

using Raylib_CSharp;
using Raylib_CSharp.Camera.Cam2D;
using Raylib_CSharp.Colors;
using Raylib_CSharp.Rendering;
using Raylib_CSharp.Textures;
using Raylib_CSharp.Transformations;
using Raylib_CSharp.Windowing;

public abstract class SketchOld
{
    protected readonly int width;
    protected readonly int height;

    protected SketchOld(int width, int height)
    {
        this.width = width;
        this.height = height;
    }
    
    protected virtual void Setup() {}

    protected abstract void Draw();

    public void Run()
    {
        Window.Init(width, height, "Sketch");
        Time.SetTargetFPS(60);

        var canvas = RenderTexture2D.Load(width, height);
        var source = new Rectangle(
            0,
            0,
            canvas.Texture.Width,
            -canvas.Texture.Height);

        var offset = new Vector2(width / 2f, height / 2f);
        var camera = new Camera2D(offset, Vector2.Zero, 0, 1f);
        
        this.Setup();

        Graphics.BeginTextureMode(canvas);
        Graphics.ClearBackground(Color.White);
        Graphics.EndTextureMode();
        
        while (!Window.ShouldClose())
        {
            Graphics.BeginTextureMode(canvas);
            Graphics.BeginMode2D(camera);
            this.Draw();
            Graphics.EndMode2D();
            Graphics.DrawRectangle(
                0,
                0,
                width,
                height,
                new Color(255, 255, 255, 30));
            Graphics.EndTextureMode();
            
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
}