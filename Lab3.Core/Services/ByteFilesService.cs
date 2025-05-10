namespace Lab3.Core.Services;

public static class ByteFilesService
{
    public static void WriteBytes(string outputFile, byte[] result, int length)
    {
        using var writer = new FileStream(outputFile, FileMode.OpenOrCreate);
        writer.Write(result, 0, length);
    }

    public static byte[] ReadBytes(string inputFile)
    {
        using var reader = File.OpenRead(inputFile);
        var array = new byte[reader.Length];
        reader.ReadExactly(array, 0, array.Length);
        return array;
    }
}