using System.Threading.Channels;
using FolderSynchronizer.Abstractions;

namespace FolderSynchronizer.FileSystem;

public class ContentManager : IContentManager
{
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

    public void RemoveContentSizeMismatch(string sourcePath, string replicaPath)
    {
        throw new NotImplementedException();
    }

    public void RemoveContentTimestampMismatch(string sourcePath, string replicaPath)
    {
        throw new NotImplementedException();
    }

    public void EqualizeFileCount(string sourcePath, string replicaPath)
    {
        var sourceFiles = GetAllFilesPaths(sourcePath);
        var replicaFiles = GetAllFilesPaths(replicaPath);

        var sourceFileNames = sourceFiles.Select(Path.GetFileName).ToList();
        var replicaFileNames = replicaFiles.Select(Path.GetFileName).ToList();

        if (sourceFiles.Length > replicaFiles.Length)
        {
            var missingFileNamesInReplica = sourceFileNames.Except(replicaFileNames);

            var filesToCopy = sourceFiles
                .Where(file => missingFileNamesInReplica.Contains(Path.GetFileName(file)));
        
            foreach (var sourceFilePath in filesToCopy)
            {
                var relativePath = Path.GetRelativePath(sourcePath, sourceFilePath);
        
                var targetFilePath = Path.Combine(replicaPath, relativePath);

                Console.WriteLine($"Copy: {sourceFilePath} file to: {targetFilePath}"); // TODO: Add real logging
                File.Copy(sourceFilePath, targetFilePath);
            }
        }

        var excessFileNamesInReplica = replicaFileNames.Except(sourceFileNames);

        var replicaFilesToDeletePaths = replicaFiles
            .Where(file => excessFileNamesInReplica.Contains(Path.GetFileName(file)));
        
        foreach (var replicaFilePath in replicaFilesToDeletePaths)
        {
            Console.WriteLine("Deleting: " + replicaFilePath); // TODO: Add real logging
            File.Delete(replicaFilePath);
        }
    }

    public void EqualizeDirectoryCount(string sourcePath, string replicaPath)
    {
        throw new NotImplementedException();
    }

    public void RemoveContentStructureMismactch(string sourcePath, string replicaPath)
    {
        throw new NotImplementedException();
    }

    public void RemoveNonIntegralContent(string sourcePath, string replicaPath)
    {
        throw new NotImplementedException();
    }

    public string[] GetAllFilesPaths(string path)
    {
        var allDirectories = GetAllDirectoriesPaths(path);
        var allFiles = Directory.GetFiles(path).Select(file => file).ToList();

        allFiles.AddRange(allDirectories.SelectMany(Directory.GetFiles));

        return allFiles.ToArray();
    }
}