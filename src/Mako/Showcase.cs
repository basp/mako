namespace Mako;

using Raylib_CSharp.Colors;

public class Showcase
{
    public static ISketch Sketch0()
    {
        const int width = 640;
        const int height = 240;

        return new Sketch(width, height)
        {
            Setup = s =>
            {
                s.Background(Color.White);
            },
            Draw = (s, _) =>
            {
                s.Stroke(Color.Black);
                s.StrokeWeight(2f);
                s.Fill(Color.Lime);
                s.Rect(30, 30, 50, 50);
            },
        };
    }
}