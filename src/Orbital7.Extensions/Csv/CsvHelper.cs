using CsvHelper;
using CsvHelper.Configuration;

namespace Orbital7.Extensions.Csv;

public static class CsvHelper
{
    public static List<TModel> ParseHeaderedCsvFileToModels<TModel>(
        string filePath)
    {
        if (File.Exists(filePath))
        {
            using (var reader = new StreamReader(filePath))
            {
                return ParseHeaderedCsvStreamReaderToModels<TModel>(reader);
            }
        }
        else
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }
    }

    public static List<TModel> ParseHeaderedCsvContentsToModels<TModel>(
        string csvContents)
    {
        using (var ms = csvContents.ToStream())
        {
            using (var reader = new StreamReader(ms))
            {
                return ParseHeaderedCsvStreamReaderToModels<TModel>(reader);
            }
        }
    }

    private static List<TModel> ParseHeaderedCsvStreamReaderToModels<TModel>(
        StreamReader reader)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
        };

        using (var csv = new CsvReader(reader, config))
        {
            return csv.GetRecords<TModel>().ToList();
        }
    }
}
