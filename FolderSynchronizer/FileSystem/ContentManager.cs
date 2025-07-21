using FolderSynchronizer.Abstractions;

namespace FolderSynchronizer.FileSystem;

public class ContentManager : IContentManager
{
    private readonly List<string> _allDirectories = [];
    
    public string[] GetAllDirectoriesPaths(string path)
    {
        var allDirectories = Directory.GetDirectories(path);
        
        foreach (var directory in allDirectories)
        {
            _allDirectories.Add(directory);
            
            GetAllDirectoriesPaths(directory);
        }

        return _allDirectories.ToArray();
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