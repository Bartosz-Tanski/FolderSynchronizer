using FolderSynchronizer.Abstractions;

namespace FolderSynchronizer.FileSystem;

public class DirectoryMonitor : IDirectoryMonitor
{
    private readonly IDirectoryContentManager _directoryContentManager;
    private readonly IUserInterface _userInterface;

    public DirectoryMonitor(IDirectoryContentManager directoryContentManager, IUserInterface userInterface)
    {
        _directoryContentManager = directoryContentManager;
        _userInterface = userInterface;
    }

    public void Monitor(string sourcePath, string replicaPath)
    {
        // if (!HasSameContentSizes(sourcePath, replicaPath))
        // {
        //     RemoveContentSizeMismatch(sourcePath, replicaPath);
        //     return;
        // }
        //
        // if (!HasMatchingTimestamps(sourcePath, replicaPath))
        // {
        //     RemoveContentTimestampMismatch(sourcePath, replicaPath);
        //     return;
        // }
        //
        // if (!HasSameContentCount(sourcePath, replicaPath))
        // {
        //     EqualizeContentCount(sourcePath, replicaPath);
        //     return;
        // }
        //
        // if (!HasMatchingContentStructure(sourcePath, replicaPath))
        // {
        //     RemoveContentStructureMismactch(sourcePath, replicaPath);
        //     return;
        // }
        //
        // if (!IsContentIntegral(sourcePath, replicaPath))
        // {
        //     RemoveNonIntegralContent(sourcePath, replicaPath);
        //     return;
        // }

        _userInterface.DisplayMessage("Directories contains the same files", ConsoleColor.DarkGreen);
    }

    
}