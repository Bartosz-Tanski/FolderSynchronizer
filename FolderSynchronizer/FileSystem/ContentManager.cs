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

        RemoveExcessDirectories(sourcePath, replicaPath); // TODO: Implement removing directories
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
        throw new NotImplementedException();
        var sourceDirectories = GetAllDirectoriesPaths(sourcePath);
        var replicaDirectories = GetAllDirectoriesPaths(replicaPath);
        
        var sourceDirectoriesNames = sourceDirectories.Select(Path.GetFileName);
        var replicaDirectoriesNames = replicaDirectories.Select(Path.GetFileName);
        
        var excessDirectoriesNamesInReplica = replicaDirectoriesNames.Except(sourceDirectoriesNames);
        
        var directoriesToRemove = replicaDirectories
            .Where(file => excessDirectoriesNamesInReplica.Contains(Path.GetFileName(file))).ToArray();
        
        for (int i = directoriesToRemove.Length - 1; i >= 0; i--)
        {
            Console.WriteLine("Deleting: " + directoriesToRemove[i]);  // TODO: Add logging
            ClearDirectory(directoriesToRemove[i]);
            Directory.Delete(directoriesToRemove[i]);
        }
    }
    private void ClearDirectory(string path)
    {
        var allFiles = GetAllFilesPaths(path);
    
        foreach (var file in allFiles)
        {
            Console.WriteLine("Remove: " + file); // TODO: Add logging
            File.Delete(file);
        }
    }
    
    public void RemoveFiles(IEnumerable<string> notValidReplicaFiles)
    {
        foreach (var file in notValidReplicaFiles)
        {
            Console.WriteLine("Removing file: " + file);  // TODO: Add logging
            // File.Delete(file);
        }
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
    
    private string[] GetMissingContent (string firstPath, string secondPath, Func<string, string[]> getContent)
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
}






// public void EqualizeDirectoryCount(string sourcePath, string replicaPath)
// {
//     // TODO: Fix code repetition 
//
//     var sourceDirectories = GetAllDirectoriesPaths(sourcePath);
//     var replicaDirectories = GetAllDirectoriesPaths(replicaPath);
//
//     var sourceDirectoriesNames = sourceDirectories.Select(Path.GetFileName);
//     var replicaDirectoriesNames = replicaDirectories.Select(Path.GetFileName);
//
//     if (sourceDirectories.Length > replicaDirectories.Length)
//     {
//         var missingDirectoriesInReplica = sourceDirectoriesNames.Except(replicaDirectoriesNames);
//
//         var directoriesToCreate = sourceDirectories
//             .Where(dir => missingDirectoriesInReplica.Contains(Path.GetFileName(dir)));
//
//         foreach (var directoryToCreate in directoriesToCreate)
//         {
//             var relativePath = Path.GetRelativePath(sourcePath, directoryToCreate);
//             var targetDirectoryPath = Path.Combine(replicaPath, relativePath);
//
//             Console.WriteLine($"Creating directory: {targetDirectoryPath}"); // TODO: Add real logging
//             Directory.CreateDirectory(targetDirectoryPath);
//         }
//
//         return;
//     }
//
//     var excessDirectoriesNamesInReplica = replicaDirectoriesNames.Except(sourceDirectoriesNames);
//
//     var directoriesToRemove = replicaDirectories
//         .Where(file => excessDirectoriesNamesInReplica.Contains(Path.GetFileName(file))).ToArray();
//
//     for (int i = directoriesToRemove.Length - 1; i >= 0; i--)
//     {
//         Console.WriteLine("Deleting: " + directoriesToRemove[i]);  // TODO: Add logging
//         RemoveAllFiles(directoriesToRemove[i]);
//         Directory.Delete(directoriesToRemove[i]);
//     }
// }