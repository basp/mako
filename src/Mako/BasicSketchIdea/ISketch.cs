namespace BasicSketchIdea.Mako;

using Raylib_CSharp.Colors;

public interface ISketch
{
    /// <summary>
    /// Tracks the number of frames drawn since the sketch started.
    /// </summary>
    int FrameCount { get; }

    /// <summary>
    /// The height (in pixels) of the sketch.
    /// </summary>
    int Height { get; }

    /// <summary>
    /// The width (in pixels) of the sketch.
    /// </summary>
    int Width { get; }

    /// <summary>
    /// Clears the pixels on the canvas.
    /// </summary>
    /// <remarks>
    /// <code>Clear()</code> makes every pixel 100% transparent.
    /// </remarks>
    void Clear();

    /// <summary>
    /// Converts an angle measured in radians to its value in degrees.
    /// </summary>
    float Degrees(float radians);

    /// <summary>
    /// Re-maps a number from one range to another.
    /// </summary>
    /// <param name="value">The value to be remapped.</param>
    /// <param name="start1">The lower bound of the value's current range.</param>
    /// <param name="stop1">The upper bound of the value's current range.</param>
    /// <param name="start2">The lower bound of the value's target range.</param>
    /// <param name="stop2">The upper bound of the value's target range.</param>
    /// <returns></returns>
    float Map(
        float value,
        float start1,
        float stop1,
        float start2,
        float stop2);

    /// <summary>
    /// Converts an angle measured in degrees to its value in radians.
    /// </summary>
    float Radians(float degrees);

    /// <summary>
    /// Changes the unit system used to measure angles.
    /// </summary>
    /// <param name="mode">The angle mode.</param>
    void SetAngleMode(AngleMode mode);

    /// <summary>
    /// Begins a drawing group that contains its own styles and transformations.
    /// </summary>
    void Push();

    /// <summary>
    /// Ends a drawing group that contains its own styles and transformations.
    /// </summary>
    void Pop();

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

    void Zoom(float value);

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

    /// <summary>
    /// Draws a circle.
    /// </summary>
    /// <param name="x">The x-coordinate of the center of the circle.</param>
    /// <param name="y">The y-coordinate of the center of the circle.</param>
    /// <param name="radius">The radius of the circle.</param>
    void Circle(float x, float y, float radius);

    /// <summary>
    /// Draws a straight line between two points.
    /// </summary>
    /// <param name="x1">The x-coordinate of the first point.</param>
    /// <param name="y1">The y-coordinate of the first point.</param>
    /// <param name="x2">The x-coordinate of the second point.</param>
    /// <param name="y2">The y-coordinate of the second point.</param>
    void Line(float x1, float y1, float x2, float y2);

    /// <summary>
    /// Draws an arc.
    /// </summary>
    /// <param name="x">The x-coordinate of the arc's ellipse.</param>
    /// <param name="y">The y-coordinate of the arc's ellipse.</param>
    /// <param name="w">The width of the arc's ellipse by default.</param>
    /// <param name="h">The height of the arc's ellipse by default.</param>
    /// <param name="start">Angle to start the arc.</param>
    /// <param name="stop">Angle to stop the arc.</param>
    /// <param name="mode">
    /// Optional parameter to determine the way of drawing the arc.
    /// </param>
    void Ellipse(
        float x,
        float y,
        float w,
        float h);
    
    /// <summary>
    /// Draws a rectangle.
    /// </summary>
    /// <param name="x">The x-coordinate of the rectangle.</param>
    /// <param name="y">The y-coordinate of the rectangle.</param>
    /// <param name="w">The width of the rectangle.</param>
    /// <param name="h">The height of the rectangle.</param>
    void Rect(float x, float y, float w, float h);

    void Run();

    void SetEllipseMode(EllipseMode mode);
}