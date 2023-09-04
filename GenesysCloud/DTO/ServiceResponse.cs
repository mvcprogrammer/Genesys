using PureCloudPlatform.Client.V2.Client;
using System.ComponentModel;

namespace GenesysCloud.DTO;

public record GenesysServiceResponse<T>
{
    public Guid Id { get; } = new();
    
    [Description("When true, Data<T> CANNOT be null.")]
    public bool Success { get; init; } = true;
    
    [Description("Exception.Message")]
    public string ErrorMessage { get; init; } = string.Empty;
    
    [Description("Http Error Code. -1 = Not an API exception.")]
    public int ErrorCode { get; init; }

    [Description("Used for logging which method succeeded for failed.")]
    public string MethodName { get; init; } = string.Empty;
    
    [Description("If API exception, the query used when exception was thrown.")]
    public string Query { get; set; } = string.Empty;
    
    [Description("Success ? valid data : null")]
    public T? Data { get; init; }
}

public static class GenesysResponse
{
    [Description("Creates a successful GenesysServiceResponse with valid data")]
    public static GenesysServiceResponse<T> SuccessResponse<T>(T data)
    {
        return new GenesysServiceResponse<T>
        {
            Success = true,
            MethodName = ClassHelpers.GetCallingMethodName(),
            Data = data
        };
    }
    
    [Description("Creates a Success = false GenesysServiceResponse and data is default/null")]
    public static GenesysServiceResponse<T> FailureResponse<T>(string errorMessage, int errorCode = Constants.Invalid, string query = "")
    {
        return new GenesysServiceResponse<T>
        {
            Success = false,
            MethodName = ClassHelpers.GetCallingMethodName(),
            ErrorMessage = $"{ClassHelpers.GetCallingMethodName()}, {errorMessage}",
            ErrorCode = errorCode,
            Query = query,
            Data = default
        };
    }
    
    [Description("DRY exception helper method to handle API exceptions AND general exceptions")]
    public static class ExceptionHandler
    {
        public static GenesysServiceResponse<T> HandleException<T>(Exception exception, string query = "")
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