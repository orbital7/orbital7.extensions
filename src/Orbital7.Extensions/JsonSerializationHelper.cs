using System.IO.Pipes;

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
        string json)
    {
        using (var stream = new MemoryStream(Encoding.Default.GetBytes(json)))
        {
            return await DeserializeAsync<T>(stream);
        }
    }

    public static T DeserializeFromJson<T>(
        string json)
    {
        using (var stream = new MemoryStream(Encoding.Default.GetBytes(json)))
        {
            return Deserialize<T>(stream);
        }
    }

    public static async Task<object> DeserializeFromJsonAsync(
        Type type,
        string json)
    {
        using (var stream = new MemoryStream(Encoding.Default.GetBytes(json)))
        {
            return await DeserializeAsync(type, stream);
        }
    }

    public static object DeserializeFromJson(
        Type type,
        string json)
    {
        using (var stream = new MemoryStream(Encoding.Default.GetBytes(json)))
        {
            return Deserialize(type, stream);
        }
    }

    public static async Task<T> DeserializeFromJsonFileAsync<T>(
        string filePath)
    {
        using (var openStream = File.OpenRead(filePath))
        {
            return await DeserializeAsync<T>(openStream);
        }
    }

    public static T DeserializeFromJsonFile<T>(
        string filePath)
    {
        using (var openStream = File.OpenRead(filePath))
        {
            return Deserialize<T>(openStream);
        }
    }

    public static async Task<object> DeserializeFromJsonFileAsync(
        Type type,
        string filePath)
    {
        using (var openStream = File.OpenRead(filePath))
        {
            return await DeserializeAsync(type, openStream);
        }
    }

    public static object DeserializeFromJsonFile(
        Type type,
        string filePath)
    {
        using (var openStream = File.OpenRead(filePath))
        {
            return Deserialize(type, openStream);
        }
    }

    public static async Task<string> SerializeToJsonAsync(
        object objectToSerialize,
        bool ignoreNullValues = true,
        bool indentFormatting = false)
    {
        using (var ms = new MemoryStream())
        {
            await SerializeAsync(objectToSerialize, ms, ignoreNullValues, indentFormatting);
            using (var reader = new StreamReader(ms))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }

    public static string SerializeToJson(
        object objectToSerialize,
        bool ignoreNullValues = true,
        bool indentFormatting = false)
    {
        using (var ms = new MemoryStream())
        {
            Serialize(objectToSerialize, ms, ignoreNullValues, indentFormatting);
            using (var reader = new StreamReader(ms))
            {
                return reader.ReadToEnd();
            }
        }
    }

    public static async Task SerializeToJsonFileAsync(
        object objectToSerialize,
        string filePath,
        bool ignoreNullValues = true,
        bool indentFormatting = false)
    {
        using (var fileStream = File.Create(filePath))
        {
            await SerializeAsync(objectToSerialize, fileStream, ignoreNullValues, indentFormatting);
        }
    }

    public static void SerializeToJsonFile(
        object objectToSerialize,
        string filePath,
        bool ignoreNullValues = true,
        bool indentFormatting = false)
    {
        using (var fileStream = File.Create(filePath))
        {
            Serialize(objectToSerialize, fileStream, ignoreNullValues, indentFormatting);
        }
    }

    private static async Task SerializeAsync(
        object objectToSerialize,
        Stream stream,
        bool ignoreNullValues,
        bool indentFormatting)
    {
        await JsonSerializer.SerializeAsync(
            stream, 
            objectToSerialize, 
            objectToSerialize.GetType(),
            CreateOptions(
                ignoreNullValues,
                indentFormatting));
    }

    private static void Serialize(
        object objectToSerialize,
        Stream stream,
        bool ignoreNullValues,
        bool indentFormatting)
    {
        JsonSerializer.Serialize(
            stream,
            objectToSerialize,
            objectToSerialize.GetType(),
            CreateOptions(
                ignoreNullValues,
                indentFormatting));
    }

    private static async Task<T> DeserializeAsync<T>(
        Stream stream)
    {
        return await JsonSerializer.DeserializeAsync<T>(stream);
    }

    private static T Deserialize<T>(
        Stream stream)
    {
        return JsonSerializer.Deserialize<T>(stream);
    }

    private static async Task<object> DeserializeAsync(
        Type type,
        Stream stream)
    {
        return await JsonSerializer.DeserializeAsync(stream, type);
    }

    private static object Deserialize(
        Type type,
        Stream stream)
    {
        return JsonSerializer.Deserialize(stream, type);
    }

    private static JsonSerializerOptions CreateOptions(
        bool ignoreNullValues,
        bool indentFormatting)
    {
        return new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            DefaultIgnoreCondition = ignoreNullValues ?
                JsonIgnoreCondition.WhenWritingNull :
                JsonIgnoreCondition.Never,
            WriteIndented = indentFormatting,
        };
    }
}