using PureCloudPlatform.Client.V2.Client;
using System.ComponentModel;
using GenesysCloud.Helpers.Logger;
using GenesysCloud.Helpers.Logger.Console;

namespace GenesysCloud.DTO;

public sealed record ServiceResponse<T>
{
    public Guid Id { get; } = Guid.NewGuid();
    
    [Description("When true, Data<T> CANNOT be null.")]
    public bool Success { get; init; } = true;
    
    [Description("Exception.Message")]
    public string ErrorMessage { get; init; } = string.Empty;
    
    [Description("Http Error Code. -1 = Not an API exception.")]
    public int ErrorCode { get; init; }
    
    [Description("Success ? valid data : null")]
    public T? Data { get; init; }
}

public static class SystemResponse
{
    private static readonly ILogger Logger = new ConsoleLogger();
    
    [Description("Creates a successful ServiceResponse with valid data")]
    public static ServiceResponse<T> SuccessResponse<T>(T data)
    {
        var response = new ServiceResponse<T>
        {
            Success = true,
            Data = data
        };
        
        Logger.LogSuccess($"{response.Id},{ClassHelpers.GetCallingMethodName()}");
        return response;
    }
    
    [Description("Creates a Success = false ServiceResponse, data is default/null")]
    private static ServiceResponse<T> FailureResponse<T>(string errorMessage, int errorCode = Constants.Invalid, string query = "")
    {
        var response = new ServiceResponse<T>
        {
            Success = false,
            ErrorMessage = errorMessage,
            ErrorCode = errorCode,
            Data = default
        };

        var methodName = ClassHelpers.GetCallingMethodName();
        Logger.LogError($"{response.Id},{methodName},{response.ErrorCode},{response.ErrorMessage}");
        Logger.LogDebug($"{response.Id},{methodName}\n{query}");
        
        return response;
    }
    
    [Description("DRY exception helper method to handle API exceptions AND general exceptions")]
    public static class ExceptionHandler
    {
        public static ServiceResponse<T> HandleException<T>(Exception exception, string query = "")
        {
            return exception switch
            {
                ApiException apiException => FailureResponse<T>(
                    apiException.Message, 
                    apiException.ErrorCode, 
                    query),
                _ => FailureResponse<T>(exception.Message)
            };
        }
    }
}