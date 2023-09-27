using PureCloudPlatform.Client.V2.Client;
using System.Diagnostics;
using GenesysCloud.Helpers.Logger;
using GenesysCloud.Helpers.Logger.Console;

namespace GenesysCloud.DTO;

public static class ServiceResponse
{
    private static readonly ILogger Logger = new ConsoleLogger();
    
    public static T LogAndReturnResponse<T>(T data, int stackTraceIndex = 4)
    {
        Debug.Assert(data is not null);
        Logger.LogSuccess($"{ClassHelpers.GetMethodName(stackTraceIndex)}");
        return data;
    }
    
    public static T? LogFailure<T>(string errorMessage, int errorCode = Constants.Invalid, string query = "", int stackTraceIndex = 3)
    {
        Logger.LogError($"{ClassHelpers.GetMethodName(stackTraceIndex)},{errorCode},{errorMessage}");
        
        if(query is not "") 
            Logger.LogDebug($"{ClassHelpers.GetMethodName(stackTraceIndex)}\n{query}");
        
        return default;
    }
    
    public static class ExceptionHandler
    {
        public static T? HandleException<T>(Exception exception, string query = "")
        {
            return exception switch
            {
                ApiException apiException => LogFailure<T>(apiException.Message, apiException.ErrorCode, query, 3),
                _ => LogFailure<T>(exception.Message, stackTraceIndex: 3)
            };
        }
    }

}