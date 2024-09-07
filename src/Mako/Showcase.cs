namespace Mako;

using Raylib_CSharp.Colors;

internal static class Showcase
{
    /// <summary>
    /// A basic sketch that draws a rectangle.
    /// </summary>
    /// <returns>The sketch.</returns>
    internal static ISketch Sketch0()
    {
        const int width = 640;
        const int height = 240;

        return new Sketch(width, height)
        {
            Setup = s =>
            {
                s.Background(Color.White);
            },
            Draw = (s, _) =>
            {
                s.Stroke(Color.Black);
                s.StrokeWeight(8f);
                s.Fill(Color.Lime);
                s.Rect(30, 30, 50, 50);
            },
        };
    }
    
    /// <summary>
    /// A basic wave example.
    /// </summary>
    /// <returns>The sketch.</returns>
    internal static ISketch WaveExample1()
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
    
    /// <summary>
    /// A more complex wave example that introduces different frequencies.
    /// </summary>
    /// <returns>The sketch.</returns>
    internal static ISketch WaveExample2()
    {
        const float twoPi = MathF.PI * 2f;

        const int width = 640;
        const int height = 240;
        
        const int xSpacing = 8;
        const int maxWaves = 16;
        const int w = width + 16;
        
        var amplitude = new float[maxWaves];
        var dx = new float[maxWaves];
        
        var theta = 0f;
        var yValues = new float[w / xSpacing];
        
        return new Sketch(width, height)
        {
            Setup = s =>
            {
                for (var i = 0; i < maxWaves; i++)
                {
                    amplitude[i] = Utils.GetRandomSingle(10, 30);
                    var period = Utils.GetRandomSingle(100, 300);
                    dx[i] = (twoPi / period) * xSpacing;
                }

                yValues = new float[w / xSpacing];

                s.Background(Color.White);
            },
            Draw = (s, _) =>
            {
                s.Background(new Color(255, 255, 255, 200));
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
                        yValues[i] += MathF.Sin(x) * amplitude[j];
                    }
                    else
                    {
                        yValues[i] += MathF.Cos(x) * amplitude[j];
                    }

                    x += dx[j];
                }
            }
        }

        void RenderWave(ISketch s)
        {
            s.Stroke(Color.Black);
            s.Fill(new Color(127, 127, 127, 100));
            for (var x = 0; x < yValues.Length; x++)
            {
                var centerX = x * xSpacing;
                var centerY = s.Height / 2f + yValues[x];
                s.Circle(centerX, centerY, 16);
            }
        }
    }
}
