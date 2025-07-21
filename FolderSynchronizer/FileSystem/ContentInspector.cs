using System.Security.Cryptography;
using System.Text;
using FolderSynchronizer.Abstractions;

namespace FolderSynchronizer.FileSystem;

public class ContentInspector : IContentInspector
{
    private readonly IContentManager _contentManager;

    public ContentInspector(IContentManager contentManager)
    {
        _contentManager = contentManager;
    }

    // public bool HasSameContentSizes(string sourcePath, string replicaPath)
    // {
    //     var allSourceFiles = _contentManager.GetAllFilesPaths(sourcePath);
    //     var allReplicaFiles = _contentManager.GetAllFilesPaths(replicaPath);
    //
    //     for (int i = 0; i < allSourceFiles.Length; i++)
    //     {
    //         var sourceFileInfo = new FileInfo(allSourceFiles[i]);
    //         var replicaFileInfo = new FileInfo(allReplicaFiles[i]);
    //
    //         if (sourceFileInfo.Length != replicaFileInfo.Length)
    //             return false;
    //     }
    //
    //     return true;
    // }
    //
    public bool HasSameFileCount(string sourcePath, string replicaPath)
    {
        var sourceFiles = _contentManager.GetAllFilesPaths(sourcePath);
        var replicaFiles = _contentManager.GetAllFilesPaths(replicaPath);
    
        return sourceFiles.Length == replicaFiles.Length;
    }
    
    public bool HasSameDirectoryCount(string sourcePath, string replicaPath)
    {
        var sourceDirectories = _contentManager.GetAllDirectoriesPaths(sourcePath);
        var replicaDirectories = _contentManager.GetAllDirectoriesPaths(replicaPath);
    
        return sourceDirectories.Length == replicaDirectories.Length;
    }
    //
    // public bool HasSameContentNames(string sourcePath, string replicaPath)
    // {
    //     throw new NotImplementedException();
    // }
    //
    //
    // public bool IsContentIntegral(string sourcePath, string replicaPath)
    // {
    //     var allSourceFiles = _contentManager.GetAllFilesPaths(sourcePath);
    //     var allReplicaFiles = _contentManager.GetAllFilesPaths(replicaPath);
    //
    //     for (int i = 0; i < allSourceFiles.Length; i++)
    //     {
    //         var sourceFileHash = ComputeMd5(allSourceFiles[i]);
    //         var replicaFileHash = ComputeMd5(allReplicaFiles[i]);
    //
    //         if (!string.Equals(sourceFileHash, replicaFileHash))
    //         {
    //             return false;
    //         }
    //     }
    //     
    //     return true;
    // }
    //
    // private string ComputeMd5(string filePath)
    // {
    //     using var md5 = MD5.Create();
    //     using var stream = File.OpenRead(filePath);
    //     
    //     var hash = md5.ComputeHash(stream);
    //     var stringBuilder = new StringBuilder(hash.Length * 2);
    //     
    //     foreach (var singleByte in hash)
    //         stringBuilder.Append(singleByte.ToString("x2"));
    //     
    //     return stringBuilder.ToString();
    // }


    public bool HasSameContentSizes(string sourcePath, string replicaPath, out List<string> notValidReplicaFiles)
    {
        var result = true;
        
        var allSourceFiles = _contentManager.GetAllFilesPaths(sourcePath);
        var allReplicaFiles = _contentManager.GetAllFilesPaths(replicaPath);

        notValidReplicaFiles = [];

        for (int i = 0; i < allSourceFiles.Length; i++)
        {
            var sourceFileInfo = new FileInfo(allSourceFiles[i]);
            var replicaFileInfo = new FileInfo(allReplicaFiles[i]);

            if (sourceFileInfo.Length != replicaFileInfo.Length)
            {
                notValidReplicaFiles.Add(allReplicaFiles[i]);
                result = false;
            }
        }

        return result;
    }

    public bool IsContentIntegral(string sourcePath, string replicaPath, out List<string> notValidReplicaFiles)
    {
        var result = true;
        
        var allSourceFiles = _contentManager.GetAllFilesPaths(sourcePath);
        var allReplicaFiles = _contentManager.GetAllFilesPaths(replicaPath);
        
        notValidReplicaFiles = [];
        
        for (int i = 0; i < allSourceFiles.Length; i++)
        {
            var sourceFileHash = ComputeMd5(allSourceFiles[i]);
            var replicaFileHash = ComputeMd5(allReplicaFiles[i]);
        
            if (!string.Equals(sourceFileHash, replicaFileHash))
            {
                notValidReplicaFiles.Add(allReplicaFiles[i]);
                result = false;
            }
        }
        
        return result;
    }
    
    private string ComputeMd5(string filePath)
    {
        using var md5 = MD5.Create();
        using var stream = File.OpenRead(filePath);
        
        var hash = md5.ComputeHash(stream);
        var stringBuilder = new StringBuilder(hash.Length * 2);
        
        foreach (var singleByte in hash)
            stringBuilder.Append(singleByte.ToString("x2"));
        
        return stringBuilder.ToString();
    }

    public bool HasSameContentNames(string sourcePath, string replicaPath, out List<string> notValidReplicaFiles)
    {
        var result = true;
        
        var allSourceFiles = _contentManager.GetAllFilesPaths(sourcePath);
        var allReplicaFiles = _contentManager.GetAllFilesPaths(replicaPath);

        notValidReplicaFiles = [];

        for (int i = 0; i < allSourceFiles.Length; i++)
        {
            var sourceFileInfo = new FileInfo(allSourceFiles[i]);
            var replicaFileInfo = new FileInfo(allReplicaFiles[i]);

            if (sourceFileInfo.Name.Length != replicaFileInfo.Name.Length)
            {
                notValidReplicaFiles.Add(allReplicaFiles[i]);
                result = false;
            }
        }

        return result;
    }
}