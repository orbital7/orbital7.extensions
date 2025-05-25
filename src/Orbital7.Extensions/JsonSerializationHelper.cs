using System.Text.Json;

namespace Orbital7.Extensions;

public static class JsonSerializationHelper
{
    private const bool DEFAULT_IGNORE_NULL_VALUES = true;
    private const bool DEFAULT_INDENT_FORMATTING = false;
    private const bool DEFAULT_CONVERT_ENUMS_TO_STRINGS = true;
    private const bool DEFAULT_PROPERTY_NAME_CASE_INSENSITIVE = false;

    public static async Task<T> CloneObjectAsync<T>(
        object objectToClone)
    {
        // TODO: Figure out why BOTH of these methods require the ignore null operator.
        string json = (await SerializeToJsonAsync(objectToClone))!;
        return (await DeserializeFromJsonAsync<T>(json))!;
    }

    public static T CloneObject<T>(
        object objectToClone)
    {
        string json = SerializeToJson(objectToClone);
        return DeserializeFromJson<T>(json);
    }

    public static JsonSerializerOptions SetSerializerOptions(
        JsonSerializerOptions options,
        bool ignoreNullValues = DEFAULT_IGNORE_NULL_VALUES,
        bool indentFormatting = DEFAULT_INDENT_FORMATTING,
        bool convertEnumsToStrings = DEFAULT_CONVERT_ENUMS_TO_STRINGS)
    {
        // Set defaults.
        options.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.DefaultIgnoreCondition = ignoreNullValues ?
            JsonIgnoreCondition.WhenWritingNull :
            JsonIgnoreCondition.Never;
        options.WriteIndented = indentFormatting;
        options.IncludeFields = true;

        // Ensure property naming policy is null so it won't auto-convert to camel case.
        options.PropertyNamingPolicy = null;

        // Handle enum to string conversion.
        if (convertEnumsToStrings)
        {
            options.Converters.Add(
                new JsonStringEnumMemberConverter());
        }

        return options;
    }

    public static JsonSerializerOptions CreateSerializationOptions(
        bool ignoreNullValues = DEFAULT_IGNORE_NULL_VALUES,
        bool indentFormatting = DEFAULT_INDENT_FORMATTING,
        bool convertEnumsToStrings = DEFAULT_CONVERT_ENUMS_TO_STRINGS)
    {
        return SetSerializerOptions(
            new JsonSerializerOptions(),
            ignoreNullValues,
            indentFormatting,
            convertEnumsToStrings);
    }

    public static JsonSerializerOptions CreateDeserializationOptions(
        bool propertyNameCaseInsensitive = DEFAULT_PROPERTY_NAME_CASE_INSENSITIVE,
        bool convertEnumsToStrings = DEFAULT_CONVERT_ENUMS_TO_STRINGS)
    {
        var options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = propertyNameCaseInsensitive,
            IncludeFields = true,
        };

        if (convertEnumsToStrings)
        {
            options.Converters.Add(
                new JsonStringEnumMemberConverter());
        }

        return options;
    }

    [return: NotNullIfNotNull(nameof(json))]
    public static async Task<T?> DeserializeFromJsonAsync<T>(
        string? json,
        bool propertyNameCaseInsensitive = false,
        bool convertEnumsToStrings = true)
    {
        if (json.HasText())
        {
            using (var stream = new MemoryStream(Encoding.Default.GetBytes(json)))
            {
                return await DeserializeAsync<T>(stream, propertyNameCaseInsensitive, convertEnumsToStrings);
            }
        }
        else
        {
            return default;
        }
    }

    [return: NotNullIfNotNull(nameof(json))]
    public static T? DeserializeFromJson<T>(
        string? json,
        bool propertyNameCaseInsensitive = DEFAULT_PROPERTY_NAME_CASE_INSENSITIVE,
        bool convertEnumsToStrings = DEFAULT_CONVERT_ENUMS_TO_STRINGS)
    {
        if (json.HasText())
        {
            using (var stream = new MemoryStream(Encoding.Default.GetBytes(json)))
            {
                // TODO: Figure out why this requires the ignore null operator.
                return Deserialize<T>(stream, propertyNameCaseInsensitive, convertEnumsToStrings)!;
            }
        }
        else
        {
            return default;
        }
    }

    public static async Task<object?> DeserializeFromJsonAsync(
        Type type,
        string? json,
        bool propertyNameCaseInsensitive = DEFAULT_PROPERTY_NAME_CASE_INSENSITIVE,
        bool convertEnumsToStrings = DEFAULT_CONVERT_ENUMS_TO_STRINGS)
    {
        if (json.HasText())
        {
            using (var stream = new MemoryStream(Encoding.Default.GetBytes(json)))
            {
                return await DeserializeAsync(type, stream, propertyNameCaseInsensitive, convertEnumsToStrings);
            }
        }
        else
        {
            return null;
        }
    }

    public static object? DeserializeFromJson(
        Type type,
        string? json,
        bool propertyNameCaseInsensitive = DEFAULT_PROPERTY_NAME_CASE_INSENSITIVE,
        bool convertEnumsToStrings = DEFAULT_CONVERT_ENUMS_TO_STRINGS)
    {
        if (json.HasText())
        {
            using (var stream = new MemoryStream(Encoding.Default.GetBytes(json)))
            {
                return Deserialize(type, stream, propertyNameCaseInsensitive, convertEnumsToStrings);
            }
        }
        else
        {
            return null;
        }
    }

    public static async Task<T?> DeserializeFromJsonFileAsync<T>(
        string filePath,
        bool propertyNameCaseInsensitive = DEFAULT_PROPERTY_NAME_CASE_INSENSITIVE,
        bool convertEnumsToStrings = DEFAULT_CONVERT_ENUMS_TO_STRINGS)
    {
        using (var openStream = File.OpenRead(filePath))
        {
            return await DeserializeAsync<T>(openStream, propertyNameCaseInsensitive, convertEnumsToStrings);
        }
    }

    public static T? DeserializeFromJsonFile<T>(
        string filePath,
        bool propertyNameCaseInsensitive = DEFAULT_PROPERTY_NAME_CASE_INSENSITIVE,
        bool convertEnumsToStrings = DEFAULT_CONVERT_ENUMS_TO_STRINGS)
    {
        using (var openStream = File.OpenRead(filePath))
        {
            return Deserialize<T>(openStream, propertyNameCaseInsensitive, convertEnumsToStrings);
        }
    }

    public static async Task<object?> DeserializeFromJsonFileAsync(
        Type type,
        string filePath,
        bool propertyNameCaseInsensitive = DEFAULT_PROPERTY_NAME_CASE_INSENSITIVE,
        bool convertEnumsToStrings = DEFAULT_CONVERT_ENUMS_TO_STRINGS)
    {
        using (var openStream = File.OpenRead(filePath))
        {
            return await DeserializeAsync(type, openStream, propertyNameCaseInsensitive, convertEnumsToStrings);
        }
    }

    public static object? DeserializeFromJsonFile(
        Type type,
        string filePath,
        bool propertyNameCaseInsensitive = DEFAULT_PROPERTY_NAME_CASE_INSENSITIVE,
        bool convertEnumsToStrings = DEFAULT_CONVERT_ENUMS_TO_STRINGS)
    {
        using (var openStream = File.OpenRead(filePath))
        {
            return Deserialize(type, openStream, propertyNameCaseInsensitive, convertEnumsToStrings);
        }
    }

    [return: NotNullIfNotNull(nameof(objectToSerialize))]
    public static async Task<string?> SerializeToJsonAsync(
        object? objectToSerialize,
        bool ignoreNullValues = DEFAULT_IGNORE_NULL_VALUES,
        bool indentFormatting = DEFAULT_INDENT_FORMATTING,
        bool convertEnumsToStrings = DEFAULT_CONVERT_ENUMS_TO_STRINGS)
    {
        using (var ms = new MemoryStream())
        {
            await SerializeAsync(objectToSerialize, ms, ignoreNullValues, indentFormatting, convertEnumsToStrings);
            if (ms.Length > 0)
            {
                return ms.ToArray().DecodeToString();
            }
            else
            {
                return null;
            }
        }
    }

    [return: NotNullIfNotNull(nameof(objectToSerialize))]
    public static string? SerializeToJson(
        object? objectToSerialize,
        bool ignoreNullValues = DEFAULT_IGNORE_NULL_VALUES,
        bool indentFormatting = DEFAULT_INDENT_FORMATTING,
        bool convertEnumsToStrings = DEFAULT_CONVERT_ENUMS_TO_STRINGS)
    {
        using (var ms = new MemoryStream())
        {
            Serialize(objectToSerialize, ms, ignoreNullValues, indentFormatting, convertEnumsToStrings);
            if (ms.Length > 0)
            {
                return ms.ToArray().DecodeToString();
            }
            else
            {
                return null;
            }
        }
    }

    public static async Task SerializeToJsonFileAsync(
        object? objectToSerialize,
        string filePath,
        bool ignoreNullValues = DEFAULT_IGNORE_NULL_VALUES,
        bool indentFormatting = DEFAULT_INDENT_FORMATTING,
        bool convertEnumsToStrings = DEFAULT_CONVERT_ENUMS_TO_STRINGS)
    {
        using (var fileStream = File.Create(filePath))
        {
            await SerializeAsync(objectToSerialize, fileStream, ignoreNullValues, indentFormatting, convertEnumsToStrings);
        }
    }

    public static void SerializeToJsonFile(
        object? objectToSerialize,
        string filePath,
        bool ignoreNullValues = DEFAULT_IGNORE_NULL_VALUES,
        bool indentFormatting = DEFAULT_INDENT_FORMATTING,
        bool convertEnumsToStrings = DEFAULT_CONVERT_ENUMS_TO_STRINGS)
    {
        using (var fileStream = File.Create(filePath))
        {
            Serialize(objectToSerialize, fileStream, ignoreNullValues, indentFormatting, convertEnumsToStrings);
        }
    }

    private static async Task SerializeAsync(
        object? objectToSerialize,
        Stream stream,
        bool ignoreNullValues,
        bool indentFormatting,
        bool convertEnumsToStrings)
    {
        if (objectToSerialize != null)
        {
            await JsonSerializer.SerializeAsync(
                stream,
                objectToSerialize,
                objectToSerialize.GetType(),
                CreateSerializationOptions(
                    ignoreNullValues,
                    indentFormatting,
                    convertEnumsToStrings));
        }
    }

    private static void Serialize(
        object? objectToSerialize,
        Stream stream,
        bool ignoreNullValues,
        bool indentFormatting,
        bool convertEnumsToStrings)
    {
        if (objectToSerialize != null)
        {
            JsonSerializer.Serialize(
                stream,
                objectToSerialize,
                objectToSerialize.GetType(),
                CreateSerializationOptions(
                    ignoreNullValues,
                    indentFormatting,
                    convertEnumsToStrings));
        }
    }

    private static async Task<T?> DeserializeAsync<T>(
        Stream stream,
        bool propertyNameCaseInsensitive,
        bool convertEnumsToStrings)
    {
        return await JsonSerializer.DeserializeAsync<T>(
            stream,
            CreateDeserializationOptions(
                propertyNameCaseInsensitive, 
                convertEnumsToStrings));
    }

    private static T? Deserialize<T>(
        Stream stream,
        bool propertyNameCaseInsensitive,
        bool convertEnumsToStrings)
    {
        return JsonSerializer.Deserialize<T>(
            stream,
            CreateDeserializationOptions(
                propertyNameCaseInsensitive,
                convertEnumsToStrings));
    }

    private static async Task<object?> DeserializeAsync(
        Type type,
        Stream stream,
        bool propertyNameCaseInsensitive,
        bool convertEnumsToStrings)
    {
        if (stream.Length > 0)
        {
            return await JsonSerializer.DeserializeAsync(
                stream,
                type,
                CreateDeserializationOptions(
                    propertyNameCaseInsensitive,
                    convertEnumsToStrings));
        }
        else
        {
            return null;
        }
    }

    private static object? Deserialize(
        Type type,
        Stream stream,
        bool propertyNameCaseInsensitive,
        bool convertEnumsToStrings)
    {
        if (stream.Length > 0)
        {
            return JsonSerializer.Deserialize(
                stream,
                type,
                CreateDeserializationOptions(
                    propertyNameCaseInsensitive,
                    convertEnumsToStrings));
        }
        else
        {
            return null;
        }
    }
}