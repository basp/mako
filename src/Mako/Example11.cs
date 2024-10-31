using Raylib_CSharp.Rendering;
using Raylib_CSharp.Transformations;

namespace Mako;

using System.Numerics;
using Raylib_CSharp.Colors;

internal static class Example11
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

        private void DrawAsCircle(Sketch scope)
        {
            scope.Circle(
                this.Position.X,
                this.Position.Y,
                this.Radius);
        }

        private void DrawAsRect(Sketch scope)
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
                this.DrawAsRect(scope);
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
        private void DrawAsCircle(Sketch scope)
        {
            scope.Circle(
                this.Position.X,
                this.Position.Y,
                this.Radius);
        }

        private void DrawAsRect(Sketch scope)
        {
            scope.Rect(
                this.Position.X,
                this.Position.Y,
                this.Radius,
                this.Radius);
        }
        
        public override void Draw()
        {
            this.s.DrawScoped(scope =>
            {
                scope.Fill(Color.Black);
                scope.StrokeWeight(0);
                // this.DrawAsCircle(scope);
                this.DrawAsRect(scope);
            });
        }

        public bool Intersects(Nuclide body)
        {
            if (!body.IsActive)
            {
                return false;
            }

            return Intersect.Circles(
                this.Position,
                this.Radius,
                body.Position,
                body.Radius);

            // Seems like Intersect.Recs is broken.
            //
            // return Intersect.Recs(
            //     new Rectangle(
            //         this.Position.X - this.Radius,
            //         this.Position.Y - this.Radius,
            //         this.Radius * 2,
            //         this.Radius * 2),
            //     new Rectangle(
            //         body.Position.X - body.Radius,
            //         body.Position.Y - body.Radius,
            //         body.Radius * 2,
            //         body.Radius * 2));
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

                // Setup some static nuclides.
                for (var i = 0; i < 20; i++)
                {
                    var x = 96 + (i * 24);
                    nuclides.Add(new Nuclide(s)
                    {
                        Position = new Vector2(x, 120),
                        Radius = 10f,
                    });
                }

                // Setup a neutron with an initial velocity.
                neutrons.Add(new Neutron(s)
                {
                    Position = new Vector2(0, 120),
                    Velocity = new Vector2(160, 0),
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

                var alive = neutrons.Count;
                var msg = $"{alive} neutrons alive";
                Graphics.DrawText(msg, 10, 30, 20, Color.Lime);
            },
        };
        
        sketch.Run();
    }
}