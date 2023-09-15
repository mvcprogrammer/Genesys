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
    
    [Description("Success ? valid data : null/default")]
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
        
        Logger.LogSuccess(response.Id,$"{ClassHelpers.GetMethodName()}");
        return response;
    }
    
    [Description("Creates a Success = false ServiceResponse, data is default/null")]
    public static ServiceResponse<T> FailureResponse<T>(string errorMessage, int errorCode = Constants.Invalid, string query = "", int stackTraceIndex = 2)
    {
        var response = new ServiceResponse<T>
        {
            Success = false,
            ErrorMessage = errorMessage,
            ErrorCode = errorCode,
            Data = default
        };
        
        Logger.LogError(response.Id,$"{ClassHelpers.GetMethodName(stackTraceIndex)},{response.ErrorCode},{response.ErrorMessage}");
        
        if(query is not "")
            Logger.LogDebug(response.Id,$"{ClassHelpers.GetMethodName(stackTraceIndex)}\n{query}");
        
        return response;
    }
    
    [Description("Exception helper method to handle API exceptions and general exceptions")]
    public static class ExceptionHandler
    {
        public static ServiceResponse<T> HandleException<T>(Exception exception, string query = "")
        {
            return exception switch
            {
                ApiException apiException => FailureResponse<T>(
                    errorMessage: apiException.Message, 
                    errorCode: apiException.ErrorCode, 
                    query: query,
                    stackTraceIndex: 3),
                _ => FailureResponse<T>(
                    errorMessage: exception.Message,
                    errorCode: Constants.Invalid,
                    query: string.Empty,
                    stackTraceIndex: 3)
            };
        }
    }
}