using Raylib_CSharp.Camera.Cam2D;
using Raylib_CSharp.Colors;
using Raylib_CSharp.Rendering;

namespace Mako;

using System.Numerics;

using Raylib_CSharp;
using Raylib_CSharp.Textures;
using Raylib_CSharp.Transformations;
using Raylib_CSharp.Windowing;

internal static class Program
{
    private const int Width = 640;
    private const int Height = 240;

    private static Mover[] movers;
    private static Attractor attractor;
    
    public static void Main(string[] args)
    {
        Program.movers = Enumerable.Range(0, 50)
            .Select(_ =>
            {
                var x = Utils.GetRandomSingle(Program.Width);
                var y = Utils.GetRandomSingle(Program.Height);
                var mass = Utils.GetRandomSingle(0.1f, 2);
                return new Mover(x, y, mass);
            })
            .ToArray();

        Program.attractor = new Attractor(
            Program.Width / 2f,
            Program.Height / 2f,
            20f); 
        
        Window.Init(Width, Height, "Mako");
        Time.SetTargetFPS(60);

        while (!Window.ShouldClose())
        {
            Graphics.BeginDrawing();
            Graphics.ClearBackground(Color.White);
            
            Program.attractor.Display();
            
            foreach (var mover in Program.movers)
            {
                var force = Program.attractor.Attract(mover);
                mover.ApplyForce(force);
                mover.Update();
                mover.Show();
            }
            
            Graphics.DrawFPS(10, 10);
            Graphics.EndDrawing();
        }
        
        Window.Close();
    }
}

