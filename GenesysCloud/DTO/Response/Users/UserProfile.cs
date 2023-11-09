namespace GenesysCloud.DTO.Response.Users;

public sealed record UserProfile
{ 
    public string Email { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string DivisionId { get; init; } = string.Empty;
    public string DivisionName { get; init; } = string.Empty;
    public string Id => Email[..Email.IndexOf('@')];
}