namespace GenesysCloud.DTO.Response;

public static class GenesysResponse
{
    public static GenesysServiceResponse<T> SuccessResponse<T>(T data)
    {
        return new GenesysServiceResponse<T>
        {
            Success = true,
            Data = data
        };
    }
    
    public static GenesysServiceResponse<T> FailureResponse<T>(string errorMessage, int errorCode = Constants.Invalid) where T : new()
    {
        return new GenesysServiceResponse<T>
        {
            Success = false,
            ErrorMessage = errorMessage,
            ErrorCode = errorCode,
            Data = new T()
        };
    }
}