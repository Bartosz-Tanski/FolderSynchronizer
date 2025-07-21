using FolderSynchronizer.Abstractions;

namespace FolderSynchronizer.FileSystem;

public class DirectoryContentContentManager : IDirectoryContentManager
{
    private readonly List<string> _allDirectories = [];
    
    public string[] GetAllDirectoriesPaths(string path)
    {
        var allDirectories = Directory.GetDirectories(path);
        
        foreach (var directory in allDirectories)
        {
            _allDirectories.Add(directory);
            
            // if (Directory.GetDirectories(directory).Length > 0)
            // {
                GetAllDirectoriesPaths(directory);
            // }
        }

        return _allDirectories.ToArray();
    }
    
    public string[] GetAllFilesPaths(string path)
    {
        var allDirectories = GetAllDirectoriesPaths(path);
        var allFiles = Directory.GetFiles(path).Select(file => file).ToList();
        
        allFiles.AddRange(allDirectories.SelectMany(Directory.GetFiles));

        return allFiles.ToArray();
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    public void CreateDirectories(string sourcePath, string targetPath)
    {
        var innerSourceDirectories = Directory.GetDirectories(sourcePath);

        foreach (var innerDirectory in innerSourceDirectories)
        {
            var combinedTargetPath = Path.Combine(targetPath, Path.GetFileName(innerDirectory));

            if (!Directory.Exists(combinedTargetPath))
            {
                // _userInterface.DisplayMessage($"Creating directory at: {combinedTargetPath}", ConsoleColor.DarkGray);
                Directory.CreateDirectory(combinedTargetPath);
            }

            var innerSourceFiles = Directory.GetFiles(innerDirectory);
            if (innerSourceFiles.Length > 0)
            {
                foreach (var innerSourceFile in innerSourceFiles)
                {
                    var combinedTargetPathForFile = Path.Combine(combinedTargetPath, Path.GetFileName(innerSourceFile));

                    if (!File.Exists(combinedTargetPathForFile))
                    {
                        // CopyFile(innerSourceFile, combinedTargetPathForFile);
                    }
                }
            }

            if (Directory.GetDirectories(innerDirectory).Length > 0)
            {
                CreateDirectories(innerDirectory, combinedTargetPath);
            }
        }
    }


}