namespace Mako.Examples;

using System.Numerics;
using Raylib_CSharp.Colors;

internal static class Example10
{
    private abstract class Body
    {
        protected Sketch s;
        
        protected Body(Sketch s)
        {
            this.s = s;
        }
        
        public Vector2 Position { get; set; }
        
        public Vector2 Velocity { get; set; }
        
        public Vector2 Acceleration { get; set; }

        public float Mass { get; set; } = 1;

        public float Radius { get; set; } = 8;

        public float MaxSpeed { get; set; } = 600;

        public void ApplyForce(Vector2 force)
        {
            this.Acceleration += Vector2.Divide(force, this.Mass);
        }
        
        public abstract void Draw();
        
        public virtual void Update(float dt)
        {
            this.Velocity += this.Acceleration;
            this.Velocity = this.Velocity.Limit(this.MaxSpeed);
            this.Position += this.Velocity * dt;
            this.Acceleration = Vector2.Zero;
        }

        public bool IsAlive()
        {
            if (this.Position.X <= -this.Radius)
            {
                return false;
            }

            if (this.Position.X >= this.s.Width + this.Radius)
            {
                return false;
            }

            if (this.Position.Y <= -this.Radius)
            {
                return false;
            }

            if (this.Position.Y >= this.s.Height + this.Radius)
            {
                return false;
            }

            return true;
        }
    }

    private class Nuclide(Sketch s) : Body(s)
    {
        public bool IsActive { get; set; } = true;

        private void DrawCircle(Sketch scope)
        {
            scope.Circle(
                this.Position.X,
                this.Position.Y,
                this.Radius);
        }

        private void DrawRect(Sketch scope)
        {
            scope.Rect(
                this.Position.X,
                this.Position.Y,
                this.Radius * 2,
                this.Radius * 2);
        }
        
        public override void Draw()
        {
            this.s.DrawScoped(scope =>
            {
                var fill = this.IsActive
                    ? Color.SkyBlue
                    : new Color(0, 0, 0, 0);
                scope.Fill(fill);
                scope.StrokeWeight(0);
                this.DrawRect(scope);
            });
        }

        public Neutron[] Eject()
        {
            var n1 = new Neutron(this.s)
            {
                Position = this.Position,
                Velocity = new Vector2(-10, -20)
                    .NormalizeMultiply(100),
                Radius = 4f,
            };

            var n2 = new Neutron(this.s)
            {
                Position = this.Position,
                Velocity = new Vector2(-10, 20)
                    .NormalizeMultiply(100),
                Radius = 4f,
            };

            return [n1, n2];
        }
    }
    
    private class Neutron(Sketch s) : Body(s)
    {
        public override void Draw()
        {
            this.s.DrawScoped(scope =>
            {
                scope.Fill(Color.Black);
                scope.StrokeWeight(0);
                scope.Circle(
                    this.Position.X,
                    this.Position.Y,
                    this.Radius);
            });
        }

        public bool Intersects(Nuclide body)
        {
            if (!body.IsActive)
            {
                return false;
            }
            
            var d = (body.Position - this.Position).Length();
            return d - this.Radius - body.Radius <= 0;
        }
    }
    
    public static void Run()
    {
        const int width = 640;
        const int height = 240;

        var neutrons = new List<Neutron>();
        var nuclides = new List<Nuclide>();
        
        var sketch = new Sketch(width, height)
        {
            Setup = s =>
            {
                s.Background(Color.RayWhite);

                for (var i = 0; i < 10; i++)
                {
                    var x = 96 + (i * 48);
                    nuclides.Add(new Nuclide(s)
                    {
                        Position = new Vector2(x, 120),
                        Radius = 8f,
                    });
                }

                neutrons.Add(new Neutron(s)
                {
                    Position = new Vector2(0, 120),
                    Velocity = new Vector2(200, 0),
                    Radius = 4f,
                });
            },
            Draw = (s, dt) =>
            {
                s.Background(Color.RayWhite);
                
                for (var i = neutrons.Count - 1; i >= 0; i--)
                {
                    var neutron = neutrons[i];

                    neutron.Update(dt);
                    
                    foreach (var nuclide in nuclides)
                    {
                        if (neutron.Intersects(nuclide))
                        {
                            nuclide.IsActive = false;
                            neutrons.AddRange(nuclide.Eject());
                        }
                        
                        nuclide.Draw();
                    }

                    if (!neutron.IsAlive())
                    {
                        neutrons.Remove(neutron);
                    }
                    
                    neutron.Draw();
                }
            },
        };
        
        sketch.Run();
    }
}