namespace Orbital7.Extensions;

public class CsvModelClassBuilder
{
    public string BuildClassFromCsvHeader(
        string header,
        string classNamespace,
        string className)
    {
        var sb = new StringBuilder();
        sb.AppendLine("using CsvHelper.Configuration.Attributes;");
        sb.AppendLine();
        sb.AppendLine($"namespace {classNamespace};");
        sb.AppendLine();
        sb.AppendLine($"public class {className}");
        sb.AppendLine("{");

        var headerFragments = ParseAndIncludeDelims(header, '"');

        foreach (var headerFragment in headerFragments)
        {
            var cleanedHeaderFragment = headerFragment.Trim();
            if (cleanedHeaderFragment.StartsWith("\","))
            {
                ParseCommaSeparatedFragments(sb, cleanedHeaderFragment);
            }
            else if (cleanedHeaderFragment.StartsWith("\""))
            {
                // Ignore ending quote fragments (which will have a comma).
                if (cleanedHeaderFragment.Length > 2)
                {
                    AppendField(sb, cleanedHeaderFragment);
                }
            }
            else
            {
                ParseCommaSeparatedFragments(sb, cleanedHeaderFragment);
            }
        }

        sb.Append("}");

        return sb.ToString();
    }

    public void WriteClassFileFromCsvHeader(
        string header,
        string classNamespace,
        string className,
        string filePath)
    {
        var classContents = BuildClassFromCsvHeader(
            header, 
            classNamespace, 
            className);

        File.WriteAllText(filePath, classContents);
    }

    private void ParseCommaSeparatedFragments(
        StringBuilder sb,
        string cleanedHeaderFragment)
    {
        var commaSeparatedFragments = cleanedHeaderFragment.Split(",", StringSplitOptions.RemoveEmptyEntries);
        foreach (var commaSeparatedFragment in commaSeparatedFragments)
        {
            AppendField(sb, commaSeparatedFragment);
        }
    }

    private void AppendField(
        StringBuilder sb,
        string field)
    {
        var cleanedField = field.Trim().RemoveQuotes();
        if (cleanedField.HasText())
        {
            sb.AppendLine($"\t[Name(\"{cleanedField}\")]");
            sb.AppendLine($"\tpublic string {GetVariableName(cleanedField)} {{ get; set; }}");
            sb.AppendLine();
        }
    }

    private string GetVariableName(
        string headerName)
    {
        var variable = headerName
            .Replace("\"", "")
            .Replace("'", "")
            .Replace(":", "_")
            .Replace(" ", "_")
            .Replace("?", "_")
            .Replace(";", "_")
            .Replace(",", "_")
            .Replace("(", "_")
            .Replace(")", "_")
            .Replace("*", "_")
            .Replace("+", "_")
            .Replace("-", "_")
            .Replace("!", "_")
            .Replace("@", "_")
            .Replace("#", "_")
            .Replace("$", "_")
            .Replace("%", "_")
            .Replace("^", "_")
            .Replace("&", "_")
            .Replace("=", "_")
            .Replace("~", "_")
            .Replace("`", "_")
            .Replace("<", "_")
            .Replace(">", "_")
            .Replace(".", "_")
            .Replace("/", "_")
            .Replace("\\", "_");

        return variable;
    }

    private List<string> ParseAndIncludeDelims(
        string input,
        char delim)
    {
        var list = new List<string>();
        var sb = new StringBuilder();
        var chars = input.ToCharArray();
        var curLength = 0;

        foreach (var c in chars)
        {
            if (c == delim && curLength > 0)
            {
                list.Add(sb.ToString());
                sb.Clear();
                curLength = 0;
            }

            sb.Append(c);
            curLength++;
        }

        if (curLength > 0)
        {
            list.Add(sb.ToString());
        }

        return list;
    }
}