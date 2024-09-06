namespace Mako.OldExamples;

using Raylib_CSharp.Colors;
using Raylib_CSharp.Rendering;

public class OscillatingCircle(int width, int height) : SketchOld(width, height)
{
    private const float TwoPi = MathF.PI * 2f;

    private readonly float period = 120f;
    private readonly float amplitude = 100;
    
    private int frameCount;
    
    public OscillatingCircle()
        : this(640, 240)
    {
    }
    
    protected override void Draw()
    {
        this.frameCount += 1;
        
        var angle = this.frameCount * OscillatingCircle.TwoPi / this.period;
        var x = this.amplitude * MathF.Sin(angle);
        
        Graphics.DrawCircle(
            (int)x,
            0,
            16,
            Color.Black);
    }
}