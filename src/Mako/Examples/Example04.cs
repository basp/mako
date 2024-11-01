namespace Mako.Examples;

using System.Numerics;
using Raylib_CSharp.Interact;
using Raylib_CSharp.Colors;

internal static class Example04
{
    public static void Run()
    {
        const int width = 640;
        const int height = 240;

        var movers = new List<Mover>();
        
        var sketch = new Sketch(width, height)
        {
            Setup = s =>
            {
                movers.Add(new Mover(s)
                {
                    Radius = 32,
                    Position = new Vector2(320, 120),
                });

                s.StrokeWeight(8);
                s.Stroke(Color.Black);
                s.Fill(Color.White);
            },
            Draw = (s, dt) =>
            {
                s.Background(Color.RayWhite);
                
                foreach (var mover in movers)
                {
                    mover.Draw();
                    mover.Update(dt);
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
        
        public float Radius { get; set; }

        public void Draw()
        {
            this.s.Circle(
                this.Position.X, 
                this.Position.Y,
                this.Radius);            
        }

        public void Update(float dt)
        {
            var mouse = Input.GetMousePosition();
            
            this.Acceleration = (mouse - this.Position).NormalizeMultiply(20f);
            this.Velocity = (this.Velocity + this.Acceleration).Limit(300);
            this.Position += this.Velocity * dt;

            this.Wrap();
        }

        private void Wrap()
        {
            var xMin = -this.Radius;
            var xMax = this.s.Width + this.Radius;

            var yMin = -this.Radius;
            var yMax = this.s.Height + this.Radius;

            if (this.Position.X < xMin)
            {
                this.Position = this.Position with { X = xMax };
            }
            else if (this.Position.X > xMax)
            {
                this.Position = this.Position with { X = xMin };
            }

            if (this.Position.Y < yMin)
            {
                this.Position = this.Position with { Y = yMax };
            }
            else if (this.Position.Y > yMax)
            {
                this.Position = this.Position with { Y = yMin };
            }
        }
        
        private void Bounce()
        {
            var xMin = this.Radius;
            var xMax = this.s.Width - this.Radius;

            var yMin = this.Radius;
            var yMax = this.s.Height - this.Radius;

            if (this.Position.X < xMin)
            {
                this.Position = this.Position with { X = xMin };
                this.Velocity = this.Velocity with { X = -this.Velocity.X };
            }
            else if (this.Position.X > xMax)
            {
                this.Position = this.Position with { X = xMax };
                this.Velocity = this.Velocity with { X = -this.Velocity.X };
            }

            if (this.Position.Y < yMin)
            {
                this.Position = this.Position with { Y = yMin };
                this.Velocity = this.Velocity with { Y = -this.Velocity.Y };
            }
            else if (this.Position.Y > yMax)
            {
                this.Position = this.Position with { Y = yMax };
                this.Velocity = this.Velocity with { Y = -this.Velocity.Y };
            }
        }
    }
}