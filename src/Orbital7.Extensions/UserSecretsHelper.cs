using Microsoft.Extensions.Configuration.UserSecrets;
using Orbital7.Extensions.Encryption;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Orbital7.Extensions;

public static class UserSecretsHelper
{
    public static async Task<int> ExportForSolutionAsync(
        string solutionFilePath,
        string exportFilePath,
        string? password = null,
        CancellationToken cancellationToken = default)
    {
        var projectFilePaths = await ParseSolutionForProjectFilePathsAsync(
            solutionFilePath,
            cancellationToken: cancellationToken);

        var userSecretsIds = GatherUserSecretsIds(
            projectFilePaths);

        await CreateUserSecretsZipFileAsync(
            userSecretsIds, 
            exportFilePath,
            cancellationToken,
            password);

        return userSecretsIds.Count;
    }


    public static async Task<List<string>> ParseSolutionForProjectFilePathsAsync(
        string solutionFilePath,
        CancellationToken cancellationToken = default)
    {
        // NOTE: We used to use Microsoft.Build for this, but as of version 18.x,
        // the assembly used for this is private and the code below will error out.
        //
        //var solutionFile = SolutionFile.Parse(solutionFilePath);
        //var projectFilePaths = solutionFile.ProjectsInOrder
        //    .Where(x => x.ProjectType == SolutionProjectType.KnownToBeMSBuildFormat)
        //    .Select(x => x.AbsolutePath)
        //    .ToList();
        //
        // ...and thus, we now need to parse the solution file ourselves.


        var solutionFileExtension = Path.GetExtension(solutionFilePath);
        var solutionFolderPath = Path.GetDirectoryName(solutionFilePath) ?? string.Empty;

        // Common project extensions considered MSBuild projects.
        var projectFileExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".csproj", ".vbproj", ".fsproj", ".vcxproj", ".shproj"
        };

        // Handle by solution file extension which indicates the format of the solution file.
        if (solutionFileExtension == ".sln")
        {
            return await ParseSlnFileForProjectFilePathsAsync(
                solutionFilePath,
                solutionFolderPath,
                projectFileExtensions,
                cancellationToken);
        }
        else if (solutionFileExtension == ".slnx")
        {
            return ParseSlnxFileForProjectFilePaths(
                solutionFilePath,
                solutionFolderPath,
                projectFileExtensions);
        }
        else
        {
            throw new Exception($"Unrecognized solution file extension: {solutionFileExtension}");
        }
    }

    public static List<string> GatherUserSecretsIds(
        List<string> projectFilePaths)
    {
        var list = new List<string>();

        foreach (var projectFilePath in projectFilePaths)
        {
            var userSecretsId = TryGetUserSecretsId(projectFilePath);
            if (userSecretsId.HasText())
            {
                list.Add(userSecretsId);
            }
        }

        // Return the distinct list in case some secret files are shared.
        return list
            .Distinct()
            .ToList();
    }

    public static async Task<int> ImportAsync(
        string exportFilePath,
        string? password = null,
        CancellationToken cancellationToken = default)
    {
        using (var zipFile = ZipFile.OpenRead(exportFilePath))
        {
            foreach (var entry in zipFile.Entries)
            {
                var secretsFilePath = PathHelper.GetSecretsPathFromSecretsId(entry.Name);
                var secretsId = Path.GetFileNameWithoutExtension(secretsFilePath);

                if (secretsId.HasText())
                {
                    using (var entryStream = entry.Open())
                    {
                        var contents = await entryStream.ReadAllBytesAsync(
                            cancellationToken: cancellationToken);

                        if (password.HasText())
                        {
                            contents = EncryptionHelper.Decrypt(
                                contents,
                                password,
                                EncryptionMethod.TripleDES);
                        }

                        var secretsFolderPath = Path.GetDirectoryName(secretsFilePath);
                        if (secretsFolderPath.HasText())
                        {
                            FileSystemHelper.EnsureFolderExists(
                                secretsFolderPath);

                            await File.WriteAllBytesAsync(
                                secretsFilePath,
                                contents,
                                cancellationToken: cancellationToken);
                        }
                    }
                }
            }

            return zipFile.Entries.Count;
        }
    }

    private static async Task CreateUserSecretsZipFileAsync(
        List<string> userSecretsIds,
        string zipFilePath,
        CancellationToken cancellationToken,
        string? password = null)
    {
        if (File.Exists(zipFilePath))
        {
            File.Delete(zipFilePath);
        }

        using (var zipFile = ZipFile.Open(
            zipFilePath,
            ZipArchiveMode.Create))
        {
            foreach (var userSecretsId in userSecretsIds)
            {
                var secretsFilePath = PathHelper.GetSecretsPathFromSecretsId(userSecretsId);

                var contents = await File.ReadAllBytesAsync(
                    secretsFilePath,
                    cancellationToken: cancellationToken);

                if (password.HasText())
                {
                    contents = EncryptionHelper.Encrypt(
                        contents,
                        password,
                        EncryptionMethod.TripleDES);
                }

                var entry = zipFile.CreateEntry(userSecretsId);
                using (var entryStream = entry.Open())
                {
                    await entryStream.WriteAsync(
                        contents, 
                        0, 
                        contents.Length,
                        cancellationToken);
                }
            }
        }
    }

    private static string? TryGetUserSecretsId(
        string projectFilePath)
    {
        // Avoid Microsoft.Build APIs that depend on Visual Studio private assemblies.
        // Load the project XML and look for a <UserSecretsId> element in any namespace.
        try
        {
            if (!File.Exists(projectFilePath))
            {
                return null;
            }

            var doc = XDocument.Load(projectFilePath);

            // Find the first element whose local name equals "UserSecretsId".
            var userSecretsElement = doc
                .Descendants()
                .FirstOrDefault(x => string.Equals(
                    x.Name.LocalName, 
                    "UserSecretsId", 
                    StringComparison.OrdinalIgnoreCase));

            var value = userSecretsElement?.Value?.Trim();

            return string.IsNullOrEmpty(value) ? null : value;
        }
        catch
        {
            // Be defensive: if the project file is malformed or unreadable, return null.
            return null;
        }
    }

    private static async Task<List<string>> ParseSlnFileForProjectFilePathsAsync(
        string slnFilePath,
        string solutionFolderPath,
        HashSet<string> acceptedExtensions,
        CancellationToken cancellationToken)
    {
        var list = new List<string>();

        // Project("{...}") = "Name", "path\to\proj.csproj", "{...}"
        var projectLineRegex = new Regex(
            "^Project\\(\"(?<typeGuid>[^\"]+)\"\\)\\s*=\\s*\"(?<name>[^\"]+)\"\\s*,\\s*\"(?<path>[^\"]+)\"\\s*,\\s*\"(?<guid>[^\"]+)\"",
            RegexOptions.Compiled);

        var lines = await File.ReadAllLinesAsync(
            slnFilePath, 
            cancellationToken: cancellationToken);

        foreach (var line in lines)
        {
            var match = projectLineRegex.Match(line);
            if (!match.Success) continue;

            var relativePath = match.Groups["path"].Value
                .Replace('/', Path.DirectorySeparatorChar)
                .Replace('\\', Path.DirectorySeparatorChar);

            var resolvedPath = Path.IsPathRooted(relativePath)
                ? relativePath
                : Path.GetFullPath(Path.Combine(solutionFolderPath, relativePath));

            if (File.Exists(resolvedPath) && acceptedExtensions.Contains(Path.GetExtension(resolvedPath)))
            {
                list.Add(resolvedPath);
            }
        }

        return list;
    }

    private static List<string> ParseSlnxFileForProjectFilePaths(
        string slnxFilePath,
        string solutionFolderPath,
        HashSet<string> acceptedExtensions)
    {
        var list = new List<string>();

        var doc = XDocument.Load(slnxFilePath);
        var projectPaths = doc
          .Descendants()
          .Where(e => string.Equals(e.Name.LocalName, "Project", StringComparison.OrdinalIgnoreCase))
          .Select(e => (string?)e.Attribute("Path"))
          .Where(p => !string.IsNullOrEmpty(p))
          .ToList();

        foreach (var projectPath in projectPaths)
        {
            var normalizedProjectPath = projectPath?.Trim()
                .Replace('/', Path.DirectorySeparatorChar)
                .Replace('\\', Path.DirectorySeparatorChar);

            if (normalizedProjectPath.HasText())
            {
                var resolvedPath = Path.IsPathRooted(normalizedProjectPath) ?
                    normalizedProjectPath :
                    Path.GetFullPath(Path.Combine(solutionFolderPath, normalizedProjectPath));

                if (File.Exists(resolvedPath) && 
                    acceptedExtensions.Contains(Path.GetExtension(resolvedPath)))
                {
                    list.Add(resolvedPath);
                }
            }
        }

        return list;
    }
}
