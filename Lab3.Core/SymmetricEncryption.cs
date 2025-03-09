namespace Lab3.Core;

public class SymmetricEncryption
{
    private const int SizeOfBlock = 128;
    private const int SizeOfChar = 16;
    private const int ShiftKey = 2;
    private const int QuantityOfRounds = 16;
    private string[] Blocks;

    public string EncryptFile(string inFile, string outFile, string key)
    {
        if (key.Length > 0)
        {
            string s = string.Empty, decodeKey;
            using var reader = new StreamReader(inFile);
            while (!reader.EndOfStream)
            {
                s += reader.ReadLine();
            }

            s = StringToRightLength(s);
            Blocks = CutStringIntoBlocks(s);
            key = CorrectKeyWord(key, s.Length / (2 * Blocks.Length));
            key = StringToBinaryFormat(key);
            for (var j = 0; j < QuantityOfRounds; j++)
            {
                for (var i = 0; i < Blocks.Length; i++)
                    Blocks[i] = EncodeDesOneRound(Blocks[i], key);
                key = KeyToNextRound(key);
            }

            key = KeyToPreviousRound(key);
            decodeKey = StringFromBinaryToNormalFormat(key);
            var result = string.Empty;
            for (var i = 0; i < Blocks.Length; i++)
                result += Blocks[i];
            using var writer = new StreamWriter(outFile);
            writer.WriteLine(StringFromBinaryToNormalFormat(result));
            return decodeKey;
        }

        return null;
    }

    public void DecryptFile(string inFile, string outFile, string decodeKey)
    {
        if (decodeKey.Length > 0)
        {
            var s = string.Empty;
            var key = StringToBinaryFormat(decodeKey);
            using var reader = new StreamReader(inFile);
            while (!reader.EndOfStream)
            {
                s += reader.ReadLine();
            }

            s = StringToBinaryFormat(s);
            Blocks = CutBinaryStringIntoBlocks(s);
            for (var j = 0; j < QuantityOfRounds; j++)
            {
                for (var i = 0; i < Blocks.Length; i++)
                    Blocks[i] = DecodeDesOneRound(Blocks[i], decodeKey);
                key = KeyToPreviousRound(key);
            }
            key = KeyToNextRound(key);
            var result = string.Empty;
            for (var i = 0; i < Blocks.Length; i++)
            {
                result += Blocks[i];
            }
            using var writer = new StreamWriter(outFile);
            writer.WriteLine(StringFromBinaryToNormalFormat(result));
        }
    }
    
    private string StringToRightLength(string input)
    {
        while (((input.Length * SizeOfChar) % SizeOfBlock) != 0)
        {
            input += "#";
        }

        return input;
    }

    private string StringToBinaryFormat(string input)
    {
        var output = string.Empty;
        for (var i = 0; i < input.Length; i++)
        {
            var charBinary = Convert.ToString(input[i], 2);
            while (charBinary.Length < SizeOfChar)
            {
                charBinary = "0" + charBinary;
            }
            output += charBinary;
        }
        return output;
    }

    private string[] CutStringIntoBlocks(string input)
    {
        var blocks = new string[(input.Length * SizeOfChar) / SizeOfBlock];
        var lengthOfBlocks = input.Length / Blocks.Length;
        for (var i = 0; i < Blocks.Length; i++)
        {
            blocks[i] = input.Substring(i * lengthOfBlocks, lengthOfBlocks);
            blocks[i] = StringToBinaryFormat(Blocks[i]);
        }

        return blocks;
    }

    private string[] CutBinaryStringIntoBlocks(string input)
    {
        var blocks = new string[input.Length / SizeOfBlock];
        var lengthOfBlocks = input.Length / Blocks.Length;
        for (var i = 0; i < Blocks.Length; i++)
        {
            blocks[i] = input.Substring(i * lengthOfBlocks, lengthOfBlocks);
        }
        return blocks;
    }

    private string CorrectKeyWord(string input, int lengthKey)
    {
        if(input.Length > lengthKey) input = input.Substring(0, lengthKey);
        else
            while (input.Length < lengthKey)
            {
                input = "0" + input;
            }

        return input;
    }

    private string XOR(string first, string second)
    {
        var result = string.Empty;
        for (var i = 0; i < first.Length; i++)
        {
            var a = Convert.ToBoolean(Convert.ToInt32(first[i].ToString()));
            var b = Convert.ToBoolean(Convert.ToInt32(second[i].ToString()));
            if (a ^ b) result += "1";
            else result += "0";
        }

        return result;
    }

    private string F(string first, string second) => XOR(first, second);

    private string EncodeDesOneRound(string input, string key)
    {
        var l = input.Substring(0, input.Length / 2);
        var r = input.Substring(input.Length / 2, input.Length / 2);
        return r + XOR(l, F(r, key));
    }

    private string DecodeDesOneRound(string input, string key)
    {
        var l = input.Substring(0, input.Length / 2);
        var r = input.Substring(input.Length / 2, input.Length / 2);
        return XOR(F(l, key), r) + l;
    }

    private string KeyToNextRound(string key)
    {
        for (var i = 0; i < ShiftKey; i++)
        {
            key = key[key.Length - 1] + key;
            key = key.Remove(key.Length - 1);
        }

        return key;
    }

    private string KeyToPreviousRound(string key)
    {
        for (int i = 0; i < ShiftKey; i++)
        {
            key = key + key[0];
            key = key.Remove(0, 1);
        }

        return key;
    }

    private string StringFromBinaryToNormalFormat(string input)
    {
        var output = string.Empty;
        while (input.Length > 0)
        {
            var charBinary = input.Substring(0, SizeOfChar);
            input = input.Remove(0, SizeOfChar);
            var a = 0;
            var degree = charBinary.Length - 1;
            foreach (var c in charBinary)
            {
                a += Convert.ToInt32(c.ToString()) * (int)Math.Pow(2, degree--);
            }

            output += ((char)a).ToString();
        }

        return output;
    }
}