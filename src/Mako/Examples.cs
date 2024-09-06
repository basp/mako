using Raylib_CSharp.Colors;

namespace Mako;

public static class Examples_
{
    public static ISketch Sketch1()
    {
        const int width = 1280;
        const int height = 720;

        const int N = 40;

        var deltas = Enumerable.Range(0, N)
            .Select(i => i * MathF.PI * 2f / N)
            .ToArray();
            
        return new Sketch(width, height)
        {
            Setup = s =>
            {
                s.Background(Color.White);
            },
            Draw = (s, _) =>
            {
                s.SetAngleMode(AngleMode.Radians);
                s.Fill(Color.White);
                s.Stroke(Color.Black);
                s.StrokeWeight(2.5f);

                for(var i = 0; i < deltas.Length; i++)
                {
                    var d = deltas[i];
                        
                    var x = (width / 6f - 20f) * MathF.Cos(d);
                    var y = (height / 6f - 20f) * MathF.Sin(d);

                    var rotation = (MathF.PI * 2) * ((float)i / deltas.Length);

                    s.Push();
                    s.Rotate(rotation);
                    s.Translate(width / 2f, height / 2f);
                    s.Zoom(1.6f);
                    s.Circle(x, y, 4);
                    s.Pop();
                }

                s.Background(new Color(255, 255, 255, 2));

                for (var i = 0; i < deltas.Length; i++)
                {
                    deltas[i] += 0.02f;
                }
            },
        };
    }
}
