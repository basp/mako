# Mako
Mako is a thin wrapper on top of **Raylib** taking inspiration of **p5** API in 
a style that is somewhat idiomatic to **.NET** users.
* Setup 2D simulations without much ceremony.
* Include utility libraries for stuff lacking (in the C# wrapper).
* Still regain access to the lower level **Raylib** API.
* Offer a composable and easy to understand framework (*).

## Examples
### Minimal Sketch
In **Mako**, the root building block is a `Sketch`. A minimal `Sketch` requires
a width and height. You can run it but it is just an empty canvas.
```csharp
var sketch = new Sketch(640, 240);
sketch.Run();
```


When we want to draw (or animate) something we can assign the `Draw` property.
This property is a procedure that takes two arguments.
```csharp
var sketch = new Sketch(640, 240)
{
    Draw = (s, _) =>
    {
        s.Background(Color.White);
        s.Fill(Color.Black);
        s.Circle(320, 120, 32);
    },
};
```

The `Draw` property has a signature of `Action<Sketch, float>`. It is a 
*procedure* since it does not return a result. The `Sketch` argument is the 
`Sketch` instance for which it is invoked. The `float` argument is the
*delta time* (in seconds) between this and the previous frame. This can be used 
to scale vectors according to dips or rises in frame rate during runtime.

> If your simulation is running at 60 frames/sec then the `dt` argument should
> be close to 1/60 (~0,017)s. It's ok to ignore the delta time for basic
> sketches when you are confident it is running at the intended frame rate.

In the example above we are using various methods of the `Sketch` instance to
do the drawing. This is often the most convenient way but it is important to
note that you are not limited to using the `Sketch` API. We still have access
to the underlying `Raylib_CSharp` wrapper.
```csharp
const int width = 640;
const int height = 240;

var sketch = new Sketch(width, height)
{
    Draw = (_, _) =>
    {
        Graphics.ClearBackground(Color.White);
        Graphics.DrawCircle(
            width / 2,
            height / 2,
            32,
            Color.Black);
    },
};

sketch.Run();
```

