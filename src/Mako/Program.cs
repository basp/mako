namespace Mako;

using Raylib_CSharp.Colors;

internal static class Program
{
    public static void Main(string[] args)
    {
        const int width = 640;
        const int height = 240;
        
        var sketch = new SketchF(
            s =>
            {
                s.Background(Color.White);
            },
            s =>
            {
                s.Fill(Color.Black);
                s.Push();
                s.Translate(width /2f, height / 2f);
                s.SetRectMode(RectMode.CENTER);
                s.Rect(0, 0, 200, 100);
                s.Pop();
                s.Rect(0, 0, 200, 100);
            },
            width,
            height);
        
        sketch.Show();
    }
}

