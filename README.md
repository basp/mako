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

> Mako is **not** an engine. Its main purpose is for creativity, testing, 
> benchmarking and algorithm design. And to show off the **Raylib** and
> **Raylib-CSharp** APIs.

## Examples
### Minimal
In **Mako**, the root building block is a `Sketch`. A minimal `Sketch` requires
a width and height. You can run it but it is just an empty (black) canvas.
```csharp
const int width = 640;
const int height = 240;

var sketch = new Sketch(width, height);
sketch.Run();
```

### Static Shape
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

### Low Level API
In the example above we are using various methods of the `Sketch` instance to
do the drawing (such as `Background`, `Fill` and `Circle`). This is often the 
most convenient way to setup a quick sketch but we still have full access to 
the underlying **Raylib** API (via [Raylib-CSharp](https://github.com/MrScautHD/Raylib-CSharp).

> If you use the low level API that goes via **Raylib-CSharp** then you will be
> operating outside of the drawing context. Any `Fill`, `Stroke` and other
> context values that you have set will be ignored.

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

### Sketch API vs. Raylib API
The **Sketch** API has very limited features compared to the **Raylib** API but
some things are much easier. We can use nested (scoped) sketches and easily
push and pop drawing context when we want. Using the underlying framework also
saves us from some busywork (such as setting up the window and beginning and
making sure we end all kinds of modes).

There's a lot of things we cannot do with the **Sketch** API that we would like 
to do. For example, there's no API to draw ellipses because dependencies do nut 
support drawing ellipses with stroke weight and our Sketch API suggests it 
should. Drawing arcs and curves, segments, polygons, all that stuff is
impossible with the current **Sketch** API. We want a coherent API and if we 
cannot emulate it somewhat properly we are not going to support it. In this
regard we rely on the capabilities of **Raylib** and **Raylib-CSharp** as well.
