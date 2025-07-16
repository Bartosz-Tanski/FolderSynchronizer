namespace FolderSynchronizer.Abstractions;

public interface IArgumentsValidator
{
    void Validate(string[] args);
}