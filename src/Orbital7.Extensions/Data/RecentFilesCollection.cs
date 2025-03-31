using System.Collections.ObjectModel;

namespace Orbital7.Extensions.Data;

public class RecentFilesCollection :
    Collection<FileInfo>
{
    private const int LIMIT = 10;

    public FileInfo Add(
        string filePath)
    {
        var file = new FileInfo(filePath);

        Remove(file);
        EnsureLimit();

        this.Insert(0, file);

        return file;
    }

    public new void Remove(
        FileInfo file)
    {
        for (int i = this.Count - 1; i >= 0; i--)
        {
            if (this[i].FullName.Equals(file.FullName, StringComparison.OrdinalIgnoreCase))
            {
                this.RemoveAt(i);
            }
        }
    }

    public string SerializeToJson()
    {
        var filePaths = this.Select(x => x.FullName).ToList();
        return JsonSerializationHelper.SerializeToJson(filePaths);
    }

    public static RecentFilesCollection LoadFromJson(
         string serializedFilePaths)
    {
        var collection = new RecentFilesCollection();

        var filePaths = JsonSerializationHelper.DeserializeFromJson<List<string>>(
            serializedFilePaths);

        if (filePaths != null)
        {
            foreach (string filePath in filePaths)
            {
                if (File.Exists(filePath))
                {
                    collection.Add(new FileInfo(filePath));
                }
            }

            collection.EnsureLimit();
        }

        return collection;
    }

    private void EnsureLimit()
    {
        for (int i = this.Count - 1; i >= LIMIT; i--)
        {
            this.RemoveAt(i);
        }
    }
}
