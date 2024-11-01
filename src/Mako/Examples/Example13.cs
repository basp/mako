namespace Mako.Examples;

using Raylib_CSharp.Colors;
using Raylib_CSharp.Rendering;

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