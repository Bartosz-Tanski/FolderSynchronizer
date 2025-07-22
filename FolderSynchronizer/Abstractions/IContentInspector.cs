namespace FolderSynchronizer.Abstractions;

public interface IContentInspector
{
    bool DoesReplicaDirectoryExist(string replicaPath);
    bool HasSameDirectoryCount(string sourcePath, string replicaPath);
    bool HasSameFileCount(string sourcePath, string replicaPath);
    bool HasSameContentSizes(string sourcePath, string replicaPath, out List<string> notValidReplicaFiles);
    bool HasSameFilesNames(string sourcePath, string replicaPath, out List<string> notValidReplicaFiles);
    bool HasSameDirectoriesNames(string sourcePath, string replicaPath);
    bool IsContentIntegral(string sourcePath, string replicaPath, out List<string> notValidReplicaFiles);
}