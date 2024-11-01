namespace Mako.Examples;

using System.Numerics;
using Raylib_CSharp.Colors;

internal static class Example07
{
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
            var G = 1f;
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
            this.s.Push();
            this.s.StrokeWeight(2);
            this.s.Stroke(Color.Black);
            this.s.Fill(Color.SkyBlue);
            this.s.Circle(
                this.Position.X,
                this.Position.Y,
                this.Radius * 4);
            this.s.Pop();
        }
    }
    
    public static void Run()
    {
        const int width = 640;
        const int height = 240;

        Body? bodyA = null;
        Body? bodyB = null;
        
        var sketch = new Sketch(width, height)
        {
            Setup = s =>
            {
                s.Background(Color.RayWhite);

                bodyA = new Body(s)
                {
                    Position = new Vector2(320, 40),
                    Velocity = new Vector2(1, 0),
                    Mass = 8,
                };

                bodyB = new Body(s)
                {
                    Position = new Vector2(320, 200),
                    Velocity = new Vector2(-1, 0),
                    Mass = 8,
                };
            },
            Draw = (s, dt) =>
            {
                s.Background(Color.RayWhite);
                
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