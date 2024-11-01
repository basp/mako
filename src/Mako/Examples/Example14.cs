namespace Mako.Examples;

using Raylib_CSharp.Colors;

internal static class Example14
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
                  
                s.DrawScoped(scope =>
                {
                    scope.Translate(width / 2f, height / 2f);
                    scope.Fill(Color.SkyBlue);
                    scope.Circle(0, 0, 64f);
                });
                  
                s.Circle(width, height, 64f);
                  
                s.DrawScoped(scope =>
                {
                    scope.Translate(width / 4f, height / 4f);
                    scope.Fill(Color.Lime);
                    scope.Circle(0, 0, 32f);
                });
                  
                s.Circle(0, 0, 64f);
            },
        };
        
        sketch.Run();
    }
}