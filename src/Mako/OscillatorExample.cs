using Raylib_CSharp.Colors;
using Raylib_CSharp.Rendering;

namespace Mako;

public class OscillatorExample(int width, int height) : SketchOld(width, height)
{
    private Oscillator[] oscillators = [];

    protected override void Setup()
    {
        this.oscillators =
            Enumerable.Range(0, 10)
                .Select(_ => new Oscillator(this.width, this.height))
                .ToArray();
    }

    protected override void Draw()
    {
        // Graphics.ClearBackground(Color.White);
        foreach (var osc in this.oscillators)
        {
            osc.Update();
            osc.Show();
        }
    }
}