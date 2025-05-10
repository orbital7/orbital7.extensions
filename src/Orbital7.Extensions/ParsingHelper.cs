using CsvHelper;
using CsvHelper.Configuration;

namespace Orbital7.Extensions;

public static class ParsingHelper
{
    public static List<TType> ParseTextFileToTypedList<TType>(
        string filePath)
    {
        if (File.Exists(filePath))
        {
            var fileContents = File.ReadAllText(filePath);
            return ParseTextLinesToTypedList<TType>(fileContents);
        }
        else
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }
    }

    public static List<TType> ParseTextLinesToTypedList<TType>(
        string text)
    {
        var lines = text.ParseLines();
        return lines.ToTypedList<TType>();
    }

    public static List<TModel> ParseCsvFileToModels<TModel>(
        string filePath,
        bool hasColumnHeadersRow)
    {
        if (File.Exists(filePath))
        {
            using (var reader = new StreamReader(filePath))
            {
                return ParseCsvStreamReaderToModels<TModel>(
                    reader,
                    hasColumnHeadersRow);
            }
        }
        else
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }
    }

    public static List<TModel> ParseCsvContentsToModels<TModel>(
        string csvContents,
        bool hasColumnHeadersRow)
    {
        using (var ms = csvContents.ToStream())
        {
            using (var reader = new StreamReader(ms))
            {
                return ParseCsvStreamReaderToModels<TModel>(
                    reader,
                    hasColumnHeadersRow);
            }
        }
    }

    private static List<TModel> ParseCsvStreamReaderToModels<TModel>(
        StreamReader reader,
        bool hasColumnHeadersRow)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = hasColumnHeadersRow,
        };

        using (var csv = new CsvReader(reader, config))
        {
            return csv.GetRecords<TModel>().ToList();
        }
    }
}
