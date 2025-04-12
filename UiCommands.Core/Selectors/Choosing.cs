namespace UiCommands.Core.Selectors;

public static class Choosing
{
    public delegate bool CompareDelegate<in TK, in T>(T first, TK second);

    public delegate TK ParserDelegate<out TK>(string input);

    public delegate string TitleDelegate<in T>(T item);

    public static string GetText(string name)
    {
        Console.Write($"Введіть {name}: ");
        return Console.ReadLine() ?? string.Empty;
    }

    public static T GetFromList<T, TK>(
        List<T> list,
        string name,
        TitleDelegate<T> titleDelegate,
        ParserDelegate<TK> parserDelegate,
        CompareDelegate<T, TK> compareDelegate,
        bool byDefault = true
    )
    {
        if (list.Count == 0) throw new Exception($"Список \"{name}\" пустий");
        if (list.Count == 1 && byDefault) return list.First();
        bool isError;
        T? result;
        do
        {
            Console.WriteLine($"Виберіть зі \"{name}\"");
            list.Select(x => titleDelegate(x)).ToList().ForEach(Console.WriteLine);
            Console.Write(">> ");
            var value = parserDelegate(Console.ReadLine() ?? string.Empty);
            result = list.FirstOrDefault(x => compareDelegate(value, x));
            isError = result == null;
            if (isError) Console.WriteLine("Відповідь не розпізнано.");
        } while (isError);

        return result!;
    }

    public static int GetNumber(string name, int? min, int? max)
    {
        var hasMin = min != null;
        var hasMax = max != null;
        var hasConditions = hasMin || hasMax;
        var maxText = hasMax ? $"меньше рівне ніж {max}" : string.Empty;
        var minText = hasMin ? $"більше рівне за {min}" : string.Empty;
        var start = hasConditions ? "(" : string.Empty;
        var end = hasConditions ? ")" : string.Empty;
        var middle = hasMin && hasMax ? " та " : string.Empty;
        var fullText = $"{name}{start}{minText}{middle}{maxText}{end}";
        int value;
        bool isError;
        do
        {
            Console.Write($"Введіть {fullText}: ");
            isError = TryGetParseNumber(Console.ReadLine() ?? string.Empty, out value) || value < min || value > max;
            if (isError) Console.WriteLine("Ви ввели не правильнк значення, спробуйте ше.");
        } while (isError);

        return value;
    }

    private static bool TryGetParseNumber(string text, out int value)
    {
        var isNotParsed = int.TryParse(text, out value) == false;
        return isNotParsed;
    }
}