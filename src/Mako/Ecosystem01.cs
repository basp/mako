using System.Numerics;
using Raylib_CSharp;
using Raylib_CSharp.Colors;

namespace Mako;

internal static class Ecosystem01
{
    public static void Run()
    {
        const int width = 640;
        const int height = 240;

        var creatures = new List<Creature>();
        
        var sketch = new Sketch(width, height)
        {
            Setup = s =>
            {
                var boobles = Enumerable
                    .Range(0, 10)
                    .Select(_ => new Booble(s)
                    {
                        Position = RandomPosition(),
                        MaxSpeed = 160,
                    })
                    .ToArray();

                creatures.AddRange(boobles);

                foreach (var booble in boobles)
                {
                    for (var i = 0; i < 30; i++)
                    {
                        creatures.Add(new Beeble(s, booble)
                        {
                            Position = RandomPosition(),
                            MaxSpeed = Raylib.GetRandomValue(10, 120),
                        });
                    }
                }
            },
            Draw = (s, dt) =>
            {
                s.Background(Color.White);
                
                foreach (var creature in creatures)
                {
                    creature.Draw();
                    creature.Update(dt);
                }
            },
        };
        
        sketch.Run();

        return;

        static Vector2 RandomPosition() => new(RandomX(), RandomY());
            
        static int RandomX() => Raylib.GetRandomValue(0, width);
        static int RandomY() => Raylib.GetRandomValue(0, height);
    }
    
    private abstract class Creature
    {
        protected readonly Sketch s;
        
        protected Creature(Sketch s)
        {
            this.s = s;
        }
        
        public Vector2 Position { get; set; }
        
        public Vector2 Velocity { get; set; }
        
        public Vector2 Acceleration { get; set; }

        public float MaxSpeed { get; set; } = 100;
        
        public abstract Color Fill { get; }
        
        public abstract float Radius { get; }
        
        public virtual void Draw()
        {
            this.s.Push();
            this.s.Fill(this.Fill);
            this.s.Circle(
                this.Position.X,
                this.Position.Y,
                this.Radius);
            this.s.Pop();
        }

        public abstract void Update(float dt);
        
        protected void Wrap()
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
    }
    
    private class Booble : Creature
    {
        private readonly Random random = new();
        
        public Booble(Sketch s) 
            : base(s)
        {
        }

        public override Color Fill => Color.Orange;

        public override float Radius => 16f;

        public override void Update(float dt)
        {
            var dir = Vector.Random2D();
            var spd = this.random.NextSingle() * 30f;
            
            this.Acceleration = dir * spd;
            this.Velocity = (this.Velocity + this.Acceleration)
                .Limit(this.MaxSpeed);
            this.Position += this.Velocity * dt;
            
            this.Wrap();
        }
    }

    private class Beeble : Creature
    {
        private readonly Booble booble;
        
        public Beeble(Sketch s, Booble booble) 
            : base(s)
        {
            this.booble = booble;
        }

        public override Color Fill => Color.SkyBlue;
        
        public override float Radius => 8f;

        public override void Update(float dt)
        {
            var dir = this.booble.Position - this.Position;

            this.Acceleration = dir.NormalizeMultiply(10f);
            this.Velocity = (this.Velocity + this.Acceleration)
                .Limit(this.MaxSpeed);
            this.Position += this.Velocity * dt;
            
            this.Wrap();
        }
    }    
}