# Folder Synchronizer

## Description

Folder Synchronizer is a console application designed to keep two directories in sync. It monitors a source directory and replicates any changes such as creating, updating, or deleting files to a replica directory at specified time intervals. Additionally, it logs synchronization activities.

## Prerequisites
* Read/write permissions on both source and replica directories

## Usage

```bash
FolderSynchronizer.exe <SourceDir> <ReplicaDir> <Interval> <LogPath>
```

### Arguments

| Argument       | Description                                                                     |
| -------------- | ------------------------------------------------------------------------------- |
| `<SourceDir>`  | Path to the source directory whose contents you want to replicate.              |
| `<ReplicaDir>` | Path to the replica directory. All files and subfolders will mirror the source. |
| `<Interval>`   | Synchronization interval in seconds. Must be a positive integer (e.g. `30`).    |
| `<LogPath>`    | Path to the directory where synchronization logs will be stored.                |

## Examples

**Basic synchronization every 60 seconds:**

   ```bash
   FolderSynchronizer.exe C:\Projects\Source C:\Projects\Replica 60 C:\Logs
   ```

## Logging

* Logs are written as plain text files in the specified `<LogPath>` directory.
* Each run appends to the log file named with the current date (e.g., `sync-20250723.log`).
* Log entries include timestamps, operation types (create, update, delete), and file paths.
* The folder retains only the most recent 30 log files to reduce disk usage. Older log files are automatically deleted.
* If a daily log file exceeds a certain size, it is split into multiple files (e.g. `sync-20250723_001.log`, `sync-20250723_002.log`).

## Error Handling

* If the application encounters an error, it logs the exception details and exits with a non-zero error code.
* Invalid arguments (missing or malformed) will display the usage instructions and exit.
