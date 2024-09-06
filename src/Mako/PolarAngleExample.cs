using Raylib_CSharp.Colors;
using Raylib_CSharp.Rendering;

namespace Mako;

public class PolarAngleExample(int width, int height) 
    : SketchOld(width, height)
{
    private float r = 0;
    private float theta;
    
    public PolarAngleExample()
        : this(620, 240)
    {
    }

    protected override void Draw()
    {
        var pos = Utils.Vector2FromAngle(this.theta, this.r);
        Graphics.DrawCircle(
            (int)pos.X,
            (int)pos.Y, 
            8f, 
            Color.Black);
        this.theta += 0.01f;
        this.r += 0.05f;
    }
}