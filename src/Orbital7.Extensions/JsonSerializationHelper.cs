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

    public static async Task<string> SerializeToJsonAsync(
        object objectToSerialize,
        bool ignoreNullValues = true)
    {
        using (var ms = new MemoryStream())
        {
            await SerializeAsync(objectToSerialize, ms, ignoreNullValues);
            using (var reader = new StreamReader(ms))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }

    public static string SerializeToJson(
        object objectToSerialize,
        bool ignoreNullValues = true)
    {
        using (var ms = new MemoryStream())
        {
            Serialize(objectToSerialize, ms, ignoreNullValues);
            using (var reader = new StreamReader(ms))
            {
                return reader.ReadToEnd();
            }
        }
    }

    public static async Task SerializeToJsonFileAsync(
        object objectToSerialize,
        string filePath,
        bool ignoreNullValues = true)
    {
        using (var fileStream = File.Create(filePath))
        {
            await SerializeAsync(objectToSerialize, fileStream, ignoreNullValues);
        }
    }

    public static void SerializeToJsonFile(
        object objectToSerialize,
        string filePath,
        bool ignoreNullValues = true)
    {
        using (var fileStream = File.Create(filePath))
        {
            Serialize(objectToSerialize, fileStream, ignoreNullValues);
        }
    }

    private static async Task SerializeAsync(
        object objectToSerialize,
        Stream stream,
        bool ignoreNullValues)
    {
        await JsonSerializer.SerializeAsync(
            stream, 
            objectToSerialize, 
            objectToSerialize.GetType(),
            CreateOptions(ignoreNullValues));
    }

    private static void Serialize(
        object objectToSerialize,
        Stream stream,
        bool ignoreNullValues)
    {
        JsonSerializer.Serialize(
            stream,
            objectToSerialize,
            objectToSerialize.GetType(),
            CreateOptions(ignoreNullValues));
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

    private static JsonSerializerOptions CreateOptions(
        bool ignoreNullValues)
    {
        return new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            DefaultIgnoreCondition = ignoreNullValues ?
                JsonIgnoreCondition.WhenWritingNull :
                JsonIgnoreCondition.Never,
        };
    }
}