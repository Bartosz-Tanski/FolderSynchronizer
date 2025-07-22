using FolderSynchronizer.Abstractions;

namespace FolderSynchronizer.FileSystem;

public class ContentManager : IContentManager
{
    public string[] GetAllFilesPaths(string path)
    {
        var allDirectories = GetAllDirectoriesPaths(path);
        var allFiles = Directory.GetFiles(path).ToList();

        allFiles.AddRange(allDirectories.SelectMany(Directory.GetFiles));

        return allFiles.ToArray();
    }

    public string[] GetAllDirectoriesPaths(string path)
    {
        var result = new List<string>();

        GetDirectories(path, result);

        return result.ToArray();
    }

    private void GetDirectories(string currentPath, List<string> outputList)
    {
        var innerDirectories = Directory.GetDirectories(currentPath);

        foreach (var directory in innerDirectories)
        {
            outputList.Add(directory);
            GetDirectories(directory, outputList);
        }
    }

    private string[] GetMissingContent(string firstPath, string secondPath, Func<string, string[]> getContent)
    {
        var firstPathContentNames = getContent(firstPath).Select(Path.GetFileName);
        var secondPathContentNames = getContent(secondPath).Select(Path.GetFileName);

        var missingContentNames = firstPathContentNames.Except(secondPathContentNames);

        return getContent(firstPath).Where(file => missingContentNames.Contains(Path.GetFileName(file))).ToArray();
    }

    private static string GetTargetPath(string sourcePath, string replicaPath, string content)
    {
        var relativePath = Path.GetRelativePath(sourcePath, content!);
        var targetDirectoryPath = Path.Combine(replicaPath, relativePath);

        Console.WriteLine($"Create: {targetDirectoryPath}");

        return targetDirectoryPath;
    }

    public void EqualizeFileCount(string sourcePath, string replicaPath)
    {
        if (GetAllFilesPaths(sourcePath).Length > GetAllFilesPaths(replicaPath).Length)
        {
            CopyMissingFiles(sourcePath, replicaPath);
            return;
        }

        RemoveExcessFiles(sourcePath, replicaPath);
    }

    private void CopyMissingFiles(string sourcePath, string replicaPath)
    {
        foreach (var file in GetMissingContent(sourcePath, replicaPath, GetAllFilesPaths))
        {
            var targetFilePath = GetTargetPath(sourcePath, replicaPath, file!);

            File.Copy(file!, targetFilePath);
        }
    }

    private void RemoveExcessFiles(string sourcePath, string replicaPath)
    {
        foreach (var file in GetMissingContent(replicaPath, sourcePath, GetAllFilesPaths))
        {
            Console.WriteLine("Delete: " + file!); // TODO: Add real logging
            File.Delete(file!);
        }
    }

    public void EqualizeDirectoryCount(string sourcePath, string replicaPath)
    {
        if (GetAllDirectoriesPaths(sourcePath).Length > GetAllDirectoriesPaths(replicaPath).Length)
        {
            CreateMissingDirectories(sourcePath, replicaPath);
            return;
        }

        RemoveExcessDirectories(sourcePath, replicaPath);
    }

    private void CreateMissingDirectories(string sourcePath, string replicaPath)
    {
        foreach (var directory in GetMissingContent(sourcePath, replicaPath, GetAllDirectoriesPaths))
        {
            var targetDirectoryPath = GetTargetPath(sourcePath, replicaPath, directory!);

            Directory.CreateDirectory(targetDirectoryPath);
        }
    }

    private void RemoveExcessDirectories(string sourcePath, string replicaPath)
    {
        var missingDirectories = GetMissingContent(replicaPath, sourcePath, GetAllDirectoriesPaths)
            .OrderByDescending(dir => dir.Count(separator => separator == Path.DirectorySeparatorChar));

        foreach (var directory in missingDirectories)
        {
            RemoveFiles(GetAllFilesPaths(directory));

            if (Directory.Exists(directory))
            {
                Console.WriteLine("Remove directory: " + directory);
                Directory.Delete(directory);
            }
        }
    }

    public void RemoveFiles(IEnumerable<string> filesToRemove)
    {
        foreach (var file in filesToRemove)
        {
            Console.WriteLine("Remove file: " + file);
            File.Delete(file);
        }
    }
}