using System.Text;

namespace Lab3.Core;

public static class Convertor
{
    public static byte[] UlongTyByte(long k)
    {
        var key = new byte[4];
        for (var i = 0; i < 4; i++) key[3 - i] = (byte)((k & (255 << (i * 8))) >> (i * 8));
        return key;
    }

    public static string BytesToString(byte[] bytes)
    {
        return Encoding.UTF8.GetString(bytes);
    }

    public static byte[] StringToBytes(string text)
    {
        return Encoding.UTF8.GetBytes(text);
    }
}