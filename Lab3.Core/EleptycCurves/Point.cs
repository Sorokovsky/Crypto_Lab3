using System.Numerics;

namespace Lab3.Core.EleptycCurves;

public class Point
{
    public Point(Point point)
    {
        X = point.X;
        Y = point.Y;
        A = point.A;
        B = point.B;
        FieldChar = point.FieldChar;
    }

    public Point()
    {
        X = new BigInteger();
        Y = new BigInteger();
        A = new BigInteger();
        B = new BigInteger();
        FieldChar = new BigInteger();
    }

    public BigInteger X { get; set; }
    public BigInteger Y { get; set; }
    public BigInteger A { get; set; }
    public BigInteger B { get; set; }
    public BigInteger FieldChar { get; set; }

    public static BigInteger ModInverse(BigInteger a, BigInteger n)
    {
        BigInteger i = n, v = 0, d = 1;
        while (a > 0)
        {
            BigInteger temp = i / a, x = a;
            a = i % x;
            i = x;
            x = d;
            d = v - temp * x;
            v = x;
        }

        v %= n;
        if (v < 0) v = (v + n) % n;
        return v;
    }

    public static Point operator +(Point first, Point second)
    {
        var result = new Point
        {
            A = first.A,
            B = first.B,
            FieldChar = first.FieldChar
        };
        var dy = second.Y - first.Y;
        var dx = second.X - first.X;
        if (dx < 0) dx += first.FieldChar;
        if (dy < 0) dy += first.FieldChar;
        var m = dy * ModInverse(dx, first.FieldChar) % first.FieldChar;
        if (m < 0) m += first.FieldChar;
        result.X = (m * m - first.X - second.X) % first.FieldChar;
        result.Y = (m * (first.X - result.X) - first.Y) % first.FieldChar;
        if (result.X < 0) result.X += first.FieldChar;
        if (result.Y < 0) result.Y += first.FieldChar;
        return result;
    }

    public static Point Double(Point point)
    {
        var result = new Point
        {
            A = point.A,
            B = point.B,
            FieldChar = point.FieldChar
        };
        var dy = 3 * point.X * point.X + point.A;
        var dx = 2 * point.Y;
        if (dx < 0) dx += point.FieldChar;
        if (dy < 0) dy += point.FieldChar;
        var m = dy * ModInverse(dx, point.FieldChar) % point.FieldChar;
        result.X = (m * m - point.X - point.X) % point.FieldChar;
        result.Y = (m * (point.X - result.X) - point.Y) % point.FieldChar;
        if (result.X < 0) result.X += point.FieldChar;
        if (result.Y < 0) result.Y += point.FieldChar;
        return result;
    }

    public static Point Multiply(BigInteger first, Point second)
    {
        var result = second;
        first -= 1;
        while (first != 0)
        {
            if (first % 2 != 0)
            {
                if (result.X == second.X || result.Y == second.Y) result = Double(result);
                else result += second;
            }

            first /= 2;
            second = Double(second);
        }

        return result;
    }
}