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

        var sourceFilesNames = sourceFiles.Select(Path.GetFileName);
        var replicaFilesNames = replicaFiles.Select(Path.GetFileName);

        if (sourceFiles.Length > replicaFiles.Length)
        {
            var excessFilesInSource = sourceFilesNames.Except(replicaFilesNames);

            foreach (var file in excessFilesInSource)
            {
                Console.WriteLine("I should create file: " + file);   
            }

            return;
        }

        var excessFileInReplica = replicaFilesNames.Except(sourceFilesNames);

        foreach (var file in excessFileInReplica)
        {
            Console.WriteLine("I should delete: " + file);
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