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
        string[] innerDirectories = Directory.GetDirectories(currentPath);
        
        foreach (var directory in innerDirectories)
        {
            outputList.Add(directory);
            GetDirectories(directory, outputList);
        }
    }

    public void CreateDirectories(string sourcePath, string targetPath)
    {
        throw new NotImplementedException();
    }

    public void RemoveContentSizeMismatch(string sourcePath, string replicaPath)
    {
        throw new NotImplementedException();
    }

    public void RemoveContentTimestampMismatch(string sourcePath, string replicaPath)
    {
        throw new NotImplementedException();
    }

    public void EqualizeContentCount(string sourcePath, string replicaPath)
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