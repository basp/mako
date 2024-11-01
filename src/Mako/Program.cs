using Raylib_CSharp.Colors;
using Raylib_CSharp.Rendering;

namespace Mako;

internal static class Example12
{
    public static void Run()
    {
        const int width = 640;
        const int height = 240;

        var sketch = new Sketch(width, height)
        {
            Draw = (s, _) =>
            {
                s.Background(Color.White);
                s.Fill(Color.Black);
                s.Circle(320, 120, 32);
            },
        };
        
        sketch.Run();
    }
}

internal static class Example13
{
    public static void Run()
    {
        const int width = 640;
        const int height = 240;

        var sketch = new Sketch(width, height)
        {
            Draw = (_, _) =>
            {
                Graphics.ClearBackground(Color.White);
                Graphics.DrawCircle(
                    width / 2,
                    height / 2,
                    32,
                    Color.Black);
            },
        };
        
        sketch.Run();
    }
}

internal static class Program
{
    public static void Main(string[] _)
    {
        Example12.Run();
    }
}
