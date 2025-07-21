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
        if (!int.TryParse(args[2], out _))
        {
            throw new FormatException($"<Interval> argument: {args[2]} is in wrong format.");
        }

        if (!Directory.Exists(args[0]))
        {
            throw new DirectoryNotFoundException($"Source directory: {args[0]} doesn't exist.");
        }
    }
}