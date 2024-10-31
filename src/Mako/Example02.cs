namespace Mako;

using System.Numerics;
using Raylib_CSharp;
using Raylib_CSharp.Colors;

internal static class Example02
{
    public static void Run()
    {
        var movers = new List<Mover>();
        var sketch = new Sketch(640, 240)
        {
            Setup = s =>
            {
                for (var i = 0; i < 1; i++)
                {
                    var x = Raylib.GetRandomValue(32, 640 - 32);
                    var y = Raylib.GetRandomValue(32, 240 - 32);

                    var ddx = Raylib.GetRandomValue(-1, 1);
                    var ddy = Raylib.GetRandomValue(-1, 1);

                    var r = Raylib.GetRandomValue(8, 32);
                    
                    var pos = new Vector2(x, y);
                    var vel = new Vector2(0, 0);
                    var acc = new Vector2(ddx, ddy);

                    var mover = new Mover(s)
                    {
                        Position = pos,
                        Velocity = vel,
                        Acceleration = acc,
                        Radius = r,
                    };

                    movers.Add(mover);
                }
                
                s.Background(Color.SkyBlue);
                s.Fill(Color.RayWhite);
                s.Stroke(Color.Black);
                s.StrokeWeight(4f);
            },
            Draw = (s, dt) =>
            {
                s.Background(Color.SkyBlue);
                
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
            this.Velocity = (this.Velocity + this.Acceleration).Limit(600);
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