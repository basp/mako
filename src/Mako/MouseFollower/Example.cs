namespace Mako.MouseFollower;

using System.Numerics;

using Raylib_CSharp;
using Raylib_CSharp.Textures;
using Raylib_CSharp.Transformations;
using Raylib_CSharp.Rendering;
using Raylib_CSharp.Windowing;
using Raylib_CSharp.Colors;

internal static class Example
{
    private const int Width = 800;
    private const int Height = 600;

    private static readonly Mover mover =
        new(Example.Width / 2f, Example.Height / 2f);
    
    public static void Run()
    {
        Window.Init(Example.Width, Example.Height, "Test");
        Time.SetTargetFPS(60);
        
        var canvas = RenderTexture2D.Load(Width, Height);
        var source = new Rectangle(
            0,
            0,
            canvas.Texture.Width,
            -canvas.Texture.Height);
        
        while (!Window.ShouldClose())
        {
            Graphics.BeginTextureMode(canvas);
            Graphics.ClearBackground(new Color(230, 230, 230, 5));
            Example.mover.Update();
            Example.mover.CheckEdges(Example.Width, Example.Height);
            Example.mover.Show();
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

