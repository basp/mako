namespace Mako.Examples;

using Raylib_CSharp.Colors;

internal static class Example15
{
    public static void Run()
    {
        const int width = 640;
        const int height = 240;

        var sketch = new Sketch(width, height)
        {
            Draw = (s, _) =>
            {
                s.Background(Color.RayWhite);
                s.Fill(Color.Orange);
                
                s.Push();
                s.Translate(width / 2f, height / 2f);
                s.Fill(Color.SkyBlue);
                s.Circle(0, 0, 64f);
                s.Pop();
                  
                s.Circle(width, height, 64f);

                s.Push();
                s.Translate(width / 4f, height / 4f);
                s.Fill(Color.Lime);
                s.Circle(0, 0, 32f);
                s.Pop();
                  
                s.Circle(0, 0, 64f);
            },
        };
        
        sketch.Run();
    }
}