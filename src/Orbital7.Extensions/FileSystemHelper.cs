using System.Linq;

namespace System.IO;

public static class FileSystemHelper
{
    public static string NormalizePathSeparator(string path, string separator, string separatorsList = "/\\")
    {
        string normalizedPath = path;

        var chars = separatorsList.ToList();
        foreach (var c in chars)
            normalizedPath = normalizedPath.Replace(c.ToString(), separator);

        return normalizedPath;
    }

    public static string GetFolderSize(string folderPath)
    {
        long size = 0;

        foreach (FileInfo file in new DirectoryInfo(folderPath).GetFiles())
            size += file.Length;

        return size.ToFileSize();
    }

    public static string GetParentFolderPath(string path)
    {
        if (Directory.Exists(path))
            return new DirectoryInfo(path).Parent.FullName;
        else if (File.Exists(path))
            return new FileInfo(path).Directory.Parent.FullName;
        else
            throw new Exception("The specified path was not found");
    }

    public static void EnsureFileIsWritable(string filePath)
    {
        if (File.Exists(filePath))
            new FileInfo(filePath).IsReadOnly = false;
    }
    
    public static string GetFilePath(string path, string filename, bool deleteIfExists)
    {
        string filePath = Path.Combine(path, filename);
        if (deleteIfExists && File.Exists(filePath))
            DeleteFile(filePath);

        return filePath;
    }

    public static string EnsureFolderExists(string folderPath)
    {
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        return folderPath;
    }

    public static string EnsureFolderExists(params string[] paths)
    {
        return EnsureFolderExists(Path.Combine(paths));
    }

    public static string GetTempPath(string filename)
    {
        return Path.Combine(Path.GetTempPath(), filename);
    }

    public static bool DeleteFolder(string folderPath)
    {
        DeleteFolderRecur(folderPath);
        return !Directory.Exists(folderPath);
    }
    
    public static void DeleteFile(string filePath)
    {
        if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
        {
            try
            {
                File.Delete(filePath);
            }
            catch { }
        }
    }

    public static void DeleteFile(params string[] paths)
    {
        DeleteFile(Path.Combine(paths));
    }

    public static string DeleteFolderFiles(string folderPath)
    {
        foreach (string filePath in Directory.GetFiles(folderPath))
        {
            try
            {
                File.Delete(filePath);
            }
            catch { }
        }

        return folderPath;
    }

    public static string DeleteFolderSubFolders(string folderPath)
    {
        foreach (string subFolderPath in Directory.GetDirectories(folderPath))
            DeleteFolder(subFolderPath);

        return folderPath;
    }

    public static string DeleteFolderContents(string folderPath)
    {
        if (Directory.Exists(folderPath))
        {
            foreach (string childFolderPath in Directory.GetDirectories(folderPath))
                DeleteFolderRecur(childFolderPath);

            DeleteFolderFiles(folderPath);
        }

        return folderPath;
    }

    public static void CopyFile(string sourcePath, string destinationPath)
    {
        try
        {
            File.Copy(sourcePath, destinationPath, true);
        }
        catch { }
    }

    public static void CopyFolder(string sourcePath, string destinationPath, bool recurse)
    {
        String[] listings;

        if (destinationPath[destinationPath.Length - 1] != Path.DirectorySeparatorChar)
            destinationPath += Path.DirectorySeparatorChar;
        if (!Directory.Exists(destinationPath)) Directory.CreateDirectory(destinationPath);
        listings = Directory.GetFileSystemEntries(sourcePath);

        foreach (string listing in listings)
        {
            if (recurse)
            {
                if (Directory.Exists(listing))
                    CopyFolder(listing, destinationPath + Path.GetFileName(listing), recurse);
                else
                    File.Copy(listing, destinationPath + Path.GetFileName(listing), true);
            }
            else if (!Directory.Exists(listing))
            {
                File.Copy(listing, destinationPath + Path.GetFileName(listing), true);
            }
        }
    }
    
    public static string GetUniqueFilePath(string folder, string filename)
    {
        string filePath = Path.Combine(folder, filename);
        string baseFilename = Path.GetFileNameWithoutExtension(filename);
        string extension = Path.GetExtension(filename);
        int count = 0;

        while (File.Exists(filePath))
        {
            count++;
            string newFilename = baseFilename + "(" + count + ")" + extension;
            filePath = Path.Combine(folder, newFilename);
        }

        return filePath;
    }

    private static void DeleteFolderRecur(string folderPath)
    {
        DeleteFolderContents(folderPath);

        try
        {
            Directory.Delete(folderPath, true);
        }
        catch { }
    }
}
