namespace Orbital7.Extensions;

public static class StringBuilderExtensions
{
    public static StringBuilder AppendLine(
        this StringBuilder sb, 
        string value, 
        string lineTerminator)
    {
        sb.Append(value);
        sb.Append(lineTerminator);
        return sb;
    }
}
