using System.Numerics;

using Raylib_CSharp;
using Raylib_CSharp.Camera.Cam2D;
using Raylib_CSharp.Rendering;
using Raylib_CSharp.Windowing;
using Raylib_CSharp.Colors;
using Raylib_CSharp.Transformations;

namespace Mako;

internal static class Program
{
    private const int Width = 640;
    private const int Height = 240;

    private static readonly Mover mover = new(320, 120);
    
    public static void Main(string[] args)
    {
        Window.Init(Program.Width, Program.Height, "Test");
        Time.SetTargetFPS(60);
        
        while (!Window.ShouldClose())
        {
            Graphics.BeginDrawing();
            Graphics.ClearBackground(Color.RayWhite);
            Program.mover.Update();
            Program.mover.CheckEdges(Program.Width, Program.Height);
            Program.mover.Show();
            Graphics.EndDrawing();
        }
        
        Window.Close();
    }
}

