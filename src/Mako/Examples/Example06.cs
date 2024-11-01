namespace Mako.Examples;

using System.Numerics;
using Raylib_CSharp;
using Raylib_CSharp.Colors;

internal static class Example06
{
    private const float G = 1f;
    
    public static void Run()
    {
        const int width = 640;
        const int height = 240;

        Attractor? attractor = null;

        var movers = new List<Mover>();
        
        var sketch = new Sketch(width, height)
        {
            Setup = s =>
            {
                s.Background(Color.RayWhite);

                attractor = new Attractor(s)
                {
                    Mass = 20,
                    Radius = 40,
                    Position = new Vector2(320, 120),
                };

                for (var i = 0; i < 10; i++)
                {
                    var x = Raylib.GetRandomValue(0, width);
                    var y = Raylib.GetRandomValue(0, height);
                    var mass = Utils.GetRandomSingle(0.5f, 3);
                    
                    movers.Add(new Mover(s)
                    {
                        Position = new Vector2(x, y),
                        Velocity = new Vector2(1, 0),
                        Mass = mass,
                        Radius = mass * 8,
                    });
                }
            },
            Draw = (s, dt) =>
            {
                s.Background(Color.RayWhite);

                attractor!.Draw();
                
                foreach (var mover in movers)
                {
                    var force = attractor!.Attract(mover);
                    mover.ApplyForce(force);
                    mover.Update(dt);
                    mover.Draw();
                }
            },
        };
        
        sketch.Run();
    }

    private class Mover
    {
        private readonly Sketch s;
        
        public Mover(Sketch s)
        {
            this.s = s;
        }
        
        public Vector2 Position { get; set; }
        
        public Vector2 Velocity { get; set; }
        
        public Vector2 Acceleration { get; set; }
        
        public float Mass { get; init; }
        
        public float Radius { get; init; }

        public void ApplyForce(Vector2 force)
        {
            this.Acceleration += Vector2.Divide(force, this.Mass);
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
            this.s.Fill(new Color(127, 127, 127, 180));
            this.s.Circle(
                this.Position.X,
                this.Position.Y,
                this.Radius);
            this.s.Pop();
        }
    }

    private class Attractor
    {
        private readonly Sketch s;
        
        public Attractor(Sketch s)
        {
            this.s = s;
        }

        public Vector2 Position { get; init; }
        
        public float Mass { get; init; } = 20f;

        public float Radius { get; init; } = 20f;
        
        public Vector2 Attract(Mover mover)
        {
            var force = this.Position - mover.Position;
            var distance = force.Length().Constrain(5, 25);
            var strength = 
                (Example06.G * this.Mass * mover.Mass) / (distance * distance);
            force = force.NormalizeMultiply(strength);
            return force;
        }
        
        public void Draw()
        {
            this.s.Push();
            this.s.StrokeWeight(2);
            this.s.Stroke(Color.Black);
            this.s.Fill(new Color(175, 175, 175, 255));
            this.s.Circle(
                this.Position.X,
                this.Position.Y,
                this.Radius);
            this.s.Pop();
        }
    }
}