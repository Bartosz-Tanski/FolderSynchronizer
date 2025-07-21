using FolderSynchronizer.Abstractions;

namespace FolderSynchronizer.FileSystem;

public class ContentManager : IContentManager
{
    public string[] GetAllFilesPaths(string path)
    {
        var allDirectories = GetAllDirectoriesPaths(path);
        var allFiles = Directory.GetFiles(path).Select(file => file).ToList();

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
        // TODO: Fix code repetition 
        
        var sourceFiles = GetAllFilesPaths(sourcePath);
        var replicaFiles = GetAllFilesPaths(replicaPath);

        var sourceFileNames = sourceFiles.Select(Path.GetFileName).ToList();
        var replicaFileNames = replicaFiles.Select(Path.GetFileName).ToList();

        if (sourceFiles.Length > replicaFiles.Length)
        {
            var missingFileNamesInReplica = sourceFileNames.Except(replicaFileNames);

            var filesToCopy = sourceFiles
                .Where(file => missingFileNamesInReplica.Contains(Path.GetFileName(file)));

            foreach (var fileToCopy in filesToCopy)
            {
                var relativePath = Path.GetRelativePath(sourcePath, fileToCopy);

                var targetFilePath = Path.Combine(replicaPath, relativePath);

                Console.WriteLine($"Copy: {fileToCopy} file to: {targetFilePath}"); // TODO: Add real logging
                File.Copy(fileToCopy, targetFilePath);
            }

            return;
        }

        var excessFileNamesInReplica = replicaFileNames.Except(sourceFileNames);

        var replicaFilesToDeletePaths = replicaFiles
            .Where(file => excessFileNamesInReplica.Contains(Path.GetFileName(file)));

        foreach (var filesToDelete in replicaFilesToDeletePaths)
        {
            Console.WriteLine("Deleting: " + filesToDelete); // TODO: Add real logging
            File.Delete(filesToDelete);
        }
    }

    public void EqualizeDirectoryCount(string sourcePath, string replicaPath)
    {
        // TODO: Fix code repetition 

        var sourceDirectories = GetAllDirectoriesPaths(sourcePath);
        var replicaDirectories = GetAllDirectoriesPaths(replicaPath);

        var sourceDirectoriesNames = sourceDirectories.Select(Path.GetFileName);
        var replicaDirectoriesNames = replicaDirectories.Select(Path.GetFileName);

        if (sourceDirectories.Length > replicaDirectories.Length)
        {
            var missingDirectoriesInReplica = sourceDirectoriesNames.Except(replicaDirectoriesNames);

            var directoriesToCreate = sourceDirectories
                .Where(dir => missingDirectoriesInReplica.Contains(Path.GetFileName(dir)));

            foreach (var directoryToCreate in directoriesToCreate)
            {
                var relativePath = Path.GetRelativePath(sourcePath, directoryToCreate);

                var targetDirectoryPath = Path.Combine(replicaPath, relativePath);

                Console.WriteLine($"Creating directory: {targetDirectoryPath}"); // TODO: Add real logging
                Directory.CreateDirectory(targetDirectoryPath);
            }

            return;
        }

        var excessDirectoriesNamesInReplica = replicaDirectoriesNames.Except(sourceDirectoriesNames);

        var directoriesToRemove = replicaDirectories
            .Where(file => excessDirectoriesNamesInReplica.Contains(Path.GetFileName(file))).ToArray();

        for (int i = directoriesToRemove.Length - 1; i >= 0; i--)
        {
            Console.WriteLine("Deleting: " + directoriesToRemove[i]);
            RemoveAllFiles(directoriesToRemove[i]);
            Directory.Delete(directoriesToRemove[i]);
        }
    }

    public void RemoveFiles(List<string> notValidReplicaFiles)
    {
        foreach (var file in notValidReplicaFiles)
        {
            Console.WriteLine("Removing file: " + file);
            File.Delete(file);
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
    
    private void RemoveAllFiles(string path)
    {
        var allFiles = GetAllFilesPaths(path);

        foreach (var file in allFiles)
        {
            Console.WriteLine("Remove: " + file);
            File.Delete(file);
        }
    }
}