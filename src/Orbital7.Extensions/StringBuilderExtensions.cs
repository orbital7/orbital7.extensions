namespace Orbital7.Extensions;

public static class StringBuilderExtensions
{
    public static StringBuilder AppendLine(
        this StringBuilder sb, 
        string? value, 
        string lineTerminator)
    {
        sb.Append(value);
        sb.Append(lineTerminator);
        return sb;
    }

    public static StringBuilder AppendCommaSeparatedValuesLine(
        this StringBuilder sb,
        params string?[] values)
    {
        sb.AppendLine(string.Join(",", values));
        return sb;
    }

    public static StringBuilder AppendTabSeparatedValuesLine(
        this StringBuilder sb,
        params string?[] values)
    {
        sb.AppendLine(string.Join("\t", values));
        return sb;
    }
}
