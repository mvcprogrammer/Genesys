namespace GenesysCloud.DTO;

public static class GenesysResponse
{
    public static GenesysServiceResponse<T> SuccessResponse<T>(T data)
    {
        return new GenesysServiceResponse<T>
        {
            Success = true,
            MethodName = ClassHelpers.GetCallingMethodName(),
            Data = data
        };
    }
    
    public static GenesysServiceResponse<T> FailureResponse<T>(string errorMessage, int errorCode = Constants.Invalid, string query = "") where T : new()
    {
        return new GenesysServiceResponse<T>
        {
            Success = false,
            MethodName = ClassHelpers.GetCallingMethodName(),
            ErrorMessage = $"{ClassHelpers.GetCallingMethodName()}, {errorMessage}",
            ErrorCode = errorCode,
            Query = query,
            Data = new T()
        };
    }
}