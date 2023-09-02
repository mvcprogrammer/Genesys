using System.ComponentModel;

namespace GenesysCloud.DTO;

public record GenesysServiceResponse<T>
{
    [Description("When true, Data<T> CANNOT be null.")]
    public bool Success { get; init; } = true;
    
    public string ErrorMessage { get; init; } = string.Empty;
    
    [Description("Http Error Code")]
    public int ErrorCode { get; init; }

    public string MethodName { get; init; } = string.Empty;
    public string Query { get; set; } = string.Empty;
    
    public T? Data { get; init; }
}