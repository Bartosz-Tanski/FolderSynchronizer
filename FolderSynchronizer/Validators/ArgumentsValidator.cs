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
        ValidateReplicaDirectoryArgument(args[1]);
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
    
    private void ValidateReplicaDirectoryArgument(string arg)
    {
        if (Directory.Exists(arg))
            return;
        
        try
        {
            Directory.CreateDirectory(arg);
            Directory.Delete(arg);
        }
        catch (Exception ex)
        {
            throw new IOException($"Replica directory: {arg} cannot be created.", ex);
        }
    }
    
    private void ValidateIntervalArgument(string arg)
    {
        if (!int.TryParse(arg, out _))
        {
            throw new FormatException($"<Interval> argument: {arg} is in wrong format.");
        }
    }
    
    private void ValidateLogFileArgument(string arg)
    {
        if (!File.Exists(arg))
        {
            throw new FileNotFoundException($"Log file: {arg} doesn't exist.");
        }

    }
}