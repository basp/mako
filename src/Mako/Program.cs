namespace Mako;

using Raylib_CSharp.Colors;

internal static class Program
{
    public static void Main(string[] _)
    {
        var sketch = Program.WaveExample2();
        sketch.Run();
    }

    private static ISketch WaveExample1()
    {
        const float deltaAngle = 0.1f;
        var startAngle = 0f;
        return new Sketch(640, 240)
        {
            Draw = (s, _) =>
            {
                s.Background(Color.White);
                var angle = startAngle;
                for (var x = 0; x < s.Width; x += 24)
                {
                    var y = s.Map(
                        MathF.Sin(angle),
                        -1,
                        1,
                        0,
                        s.Height);
                    s.Stroke(Color.Black);
                    s.Fill(new Color(127, 127, 127, 127));
                    s.Circle(x, y, 24);
                    angle += deltaAngle;
                }

                startAngle += 0.02f;
            },
        };
    }

    private static ISketch WaveExample2()
    {
        const float TwoPi = MathF.PI * 2f;
        const int xSpacing = 8;
        const int maxWaves = 5;
        
        int w;
        
        var theta = 0f;
        var amplitude = new float[maxWaves];
        var dx = new float[maxWaves];
        var yValues = Array.Empty<float>();
        
        return new Sketch(640, 240)
        {
            Setup = s =>
            {
                w = s.Width + 16;

                for (var i = 0; i < maxWaves; i++)
                {
                    amplitude[i] = Utils.GetRandomSingle(10, 30);
                    var period = Utils.GetRandomSingle(100, 300);
                    dx[i] = (TwoPi / period) * xSpacing;
                }

                yValues = new float[w / xSpacing];
            },
            Draw = (s, _) =>
            {
                s.Background(new Color(255, 255, 255, 224));
                UpdateWave();
                RenderWave(s);
            },
        };

        void UpdateWave()
        {
            theta += 0.02f;

            for (var i = 0; i < yValues.Length; i++)
            {
                yValues[i] = 0f;
            }

            for (var j = 0; j < maxWaves; j++)
            {
                var x = theta;
                for (var i = 0; i < yValues.Length; i++)
                {
                    if (j % 2 == 0)
                    {
                        yValues[i] += MathF.Sin(x) * amplitude![j];
                    }
                    else
                    {
                        yValues[i] += MathF.Cos(x) * amplitude![j];
                    }

                    x += dx![j];
                }
            }
        }

        void RenderWave(ISketch s)
        {
            s.Stroke(Color.Black);
            s.Fill(new Color(0, 0, 0, 100));
            for (var x = 0; x < yValues.Length; x++)
            {
                var centerX = x * xSpacing;
                var centerY = s.Height / 2f + yValues[x];
                s.Circle(centerX, centerY, 16);
            }
        }
    }
}
