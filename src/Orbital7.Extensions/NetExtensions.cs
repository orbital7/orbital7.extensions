namespace System.Net;

public static class NetExtensions
{
    public static async Task<string> ReadAsStringAsync(
        this WebResponse webResponse)
    {
        using (var reader = new StreamReader(webResponse.GetResponseStream()))
        {
            return await reader.ReadToEndAsync();
        }
    }
}
