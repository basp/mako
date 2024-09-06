namespace Mako;

internal class Sketch : AbstractSketch
{
    public Sketch(int width, int height)
        : base(width, height)
    {
    }

    public Action<ISketch> Setup { get; init; } = _ => { };

    public Action<ISketch, float> Draw { get; init; } = (_, _) => { };

    protected override void InternalSetup() => this.Setup(this);

    protected override void InternalDraw(float dt) => this.Draw(this, dt);
}