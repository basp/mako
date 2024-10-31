using Raylib_CSharp;
using Raylib_CSharp.Interact;
using Raylib_CSharp.Rendering;

namespace Mako;

using System.Numerics;
using Raylib_CSharp.Colors;

internal static class Example05
{
    public static void Run()
    {
        const int width = 640;
        const int height = 240;

        var gravity = new Vector2(0, 8);
        var wind = new Vector2(50, 0);
        
        var movers = new List<Mover>();

        Liquid? liquid = null;
        
        var sketch = new Sketch(width, height)
        {
            Setup = s =>
            {
                s.Background(Color.RayWhite);

                for (var i = 0; i < 9; i++)
                {
                    var x = 40 + i * 70;
                    var y = 0;
                    var mass = Raylib.GetRandomValue(1, 5) * 4;
                    
                    movers.Add(new Mover(s)
                    {
                        Position = new Vector2(x, y),
                        Radius = mass * 2,
                        Mass = mass,
                    });
                }

                liquid = new Liquid(
                    s, 0, 
                    height / 2f, 
                    width, 
                    height / 2f, 
                    0.015f);
            },
            Draw = (s, dt) =>
            {
                s.Background(Color.RayWhite);

                foreach (var mover in movers)
                {
                    if (Input.IsMouseButtonDown(MouseButton.Left))
                    {
                        mover.ApplyForce(wind);
                    }
                    
                    if (mover.IsAtBottomEdge())
                    {
                        const float c = 0.1f;
                        var friction = (mover.Velocity * -1)
                            .NormalizeMultiply(c);
                        mover.ApplyForce(friction);
                    }

                    if (liquid!.Contains(mover))
                    {
                        var dragForce = liquid!.CalculateDrag(mover);
                        mover.ApplyForce(dragForce);
                    }

                    mover.ApplyForce(gravity * mover.Mass);
                    mover.Update(dt);
                    mover.Bounce();
                    mover.Draw();
                }

                liquid!.Draw();
            },
        };
        
        sketch.Run();
    }

    private class Liquid
    {
        private readonly Sketch s;
        
        private readonly float x;
        private readonly float y;
        private readonly float w;
        private readonly float h;

        private readonly float c;
        
        public Liquid(
            Sketch s, 
            float x, 
            float y, 
            float w, 
            float h, 
            float c)
        {
            this.s = s;
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
            this.c = c;
        }

        public void Draw()
        {
            this.s.Push();
            this.s.StrokeWeight(0);
            this.s.Fill(new Color(31, 111, 164, 60));
            this.s.Rect(this.x, this.y, this.w, this.h);
            this.s.Pop();
        }

        public bool Contains(Mover mover)
        {
            var pos = mover.Position;
            return (
                pos.X > this.x &&
                pos.X < this.x + this.w &&
                pos.Y > this.y &&
                pos.Y < this.y + this.h);
        }

        public Vector2 CalculateDrag(Mover mover)
        {
            var speed = mover.Velocity.Length();
            var dragMag = this.c * speed * speed;
            var dragForce = (mover.Velocity * -1f)
                .NormalizeMultiply(dragMag);
            return dragForce;
        }
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

        public float Radius { get; init; } = 16f;
        
        public float Mass { get; init; } = 10f;

        public void ApplyForce(Vector2 force)
        {
            this.Acceleration += Vector2.Divide(force, this.Mass);
        }

        public void Update(float dt)
        {
            this.Velocity += this.Acceleration;
            this.Position += this.Velocity * dt;
            this.Acceleration = Vector2.Zero;
        }

        public void Draw()
        {
            this.s.Push();
            this.s.Fill(Color.LightGray);
            this.s.Stroke(Color.Black);
            this.s.StrokeWeight(2);
            this.s.Circle(
                this.Position.X,
                this.Position.Y,
                this.Radius);
            this.s.Pop();
        }

        public bool IsAtBottomEdge() =>
            this.Position.Y > this.s.Height - this.Radius - 1;
        
        public void Bounce()
        {
            var xMin = this.Radius;
            var xMax = this.s.Width - this.Radius;

            var yMin = this.Radius;
            var yMax = this.s.Height - this.Radius;

            const float bounce = -0.9f;
            
            if (this.Position.X < xMin)
            {
                this.Position = this.Position with { X = xMin };
                this.Velocity = this.Velocity with
                {
                    X = this.Velocity.X * bounce,
                };
            }
            else if (this.Position.X > xMax)
            {
                this.Position = this.Position with { X = xMax };
                this.Velocity = this.Velocity with
                {
                    X = this.Velocity.X * bounce,
                };
            }

            if (this.Position.Y > yMax)
            {
                this.Position = this.Position with { Y = yMax };
                this.Velocity = this.Velocity with
                {
                    Y = this.Velocity.Y * bounce,
                };
            }
        }
    }
}

internal static class Program
{
    public static void Main(string[] _)
    {
        Example05.Run();
    }
}
