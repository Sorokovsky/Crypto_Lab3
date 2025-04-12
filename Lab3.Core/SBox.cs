namespace Lab3.Core;

public class SBox
{
    private readonly int[,] _box = new int[4, 16]
    {
        {
            0b0010, 0b1100, 0b0100, 0b0001, 0b0111, 0b1010, 0b1011, 0b0110, 0b1000, 0b0101, 0b0011, 0b1111, 0b0000,
            0b1110, 0b1001, 0b1101
        },
        {
            0b1110, 0b1011, 0b0010, 0b1100, 0b0100, 0b0110, 0b1101, 0b0001, 0b0111, 0b1001, 0b1111, 0b0011, 0b1010,
            0b0000, 0b1000, 0b0101
        },
        {
            0b0000, 0b1111, 0b0111, 0b0100, 0b1010, 0b0010, 0b1101, 0b0001, 0b0101, 0b1011, 0b1001, 0b1100, 0b1110,
            0b0011, 0b0110, 0b1000
        },
        {
            0b1011, 0b1000, 0b1100, 0b0111, 0b0001, 0b1110, 0b0010, 0b1101, 0b0110, 0b1111, 0b0000, 0b1001, 0b1010,
            0b0100, 0b0101, 0b0011
        }
    };

    public int Substitute(int input)
    {
        var row = ((input & 0b10000) >> 4) | (input & 0b1);
        var column = (input >> 1) & 0b1111;
        return _box[row, column];
    }

    public void Print()
    {
        for (var i = 0; i < 4; i++)
        {
            for (var j = 0; j < 16; j++) Console.Write($"{_box[i, j]:X2} ");

            Console.WriteLine();
        }
    }
}