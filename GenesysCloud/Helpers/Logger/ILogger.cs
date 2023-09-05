namespace GenesysCloud.Helpers.Logger;

public interface ILogger
{
    public void LogSuccess(Guid id, string successMessage);
    public void LogError(Guid id, string errorMessage);
    public void LogDebug(Guid id, string debugMessage);
}