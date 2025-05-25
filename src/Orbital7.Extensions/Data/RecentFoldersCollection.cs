using System.Collections.ObjectModel;

namespace Orbital7.Extensions.Data;

public class RecentFoldersCollection :
    Collection<DirectoryInfo>
{
    private const int DEFAULT_LIMIT = 10;

    public DirectoryInfo Add(
        string folderPath,
        int limit = DEFAULT_LIMIT)
    {
        var folder = new DirectoryInfo(folderPath);

        Remove(folder);
        EnsureLimit(limit);

        this.Insert(0, folder);

        return folder;
    }

    public new void Remove(
        DirectoryInfo directory)
    {
        for (int i = this.Count - 1; i >= 0; i--)
        {
            if (this[i].FullName.Equals(directory.FullName, StringComparison.OrdinalIgnoreCase))
            {
                this.RemoveAt(i);
            }
        }
    }

    public string SerializeToJson()
    {
        var folderPaths = this.Select(x => x.FullName).ToList();
        return JsonSerializationHelper.SerializeToJson(folderPaths);
    }

    public static RecentFoldersCollection LoadFromJson(
         string serializedFolderPaths,
        int limit = DEFAULT_LIMIT)
    {
        var collection = new RecentFoldersCollection();

        var folderPaths = JsonSerializationHelper.DeserializeFromJson<List<string>>(
            serializedFolderPaths);

        if (folderPaths != null)
        {
            foreach (string folderPath in folderPaths)
            {
                if (Directory.Exists(folderPath))
                {
                    collection.Add(new DirectoryInfo(folderPath));
                }
            }

            collection.EnsureLimit(limit);
        }

        return collection;
    }

    private void EnsureLimit(
        int limit)
    {
        for (int i = this.Count - 1; i >= limit; i--)
        {
            this.RemoveAt(i);
        }
    }
}
