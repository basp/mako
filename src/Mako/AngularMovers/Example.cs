using Raylib_CSharp;
using Raylib_CSharp.Colors;
using Raylib_CSharp.Rendering;
using Raylib_CSharp.Windowing;

namespace Mako.AngularMovers;

public static class Example
{
    private const int Width = 640;
    private const int Height = 240;

    private static Mover[] movers;
    private static Attractor attractor;
    
    public static void Run()
    {
        Example.movers = Enumerable.Range(0, 50)
            .Select(_ =>
            {
                var x = Utils.GetRandomSingle(Example.Width);
                var y = Utils.GetRandomSingle(Example.Height);
                var mass = Utils.GetRandomSingle(0.1f, 2);
                return new Mover(x, y, mass);
            })
            .ToArray();

        Example.attractor = new Attractor(
            Example.Width / 2f,
            Example.Height / 2f,
            20f); 
        
        Window.Init(Example.Width, Example.Height, "Mako");
        Time.SetTargetFPS(60);

        while (!Window.ShouldClose())
        {
            Graphics.BeginDrawing();
            Graphics.ClearBackground(Color.White);
            
            Example.attractor.Display();
            
            foreach (var mover in Example.movers)
            {
                var force = Example.attractor.Attract(mover);
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