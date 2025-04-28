using System.Runtime.InteropServices;

namespace Lab3.Core.Encryptions.RSA;

[StructLayout(LayoutKind.Explicit)]
public struct RSApkde
{
    [FieldOffset(0)] public ulong RSAInt;
    [FieldOffset(0)] public byte RSAByte1;
    [FieldOffset(1)] public byte RSAByte2;
    [FieldOffset(2)] public byte RSAByte3;
    [FieldOffset(3)] public byte RSAByte4;
}