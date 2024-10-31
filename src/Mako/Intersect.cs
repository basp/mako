using System.Drawing;

namespace Mako;

using System.Numerics;
using Raylib_CSharp.Transformations;

public static class Intersect
{
    public static bool CircleRec(
        Vector2 center,
        float radius,
        Rectangle rec)
    {
        throw new NotImplementedException();
    }
    
    public static bool Circles(
        Vector2 center1,
        float radius1,
        Vector2 center2,
        float radius2)
    {
        var d = (center2 - center1).Length();
        return d - radius1 - radius2 <= 0;
    }

    public static bool Recs(Rectangle rec1, Rectangle rec2)
    {
        var l1 = new PointF(rec1.X, rec1.X + rec1.Width);
        var r1 = new PointF(rec1.Y, rec1.Y + rec1.Height);
        
        var l2 = new PointF(rec2.X, rec2.X + rec2.Width);
        var r2 = new PointF(rec2.Y, rec2.Y + rec2.Height);

        if (l1.X > r2.X || l2.X > r1.X)
        {
            return false;
        }

        if (r1.Y > l2.Y || r2.Y > l1.Y)
        {
            return false;
        }

        return true;
    }
}