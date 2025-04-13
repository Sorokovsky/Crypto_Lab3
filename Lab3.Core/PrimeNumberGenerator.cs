namespace Lab3.Core;

public class PrimeNumberGenerator
{
    private readonly Random _random = new();

    public long Generate()
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

    private static bool IsPrime(long number)
    {
        if ((number & 1) == 0) return number == 2;
        var limit = (long)Math.Sqrt(number);
        for (long i = 3; i <= limit; i += 2)
            if (number % i == 0)
                return false;
        return true;
    }
}