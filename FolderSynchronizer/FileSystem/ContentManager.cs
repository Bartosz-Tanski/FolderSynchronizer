using FolderSynchronizer.Abstractions;
using Serilog;

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
        var firstPathContent = getContent(firstPath);
        var secondPathContent = getContent(secondPath);

        var firstPathRelativePath = firstPathContent
            .Select(fullPath => Path.GetRelativePath(firstPath, fullPath));

        var secondPathRelativePath = secondPathContent
            .Select(fullPath => Path.GetRelativePath(secondPath, fullPath));

        var missingContentRelativePath = firstPathRelativePath.Except(secondPathRelativePath);

        return firstPathContent
            .Where(fullPath => missingContentRelativePath.Contains(Path.GetRelativePath(firstPath, fullPath)))
            .ToArray();
    }

    private static string GetTargetPath(string sourcePath, string replicaPath, string content)
    {
        var relativePath = Path.GetRelativePath(sourcePath, content);
        var targetDirectoryPath = Path.Combine(replicaPath, relativePath);

        return targetDirectoryPath;
    }

    public void EqualizeFileCount(string sourcePath, string replicaPath)
    {
        if (GetAllFilesPaths(sourcePath).Length > GetAllFilesPaths(replicaPath).Length)
        {
            CopyMissingFiles(sourcePath, replicaPath);
            return;
        }
        
        RemoveFiles(GetMissingContent(replicaPath, sourcePath, GetAllFilesPaths));
    }

    private void CopyMissingFiles(string sourcePath, string replicaPath)
    {
        foreach (var file in GetMissingContent(sourcePath, replicaPath, GetAllFilesPaths))
        {
            var targetFilePath = GetTargetPath(sourcePath, replicaPath, file);

            Log.Information($"Copy: {file}, to: {targetFilePath}");
            File.Copy(file, targetFilePath);
        }
    }

    public void EqualizeDirectoryCount(string sourcePath, string replicaPath)
    {
        if (GetAllDirectoriesPaths(sourcePath).Length > GetAllDirectoriesPaths(replicaPath).Length)
        {
            CreateMissingDirectories(sourcePath, replicaPath);
            return;
        }

        RemoveDirectories(sourcePath, replicaPath);
    }

    private void CreateMissingDirectories(string sourcePath, string replicaPath)
    {
        foreach (var directory in GetMissingContent(sourcePath, replicaPath, GetAllDirectoriesPaths))
        {
            var targetDirectoryPath = GetTargetPath(sourcePath, replicaPath, directory);

            CreateDirectory(targetDirectoryPath);
        }
    }
    
    public void RemoveDirectories(string sourcePath, string replicaPath)
    {
        var missingDirectories = GetMissingContent(replicaPath, sourcePath, GetAllDirectoriesPaths)
            .OrderByDescending(dir => dir.Count(separator => separator == Path.DirectorySeparatorChar));
    
        foreach (var directory in missingDirectories)
        {
            RemoveFiles(GetAllFilesPaths(directory));
    
            if (Directory.Exists(directory))
            {
                Log.Information("Delete directory: " + directory);
                Directory.Delete(directory);
            }
        }
    }

    public void RemoveFiles(IEnumerable<string> filesToRemove)
    {
        foreach (var file in filesToRemove)
        {
            Log.Information("Delete file: " + file);
            File.Delete(file);
        }
    }

    public void CreateDirectory(string path)
    {
        Log.Information("Create: " + path);
        Directory.CreateDirectory(path);
    }
}