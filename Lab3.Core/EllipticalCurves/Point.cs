using System.Numerics;
using System.Text.Json.Serialization;

namespace Lab3.Core.EllipticalCurves;

public class Point
{
    [JsonConstructor]
    public Point(BigInteger x, BigInteger y, BigInteger a, BigInteger b, BigInteger p, bool isInfinity = false)
    {
        if (!isInfinity && !IsOnCurve(x, y, a, b, p))
            throw new ArgumentException("Point does not lie on the elliptic curve");

        X = x;
        Y = y;
        A = a;
        B = b;
        P = p;
        IsInfinity = isInfinity;
    }

    // Конструктор копіювання
    public Point(Point point) : this(point.X, point.Y, point.A, point.B, point.P, point.IsInfinity)
    {
    }

    public Point()
    {
    }

    public BigInteger X { get; }
    public BigInteger Y { get; }
    public BigInteger A { get; }
    public BigInteger B { get; }
    public BigInteger P { get; }
    public bool IsInfinity { get; }

    public static Point operator +(Point first, Point second)
    {
        if (first.IsInfinity) return new Point(second);
        if (second.IsInfinity) return new Point(first);
        if (!SameCurve(first, second))
            throw new ArgumentException("Points are not on the same curve");

        // Якщо точки рівні за x, але протилежні за y
        if (first.X == second.X && first.Y != second.Y)
            return Infinity(first.A, first.B, first.P);

        var lambda = CalculateLambda(first, second);
        var x3 = Mod(lambda * lambda - first.X - second.X, first.P);
        var y3 = Mod(lambda * (first.X - x3) - first.Y, first.P);

        return new Point(x3, y3, first.A, first.B, first.P);
    }

    public static Point operator *(BigInteger scalar, Point point)
    {
        if (point.IsInfinity) return Infinity(point.A, point.B, point.P);

        var result = Infinity(point.A, point.B, point.P);
        var temp = new Point(point);
        scalar = Mod(scalar, point.P);

        while (scalar > 0)
        {
            if (scalar % 2 == 1)
                result += temp;

            temp += temp;
            scalar /= 2;
        }

        return result;
    }

    public static Point operator *(Point point, BigInteger scalar)
    {
        return scalar * point;
    }

    private static bool SameCurve(Point a, Point b)
    {
        return a.A == b.A && a.B == b.B && a.P == b.P;
    }

    private static BigInteger CalculateLambda(Point first, Point second)
    {
        if (first.X != second.X)
        {
            // Додавання різних точок
            var numerator = Mod(second.Y - first.Y, first.P);
            var denominator = Mod(second.X - first.X, first.P);
            return ModDiv(numerator, denominator, first.P);
        }
        else
        {
            // Подвоєння точки
            var numerator = Mod(3 * BigInteger.Pow(first.X, 2) + first.A, first.P);
            var denominator = Mod(2 * first.Y, first.P);
            return ModDiv(numerator, denominator, first.P);
        }
    }

    private static bool IsOnCurve(BigInteger x, BigInteger y, BigInteger a, BigInteger b, BigInteger p)
    {
        if (p == 0) return true; // Для нескінченної точки
        var left = Mod(y * y, p);
        var right = Mod(BigInteger.Pow(x, 3) + a * x + b, p);
        return left == right;
    }

    private static BigInteger Mod(BigInteger value, BigInteger modulus)
    {
        if (modulus == 0) return value;
        var result = value % modulus;
        return result < 0 ? result + modulus : result;
    }

    private static BigInteger ModDiv(BigInteger a, BigInteger b, BigInteger p)
    {
        var inverse = ModInverse(b, p);
        return Mod(a * inverse, p);
    }

    private static BigInteger ModInverse(BigInteger a, BigInteger p)
    {
        if (a == 0)
            throw new DivideByZeroException();

        BigInteger x, y;
        var g = ExtendedGcd(a, p, out x, out y);
        if (g != 1)
            throw new ArithmeticException("Inverse doesn't exist");

        return Mod(x, p);
    }

    private static BigInteger ExtendedGcd(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
    {
        if (a == 0)
        {
            x = 0;
            y = 1;
            return b;
        }

        BigInteger x1, y1;
        var gcd = ExtendedGcd(b % a, a, out x1, out y1);
        x = y1 - b / a * x1;
        y = x1;
        return gcd;
    }

    public static Point Infinity(BigInteger a, BigInteger b, BigInteger p)
    {
        return new Point(0, 0, a, b, p, true);
    }

    public override string ToString()
    {
        return IsInfinity ? "Infinity" : $"({X}, {Y})";
    }
}