public static class NumberFormatExtension
{
    private static readonly string[] _suffixes =
    {
        "", "K", "M", "B", "T",
        "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj",
        "ak", "al", "am", "an", "ao", "ap", "aq", "ar", "as", "at",
        "au", "av", "aw", "ax", "ay", "az",
        "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj",
        "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br", "bs", "bt",
        "bu", "bv", "bw", "bx", "by", "bz"
    };
    
    public static string ToFormattedString(this double number)
    {
        if (number < 1000) return $"<sprite=0>{number:N0}";

        int suffixIndex = 0;

        double value = number;
        while (value >= 1000 && suffixIndex < _suffixes.Length - 1)
        {
            value /= 1000;
            suffixIndex++;
        }

        return value switch
        {
            >= 100 => $"<sprite=0>{value:F0}{_suffixes[suffixIndex]}",
            >= 10  => $"<sprite=0>{value:F1}{_suffixes[suffixIndex]}",
            _      => $"<sprite=0>{value:F2}{_suffixes[suffixIndex]}"
        };
    }

    public static string ToCompactString(this double number)
    {
        if (number < 1000) return number == (int)number ? $"{(int)number}" : $"{number:F1}";

        int suffixIndex = 0;

        double value = number;
        while (value >= 1000 && suffixIndex < _suffixes.Length - 1)
        {
            value /= 1000;
            suffixIndex++;
        }

        return value switch
        {
            >= 100 => $"{value:F0}{_suffixes[suffixIndex]}",
            >= 10  => $"{value:F1}{_suffixes[suffixIndex]}",
            _      => $"{value:F2}{_suffixes[suffixIndex]}"
        };
    }
}
