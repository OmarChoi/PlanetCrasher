using System.Text;

public static class UpgradeDescriptionBuilder
{
    private static readonly StringBuilder _stringBuilder = new StringBuilder();

    private static string Generate(EffectDescriptionTableSO table, UpgradeEffect effect, int level)
    {
        string format = table.GetFormat(effect.Type);
        double value = UpgradeCalculator.CalculateEffect(effect, level);
        return string.Format(format, value.ToCompactString());
    }

    public static string GenerateAll(EffectDescriptionTableSO table, Upgrade upgrade)
    {
        if (upgrade.MetaData.Effects == null || upgrade.MetaData.Effects.Length == 0)
        {
            return string.Empty;
        }

        _stringBuilder.Clear();

        for (int i = 0; i < upgrade.MetaData.Effects.Length; i++)
        {
            if (i > 0) _stringBuilder.Append('\n');
            _stringBuilder.Append(Generate(table, upgrade.MetaData.Effects[i], upgrade.Level));
        }

        return _stringBuilder.ToString();
    }
}
