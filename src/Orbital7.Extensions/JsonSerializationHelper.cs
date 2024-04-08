namespace System.Text.Json.Serialization;

public static class JsonSerializationHelper
{
    public static async Task<T> CloneObjectAsync<T>(
        object objectToClone)
    {
        return await DeserializeFromJsonAsync<T>(
            await SerializeToJsonAsync(objectToClone));
    }

    public static T CloneObject<T>(
        object objectToClone)
    {
        return DeserializeFromJson<T>(
            SerializeToJson(objectToClone));
    }

    public static async Task<T> DeserializeFromJsonAsync<T>(
        string json,
        bool propertyNameCaseInsensitive = false,
        bool convertEnumsToStrings = true)
    {
        using (var stream = new MemoryStream(Encoding.Default.GetBytes(json)))
        {
            return await DeserializeAsync<T>(stream, propertyNameCaseInsensitive, convertEnumsToStrings);
        }
    }

    public static T DeserializeFromJson<T>(
        string json,
        bool propertyNameCaseInsensitive = false,
        bool convertEnumsToStrings = true)
    {
        using (var stream = new MemoryStream(Encoding.Default.GetBytes(json)))
        {
            return Deserialize<T>(stream, propertyNameCaseInsensitive, convertEnumsToStrings);
        }
    }

    public static async Task<object> DeserializeFromJsonAsync(
        Type type,
        string json,
        bool propertyNameCaseInsensitive = false,
        bool convertEnumsToStrings = true)
    {
        using (var stream = new MemoryStream(Encoding.Default.GetBytes(json)))
        {
            return await DeserializeAsync(type, stream, propertyNameCaseInsensitive, convertEnumsToStrings);
        }
    }

    public static object DeserializeFromJson(
        Type type,
        string json,
        bool propertyNameCaseInsensitive = false,
        bool convertEnumsToStrings = true)
    {
        using (var stream = new MemoryStream(Encoding.Default.GetBytes(json)))
        {
            return Deserialize(type, stream, propertyNameCaseInsensitive, convertEnumsToStrings);
        }
    }

    public static async Task<T> DeserializeFromJsonFileAsync<T>(
        string filePath,
        bool propertyNameCaseInsensitive = false,
        bool convertEnumsToStrings = true)
    {
        using (var openStream = File.OpenRead(filePath))
        {
            return await DeserializeAsync<T>(openStream, propertyNameCaseInsensitive, convertEnumsToStrings);
        }
    }

    public static T DeserializeFromJsonFile<T>(
        string filePath,
        bool propertyNameCaseInsensitive = false,
        bool convertEnumsToStrings = true)
    {
        using (var openStream = File.OpenRead(filePath))
        {
            return Deserialize<T>(openStream, propertyNameCaseInsensitive, convertEnumsToStrings);
        }
    }

    public static async Task<object> DeserializeFromJsonFileAsync(
        Type type,
        string filePath,
        bool propertyNameCaseInsensitive = false,
        bool convertEnumsToStrings = true)
    {
        using (var openStream = File.OpenRead(filePath))
        {
            return await DeserializeAsync(type, openStream, propertyNameCaseInsensitive, convertEnumsToStrings);
        }
    }

    public static object DeserializeFromJsonFile(
        Type type,
        string filePath,
        bool propertyNameCaseInsensitive = false,
        bool convertEnumsToStrings = true)
    {
        using (var openStream = File.OpenRead(filePath))
        {
            return Deserialize(type, openStream, propertyNameCaseInsensitive, convertEnumsToStrings);
        }
    }

    public static async Task<string> SerializeToJsonAsync(
        object objectToSerialize,
        bool ignoreNullValues = true,
        bool indentFormatting = false,
        bool convertEnumsToStrings = true)
    {
        using (var ms = new MemoryStream())
        {
            await SerializeAsync(objectToSerialize, ms, ignoreNullValues, indentFormatting, convertEnumsToStrings);
            return ms.ToArray().ToTextString();
        }
    }

    public static string SerializeToJson(
        object objectToSerialize,
        bool ignoreNullValues = true,
        bool indentFormatting = false,
        bool convertEnumsToStrings = true)
    {
        using (var ms = new MemoryStream())
        {
            Serialize(objectToSerialize, ms, ignoreNullValues, indentFormatting, convertEnumsToStrings);
            return ms.ToArray().ToTextString();
        }
    }

    public static async Task SerializeToJsonFileAsync(
        object objectToSerialize,
        string filePath,
        bool ignoreNullValues = true,
        bool indentFormatting = false,
        bool convertEnumsToStrings = true)
    {
        using (var fileStream = File.Create(filePath))
        {
            await SerializeAsync(objectToSerialize, fileStream, ignoreNullValues, indentFormatting, convertEnumsToStrings);
        }
    }

    public static void SerializeToJsonFile(
        object objectToSerialize,
        string filePath,
        bool ignoreNullValues = true,
        bool indentFormatting = false,
        bool convertEnumsToStrings = true)
    {
        using (var fileStream = File.Create(filePath))
        {
            Serialize(objectToSerialize, fileStream, ignoreNullValues, indentFormatting, convertEnumsToStrings);
        }
    }

    private static async Task SerializeAsync(
        object objectToSerialize,
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
        object objectToSerialize,
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

    private static async Task<T> DeserializeAsync<T>(
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

    private static T Deserialize<T>(
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

    private static async Task<object> DeserializeAsync(
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

    private static object Deserialize(
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

    private static JsonSerializerOptions CreateSerializationOptions(
        bool ignoreNullValues,
        bool indentFormatting,
        bool convertEnumsToStrings)
    {
        var options = new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            DefaultIgnoreCondition = ignoreNullValues ?
                JsonIgnoreCondition.WhenWritingNull :
                JsonIgnoreCondition.Never,
            WriteIndented = indentFormatting,
        };

        if (convertEnumsToStrings)
        {
            options.Converters.Add(
                new JsonStringEnumMemberConverter());
        }

        return options;
    }

    private static JsonSerializerOptions CreateDeserializationOptions(
        bool propertyNameCaseInsensitive,
        bool convertEnumsToStrings)
    {
        var options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = propertyNameCaseInsensitive,
        };

        if (convertEnumsToStrings)
        {
            options.Converters.Add(
                new JsonStringEnumMemberConverter());
        }

        return options;
    }
}