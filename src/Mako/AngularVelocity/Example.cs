namespace Mako;

using System.Numerics;

using Raylib_CSharp;
using Raylib_CSharp.Camera.Cam2D;
using Raylib_CSharp.Colors;
using Raylib_CSharp.Rendering;
using Raylib_CSharp.Textures;
using Raylib_CSharp.Transformations;
using Raylib_CSharp.Windowing;

public static class Example
{
    public static void Run()
    {
        const int Width = 640;
        const int Height = 240;

        Window.Init(Width, Height, "Mako");
        Time.SetTargetFPS(60);

        var canvas = RenderTexture2D.Load(Width, Height);
        var source = new Rectangle(
            0,
            0,
            canvas.Texture.Width,
            canvas.Texture.Height);

        var offset = new Vector2(Width / 2f, Height / 2f);
        var target = Vector2.Zero;
        var camera = new Camera2D(offset, target, 0, 1);

        var angle = 0f;
        var angleVelocity = 0f;
        var angleAcceleration = 0.006f;

        while (!Window.ShouldClose())
        {
            camera.Rotation = angle;

            Graphics.BeginTextureMode(canvas);
            Graphics.BeginMode2D(camera);
            Graphics.ClearBackground(new Color(255, 255, 255, 10));
            Graphics.DrawLine(-50, 0, 50, 0, Color.Black);
            Graphics.DrawCircle(-50, 0, 8, Color.Black);
            Graphics.DrawCircle(50, 0, 8, Color.Black);
            Graphics.EndMode2D();
            Graphics.EndTextureMode();
    
            Graphics.BeginDrawing();
            Graphics.DrawTextureRec(
                canvas.Texture,
                source,
                Vector2.Zero,
                Color.White);
            Graphics.DrawFPS(10, 10);
            Graphics.EndDrawing();

            angleVelocity += angleAcceleration;
            angle += angleVelocity;
        }

        Window.Close();
    }
}