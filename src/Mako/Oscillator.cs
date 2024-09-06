using System.Numerics;
using Raylib_CSharp.Camera.Cam2D;
using Raylib_CSharp.Colors;
using Raylib_CSharp.Rendering;

namespace Mako;

public class Oscillator
{
    private readonly int width;
    private readonly int height;
    
    private readonly Vector2 angleVelocity;
    private readonly Vector2 amplitude;
    
    private Vector2 angle;

    public Oscillator(int width, int height)
    {
        this.width = width;
        this.height = height;
        this.angle = Vector2.Zero;
        this.angleVelocity =
            new Vector2(
                Utils.GetRandomSingle(-0.05f, 0.05f),
                Utils.GetRandomSingle(-0.05f, 0.05f));
        this.amplitude =
            new Vector2(
                Utils.GetRandomSingle(20, this.width / 2f),
                Utils.GetRandomSingle(20, this.height / 2f));
    }

    public void Update()
    {
        this.angle += this.angleVelocity;
    }

    public void Show()
    {
        var x = MathF.Sin(this.angle.X) * this.amplitude.X;
        var y = MathF.Sin(this.angle.Y) * this.amplitude.Y;
        var offset = new Vector2(this.width / 2f, this.height / 2f);
        var camera = new Camera2D(offset, Vector2.Zero, 0f, 1f);
        Graphics.BeginMode2D(camera);
        Graphics.DrawLineEx(
            Vector2.Zero,
            new Vector2(x, y),
            2f,
            Color.Black);
        Graphics.DrawCircle(
            (int)x,
            (int)y,
            16f,
            Color.Black);
        Graphics.DrawCircle(
            (int)x,
            (int)y,
            14f,
            new Color(127, 127, 127, 255));
        Graphics.EndMode2D();
    }
}