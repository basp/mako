namespace Mako;

internal class SketchF : Sketch
{
    private readonly Action<Sketch> setup;
    private readonly Action<Sketch> draw;
    
    public SketchF(
        Action<Sketch> setup,
        Action<Sketch> draw,
        int width,
        int height)
        : base(width, height)
    {
        this.setup = setup;
        this.draw = draw;
    }

    protected override void Setup() => this.setup(this);

    protected override void Draw() => this.draw(this);
}