using System.Security.Cryptography;
using System.Text;
using FolderSynchronizer.Abstractions;

namespace FolderSynchronizer.FileSystem;

public class ContentInspector : IContentInspector
{
    public bool DoesReplicaDirectoryExist(string replicaPath)
    {
        return Directory.Exists(replicaPath);
    }
    
    public bool HasSameCount(string[] sourceContent, string[] replicaContent)
    {
        return sourceContent.Length == replicaContent.Length;
    }

    public bool HasSameFileSizes(string[] sourceFiles, string[] replicaFiles, out List<string> notValidReplicaFiles)
    {
        var result = true;

        notValidReplicaFiles = [];

        for (int i = 0; i < sourceFiles.Length; i++)
        {
            var sourceFileInfo = new FileInfo(sourceFiles[i]);
            var replicaFileInfo = new FileInfo(replicaFiles[i]);

            if (sourceFileInfo.Length != replicaFileInfo.Length)
            {
                notValidReplicaFiles.Add(replicaFiles[i]);
                result = false;
            }
        }

        return result;
    }
    
    public bool HasSameNames(string[] sourceContent, string[] replicaContent, out List<string> notValidReplicaFiles)
    {
        notValidReplicaFiles = [];

        var result = true;
        
        for (int i = 0; i < sourceContent.Length; i++)
        {
            if (sourceContent.Length != replicaContent.Length)
                continue;
            
            var sourceName = Path.GetFileName(sourceContent[i]);
            var replicaName = Path.GetFileName(replicaContent[i]);

            if (sourceName.Length != replicaName.Length)
            {
                notValidReplicaFiles.Add(replicaContent[i]);
                result = false;
            }
        }

        return result;
    }
    
    public bool IsContentIntegral(string[] sourceFiles, string[] replicaFiles, out List<string> notValidReplicaFiles)
    {
        var result = true;
        
        notValidReplicaFiles = [];
        
        for (int i = 0; i < sourceFiles.Length; i++)
        {
            if (sourceFiles.Length != replicaFiles.Length)
                continue;
            
            var sourceFileHash = ComputeMd5(sourceFiles[i]);
            var replicaFileHash = ComputeMd5(replicaFiles[i]);
        
            if (!string.Equals(sourceFileHash, replicaFileHash))
            {
                notValidReplicaFiles.Add(replicaFiles[i]);
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

}