namespace GenesysCloud.DTO.Response.Presence;

public sealed record PresenceDefinition
{
    public string SystemPresence { get; init; } = string.Empty;
    public bool IsPrimary { get; init; }
    public bool IsDeactivated { get; init; }
    public string Label { get; init; } = string.Empty;
}