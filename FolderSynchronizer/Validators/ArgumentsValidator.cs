using FolderSynchronizer.Abstractions;

namespace FolderSynchronizer.Validators;

public class ArgumentsValidator : IArgumentsValidator
{
    public void Validate(string[] args)
    {
        if (args.Length != 4)
        {
            throw new InvalidOperationException("Incorrect number of arguments. Please run program with 4 arguments");
        }
        
        ValidateSourceDirectoryArgument(args[0]);
        ValidateReplicaDirectoryArgument(args[1], args[0]);
        ValidateIntervalArgument(args[2]);
        ValidateLogFileArgument(args[3]);
    }
    
    private void ValidateSourceDirectoryArgument(string arg)
    {
        if (!Directory.Exists(arg))
        {
            throw new DirectoryNotFoundException($"Source directory: {arg} doesn't exist.");
        }
    }
    
    private void ValidateReplicaDirectoryArgument(string replicaDir, string sourceDir)
    {
        if (replicaDir == sourceDir)
            throw new IOException($"Replica directory: {replicaDir} cannot be the same as source directory.");
        
        if (Directory.Exists(replicaDir))
            return;
        
        try
        {
            Directory.CreateDirectory(replicaDir);
            Directory.Delete(replicaDir);
        }
        catch (Exception ex)
        {
            throw new IOException($"Replica directory: {replicaDir} cannot be created.", ex);
        }
    }
    
    private void ValidateIntervalArgument(string arg)
    {
        if (!uint.TryParse(arg, out _))
        {
            throw new FormatException($"<Interval> argument: {arg} must be a positive integer.");
        }
    }
    
    private void ValidateLogFileArgument(string arg)
    {
        if (!Directory.Exists(arg))
        {
            throw new FileNotFoundException($"Log dircetory: {arg} doesn't exist.");
        }
    }
}