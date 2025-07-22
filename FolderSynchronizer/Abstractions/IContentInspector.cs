namespace FolderSynchronizer.Abstractions;

public interface IContentInspector
{
    bool HasSameContentSizes(string sourcePath, string replicaPath, out List<string> notValidReplicaFiles);
    bool IsContentIntegral(string sourcePath, string replicaPath, out List<string> notValidReplicaFiles);
    bool HasSameFilesNames(string sourcePath, string replicaPath, out List<string> notValidReplicaFiles);
    bool HasSameFileCount(string sourcePath, string replicaPath);
    bool HasSameDirectoryCount(string sourcePath, string replicaPath);
    bool DoesReplicaDirectoryExist(string replicaPath);
}