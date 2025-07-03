namespace Orbital7.Extensions;

public static class HtmlHelper
{
    public static string FormatLineTerminatorsAsHtml(
        string value)
    {
        var lineTerminator = "\n";

        return value
            .NormalizeLineTerminators(lineTerminator)
            .Replace(lineTerminator, "<br />");
    }
}
