namespace Mako.Examples;

using System.Numerics;
using Raylib_CSharp.Colors;
using Raylib_CSharp.Rendering;

internal static class Example08
{
    private static float G = 8;
    
    private class Body
    {
        private readonly Sketch s;

        public Body(Sketch s)
        {
            this.s = s;
        }
        
        public Vector2 Position { get; set; }
        
        public Vector2 Velocity { get; set; }
        
        public Vector2 Acceleration { get; set; }
        
        public float Mass { get; init; }

        public float Radius => MathF.Sqrt(this.Mass) * 2;

        public void ApplyForce(Vector2 force)
        {
            this.Acceleration += Vector2.Divide(force, this.Mass);
        }
        
        public void Attract(Body body)
        {
            var force = this.Position - body.Position;
            var d = force.Length().Constrain(5, 25);
            var str = (G * (this.Mass * body.Mass)) / (d * d);
            force = force.NormalizeMultiply(str);
            body.ApplyForce(force);
        }

        public void Update(float dt)
        {
            this.Velocity += this.Acceleration;
            this.Position += this.Velocity;
            this.Acceleration = Vector2.Zero;
        }

        public void Draw()
        {
            this.s.DrawScoped(scope =>
            {
                this.s.SetBlendMode(BlendMode.Alpha);
                this.s.StrokeWeight(0);
                this.s.Stroke(Color.Black);
                this.s.Fill(new Color(16, 160, 160, 63));
                this.s.Circle(
                    this.Position.X,
                    this.Position.Y,
                    this.Radius * 1f);
            });
        }
    }
    
    public static void Run()
    {
        const int width = 640;
        const int height = 640;

        Body? bodyA = null;
        Body? bodyB = null;

        const float velocity = 2;
        
        var sketch = new Sketch(width, height)
        {
            GlobalBlendMode = BlendMode.Alpha,
            Setup = s =>
            {
                s.Background(Color.Black);

                bodyA = new Body(s)
                {
                    Position = new Vector2(width / 2f, 80),
                    Velocity = new Vector2(velocity, 0),
                    Mass = 16,
                };

                bodyB = new Body(s)
                {
                    Position = new Vector2(width / 2f, height - 80),
                    Velocity = new Vector2(-velocity, 0),
                    Mass = 16,
                };
            },
            Draw = (s, dt) =>
            {
                if (s.FrameCount % 15 == 0)
                {
                    s.Background(new Color(0, 0,0, 4));;
                }
             
                bodyA!.Attract(bodyB!);
                bodyB!.Attract(bodyA!);

                bodyA!.Update(dt);
                bodyA!.Draw();

                bodyB!.Update(dt);
                bodyB!.Draw();
            },
        };
        
        sketch.Run();
    }
}