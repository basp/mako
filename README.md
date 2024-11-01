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

The `Draw` property has a signature of `Action<Sketch, float>`.  The `Sketch` 
argument is the `Sketch` instance for which it is invoked. This object offers 
various drawing and utility methods to help with composing sketches. The 
`float` argument is the *delta time* (in seconds) between this and the previous 
frame. This can be used to scale vectors according to dips or rises in frame 
rate during runtime.

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

### Scopes
A useful feature of the `Sketch` API is that we have somewhat formalized scoped
sketches. Whenever we enter a scope we *push* the drawing context onto a stack 
and when we are done we *pop* that context. This is convenient if we want to 
transform the camera or any other context parameters for a particular 
object that we want to draw. The `DrawScoped` method is a helper so that we
do not have to worry about maintaining the drawing context stack.
```csharp
const int width = 640;
const int height = 240;

var sketch = new Sketch(width, height)
{
    Draw = (s, _) =>
    {
        const int width = 640;
        const int height = 240;

        var sketch = new Sketch(width, height)
        {
            Draw = (s, _) =>
            {
                s.Background(Color.RayWhite);
                s.Fill(Color.Orange);
                  
                s.DrawScoped(scope =>
                {
                    scope.Translate(width / 2f, height / 2f);
                    scope.Fill(Color.SkyBlue);
                    scope.Circle(0, 0, 64f);
                });
                  
                s.Circle(width, height, 64f);
                  
                s.DrawScoped(scope =>
                {
                    scope.Translate(width / 4f, height / 4f);
                    scope.Fill(Color.Lime);
                    scope.Circle(0, 0, 32f);
                });
                  
                s.Circle(0, 0, 64f);
            },
        };
        
        sketch.Run();        
    },
};
```

Of course it is possible to `Push` and `Pop` manually instead of using the 
`DrawScoped` method. But we need to be careful about our pushes and pops to
make sure the stack does not get unbalanced.
```csharp
...
    
Draw = (s, _) =>
{
    s.Background(Color.RayWhite);
    s.Fill(Color.Orange);
    
    s.Push();
    s.Translate(width / 2f, height / 2f);
    s.Fill(Color.SkyBlue);
    s.Circle(0, 0, 64f);
    s.Pop();
      
    s.Circle(width, height, 64f);

    s.Push();
    s.Translate(width / 4f, height / 4f);
    s.Fill(Color.Lime);
    s.Circle(0, 0, 32f);
    s.Pop();
      
    s.Circle(0, 0, 64f);
},   
...
```

Sometimes (or depending on your taste) this can be clearer than the scoping
mechanism. However, it is recommended to use scoping since it will always make
sure the drawing context stack is maintained correctly.