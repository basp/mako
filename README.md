# Mako
> **NOTE**: 
> This is a toy to play around with. 
> Don't use anywhere near production!

Mako is a thin wrapper on top of [Raylib](https://www.raylib.com/) via
[Raylib-CSharp](https://github.com/MrScautHD/Raylib-CSharp). The main use case
for this is to setup (simulation based) animations using **.NET** as runtime
platform.

* Setup 2D simulations without much ceremony
* Offer a composable and easy to understand framework (debatable)
* Still regain access to the lower level **Raylib** API (via **Raylib-CSharp**)
* Inspired by **p5.js** API
* Include utility libraries for stuff lacking (in the wrapper)
* Easy things should be easy, hard things should be possible

## Examples
### Minimal Sketch
In **Mako**, the root building block is a `Sketch`. A minimal `Sketch` requires
a width and height. You can run it but it is just an empty (black) canvas.
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
*procedure* since it does not return a result (an `Action` in .NET). The 
`Sketch` argument is the `Sketch` instance for which it is invoked. This object
offers various drawing and utility methods to help with composing sketches. The 
`float` argument is the *delta time* (in seconds) between this and the previous 
frame. This can be used to scale vectors according to dips or rises in frame rate 
during runtime.

> If your simulation is running at 60 frames/sec then the `dt` argument should
> be close to 1/60 (~0,017)s. It's ok to ignore the delta time for basic
> sketches when you are confident it is running at the intended frame rate.

In the example above we are using various methods of the `Sketch` instance to
do the drawing. This is often the most convenient way to setup a quick sketch
but we still have full access to the underlying **Raylib** API 
(via [Raylib-CSharp](https://github.com/MrScautHD/Raylib-CSharp))
```csharp
using Raylib_CSharp.Colors;
using Raylib_CSharp.Rendering;

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

