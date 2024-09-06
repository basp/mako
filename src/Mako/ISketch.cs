namespace Mako;

using Raylib_CSharp.Colors;

public interface ISketch
{
    /// <summary>
    /// The width (in pixels) of the sketch.
    /// </summary>
    int Width { get; }
    
    /// <summary>
    /// The height (in pixels) of the sketch.
    /// </summary>
    int Height { get; }
    
    /// <summary>
    /// Begins a drawing group that contains its own styles and transformations.
    /// </summary>
    void Push();
    
    /// <summary>
    /// Ends a drawing group that contains its own styles and transformations.
    /// </summary>
    void Pop();
    
    /// <summary>
    /// Clears the pixels on the canvas.
    /// </summary>
    /// <remarks>
    /// <code>Clear()</code> makes every pixel 100% transparent.
    /// </remarks>
    void Clear();
    
    /// <summary>
    /// Sets the color used for the background of the canvas.
    /// </summary>
    /// <param name="color">The background color.</param>
    void Background(Color color);

    /// <summary>
    /// Translates the coordinate system.
    /// </summary>
    /// <param name="dx">The translation along the x-axis.</param>
    /// <param name="dy">The translation along the y-axis.</param>
    void Translate(float dx, float dy);

    /// <summary>
    /// Rotates the coordinate system.
    /// </summary>
    /// <param name="angle">The rotation angle.</param>
    void Rotate(float angle);
    
    /// <summary>
    /// Sets the color used to fill shapes.
    /// </summary>
    /// <param name="color">The fill color.</param>
    void Fill(Color color);

    /// <summary>
    /// Sets the color used to draw points, lines, and the outlines of shapes.
    /// </summary>
    /// <param name="color">The stroke color.</param>
    void Stroke(Color color);

    /// <summary>
    /// Sets the width of the stroke used for points, lines, and the outlines of shapes.
    /// </summary>
    /// <param name="value">The stroke width.</param>
    void StrokeWeight(float value);

    /// <summary>
    /// Changes where rectangles and squares are drawn.
    /// </summary>
    /// <param name="mode">The rectangle drawing mode.</param>
    void SetRectMode(RectMode mode);

    /// <summary>
    /// Disables setting the fill color for shapes.
    /// </summary>
    void NoFill();

    /// <summary>
    /// Disables drawing points, lines, and the outlines of shapes.
    /// </summary>
    void NoStroke();

    void Circle(float x, float y, float radius);

    void Line(float x1, float y1, float x2, float y2);

    void Rect(float x, float y, float w, float h);

    void Run();
}