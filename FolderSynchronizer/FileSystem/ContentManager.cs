using System.Threading.Channels;
using FolderSynchronizer.Abstractions;

namespace FolderSynchronizer.FileSystem;

public class ContentManager : IContentManager
{
    private string GetNameOnly(string path) => Path.GetFileName(path);
    
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
        var sourceFilesNames = GetAllFilesPaths(sourcePath).Select(GetNameOnly).ToArray();
        var replicaFilesNames = GetAllFilesPaths(replicaPath).Select(GetNameOnly).ToArray();

        if (sourceFilesNames.Length > replicaFilesNames.Length)
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