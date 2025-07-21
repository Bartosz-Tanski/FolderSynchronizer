namespace FolderSynchronizer.Abstractions;

public interface IDirectoryMonitor
{
    void Monitor(string sourcePath, string replicaPath);
}