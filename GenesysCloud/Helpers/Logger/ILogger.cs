namespace GenesysCloud.Helpers.Logger;

public interface ILogger
{
    public void LogSuccess(string successMessage);
    public void LogError(string errorMessage);
    public void LogDebug(string debugMessage);
}