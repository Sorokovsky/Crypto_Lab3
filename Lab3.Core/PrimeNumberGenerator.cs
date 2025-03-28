namespace Lab3.Core;

public class PrimeNumberGenerator
{
    private readonly Random _random = new();

    public ulong Generate()
    {
        var numberBytes = new byte[4];
        _random.NextBytes(numberBytes);
        var number = BitConverter.ToUInt32(numberBytes, 0);
        while (!IsPrime(number))
            unchecked
            {
                number++;
            }

        return number;
    }

    private static bool IsPrime(ulong number)
    {
        if ((number & 1) == 0) return number == 2;
        var limit = (ulong)Math.Sqrt(number);
        for (ulong i = 3; i <= limit; i += 2)
            if (number % i == 0)
                return false;
        return true;
    }
}