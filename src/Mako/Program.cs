namespace Mako;

using Raylib_CSharp.Colors;

internal static class Program
{
    public static void Main(string[] _)
    {
        var sketch = Sketch1();
        sketch.Run();
        return;
        
        ISketch Sketch1()
        {
            const int width = 640;
            const int height = 240;
            var angle = 0f;
            return new Sketch(width, height)
            {
                Setup = s =>
                {
                    s.Background(new Color(40, 240, 220, 255));
                },
                Draw = (s, _) =>
                {
                    var x = (width / 2f - 20f) * MathF.Cos(angle);
                    var y = (height / 2f - 50f) * MathF.Sin(angle);
                    s.Fill(Color.White);
                    s.Stroke(Color.Black);
                    s.Push();
                    s.Translate(width / 2f, height / 2f);
                    s.Rotate(90);
                    s.Circle(x, y, 16);
                    s.Pop();
                    s.Background(new Color(40, 240, 200, 50));
                    angle += 0.025f;
                },
            };
        }
    }
}
