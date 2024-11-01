namespace Mako.Examples;

using System.Numerics;
using Raylib_CSharp;
using Raylib_CSharp.Colors;
using Raylib_CSharp.Rendering;

internal static class Example09
{
    private static float G = 1.2f;
    
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
        
        public Vector2 Attract(Body body)
        {
            var force = this.Position - body.Position;
            var d = force.Length().Constrain(5, 25);
            var str = (G * (this.Mass * body.Mass)) / (d * d);
            return force.NormalizeMultiply(str);
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
                this.s.StrokeWeight(4);
                this.s.Stroke(Color.White);
                this.s.Fill(new Color(32, 200, 220, 200));
                this.s.Circle(
                    this.Position.X,
                    this.Position.Y,
                    this.Radius * 8f);
            });
        }
    }
    
    public static void Run()
    {
        const int width = 640;
        const int height = 640;

        var bodies = new List<Body>();
        
        var sketch = new Sketch(width, height)
        {
            GlobalBlendMode = BlendMode.Alpha,
            Setup = s =>
            {
                s.Background(Color.Black);

                for (var i = 0; i < 10; i++)
                {
                    var x = Raylib.GetRandomValue(0, width);
                    var y = Raylib.GetRandomValue(0, height);
                    var mass = Utils.GetRandomSingle(0.1f, 2);

                    bodies.Add(new Body(s)
                    {
                        Position = new Vector2(x, y),
                        Mass = mass,
                    });
                }
            },
            Draw = (s, dt) =>
            {
                s.DrawScoped(scope =>
                {
                    s.SetBlendMode(BlendMode.Multiplied);
                    s.Background(new Color(0, 0, 0, 20));
                });

                for (var i = 0; i < bodies.Count; i++)
                {
                    for (var j = 0; j < bodies.Count; j++)
                    {
                        if (i == j)
                        {
                            // Don't attract ourself.
                            continue;
                        }

                        var force = bodies[j].Attract(bodies[i]);
                        bodies[i].ApplyForce(force);
                    }
                    
                    bodies[i].Update(dt);
                    bodies[i].Draw();
                }
            },
        };
        
        sketch.Run();
    }
}