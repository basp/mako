namespace Mako;

using System.Numerics;

using Raylib_CSharp.Colors;

public static class Examples
{
    public static ISketch Sketch0()
    {
        const int width = 1280;
        const int height = 720;

        const int N = 32;

        var deltas = Enumerable.Range(0, N)
            .Select(i => i * MathF.PI * 2f / N)
            .ToArray();

        var oscillators = Enumerable
            .Range(0, N / 8)
            .Select(_ => new Oscillator(width, height))
            .ToArray();

        return new Sketch(width, height)
        {
            Setup = s =>
            {
                s.Background(Color.White);
            },
            Draw = (s, dt) =>
            {
                Layer1(s, dt);
                Layer2(s, dt);
                Layer3(s, dt);
            },
        };

        void Layer1(ISketch s, float _)
        {
            s.SetAngleMode(AngleMode.Radians);
            s.Fill(Color.Pink);
            s.Stroke(Color.Black);
            s.StrokeWeight(2.5f);

            for (var i = 0; i < deltas.Length; i++)
            {
                var d = deltas[i];

                var x = (width / 6f - 20f) * MathF.Cos(d);
                var y = (height / 6f - 20f) * MathF.Sin(d);

                var rotation = (MathF.PI * 2) * ((float)i / deltas.Length);

                s.Push();
                s.Rotate(rotation);
                s.Translate(width / 2f, height / 2f);
                s.Circle(x, y, 8);
                s.Pop();
            }

            for (var i = 0; i < deltas.Length; i++)
            {
                deltas[i] += 0.02f;
            }
        }

        void Layer2(ISketch s, float _)
        {
            foreach (var osc in oscillators)
            {
                osc.Update();
                osc.Show(s);
            }
        }

        void Layer3(ISketch s, float _)
        {
            if (s.FrameCount % 4 == 0)
            {
                s.Background(new Color(255, 255, 255, 3));
            }
        }
    }

    private class Oscillator(int width, int height)
    {
        private Vector2 angle = Vector2.Zero;

        private readonly Vector2 angleVelocity = new(
            Utils.GetRandomSingle(-0.05f, 0.05f),
            Utils.GetRandomSingle(-0.05f, 0.05f));

        private readonly Vector2 amplitude = new(
            Utils.GetRandomSingle(20, width / 2f),
            Utils.GetRandomSingle(20, height / 2f));

        public void Update()
        {
            this.angle += this.angleVelocity;
        }

        public void Show(ISketch s)
        {
            var x = MathF.Sin(this.angle.X) * this.amplitude.X;
            var y = MathF.Sin(this.angle.Y) * this.amplitude.Y;
            s.Push();
            s.Translate(s.Width / 2f, s.Height / 2f);
            s.Stroke(Color.Black);
            s.StrokeWeight(1f);
            s.Fill(new Color(20, 200, 240, 255));
            s.Circle(x, y, 5);
            s.Pop();
        }
    }
}
