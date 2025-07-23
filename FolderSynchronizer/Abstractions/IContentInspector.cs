namespace FolderSynchronizer.Abstractions;

public interface IContentInspector
{
    bool DoesReplicaDirectoryExist(string replicaPath);
    bool HasSameCount(string[] sourceContent, string[] replicaContent);
    bool HasSameFileSizes(string[] sourceFiles, string[] replicaFiles, out List<string> notValidReplicaFiles);
    bool HasSameNames(string[] sourceContent, string[] replicaContent, out List<string> notValidReplicaFiles);
    bool IsContentIntegral(string[] sourceFiles, string[] replicaFiles, out List<string> notValidReplicaFiles);
}