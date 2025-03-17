namespace Lab3.Core.Encryptions;

public partial class SymmetricEncryption
{
    private static byte[] EncodeDesOneRound(byte[] input, byte[] key)
    {
        var k = input.Length / 2;
        var result = new byte[input.Length];
        var l = new byte[k];
        var r = new byte[k];
        Array.Copy(input, 0, l, 0, k);
        Array.Copy(input, k, r, 0, k);
        var temp = Xor(l, F(r, key));
        Array.Copy(r, 0, result, 0, k);
        Array.Copy(temp, 0, result, k, k);
        return result;
    }

    private static byte[] DecodeDesOneRound(byte[] input, byte[] key)
    {
        var k = input.Length / 2;
        var result = new byte[input.Length];
        var l = new byte[k];
        var r = new byte[k];
        Array.Copy(input, 0, l, 0, k);
        Array.Copy(input, k, r, 0, k);
        var temp = Xor(F(l, key), r);
        Array.Copy(temp, 0, result, 0, k);
        Array.Copy(l, 0, result, k, k);
        return result;
    }
}