namespace Mako;

using System.Numerics;
using Raylib_CSharp;
using Raylib_CSharp.Colors;

internal static class Example01
{
    public static void Run()
    {
        var movers = new List<Mover>();
        var sketch = new Sketch(640, 240)
        {
            Setup = s =>
            {
                for (var i = 0; i < 32; i++)
                {
                    var x = Raylib.GetRandomValue(32, 640 - 32);
                    var y = Raylib.GetRandomValue(32, 240 - 32);

                    var dx = Raylib.GetRandomValue(-100, 100);
                    var dy = Raylib.GetRandomValue(-100, 100);

                    var r = Raylib.GetRandomValue(8, 32);
                    
                    var pos = new Vector2(x, y);
                    var dir = new Vector2(dx, dy);
                    var spd = Raylib.GetRandomValue(200, 600);
                    var vel = Vector2.Normalize(dir) * spd;
                    
                    movers.Add(new Mover(s)
                    {
                        Position = pos,
                        Velocity = vel,
                        Radius = r,
                    });
                }
                
                s.Background(Color.Black);
                s.Fill(new Color(32, 180, 180, 64));
                s.Stroke(Color.SkyBlue);
                s.StrokeWeight(4f);
            },
            Draw = (s, dt) =>
            {
                s.Background(new Color(0, 0, 0, 255));
                foreach (var walker in movers)
                {
                    walker.Draw();
                    walker.Update(dt);
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
            this.Position += this.Velocity * dt;

            var xMax = this.s.Width - this.Radius;
            var yMax = this.s.Height - this.Radius;
            
            if (this.Position.X < this.Radius)
            {
                this.Position = this.Position with { X = this.Radius };
                this.Velocity = this.Velocity with { X = -this.Velocity.X };
            }
            else if (this.Position.X > xMax)
            {
                this.Position = this.Position with { X = xMax };
                this.Velocity = this.Velocity with { X = -this.Velocity.X };
            }

            if (this.Position.Y < this.Radius)
            {
                this.Position = this.Position with { Y = this.Radius };
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