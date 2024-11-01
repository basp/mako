namespace Mako.Examples;

using Raylib_CSharp.Colors;

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