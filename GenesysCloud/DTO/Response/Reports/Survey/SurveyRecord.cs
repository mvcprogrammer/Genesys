using UserProfile = GenesysCloud.DTO.Response.Users.UserProfile;

namespace GenesysCloud.DTO.Response.Reports.Survey;

public sealed record SurveyRecord
{
    public UserProfile? AgentProfile { get; init; }
    public string QueueName { get; init; } = string.Empty;
    public string MediaType { get; init; } = string.Empty;
    public string CompletedDate { get; init; } = string.Empty;
    public string InteractionLink { get; init; } = string.Empty;
    public string ExternalTag { get; init; } = string.Empty;
    public string InteractionStartTime { get; init; } = string.Empty;
    public SurveyForm Form { get; init; } = new();
}

public sealed record SurveyForm
{
    public string Name { get; init; } = string.Empty;
    public string Header { get; init; } = string.Empty;
    public string Footer { get; init; } = string.Empty;
    public bool Disabled { get; init; }
    public string Language { get; init; } = string.Empty;
    public string ModifiedDate { get; init; } = string.Empty;
    public bool Published { get; init; }
    public string TotalScore { get; init; } = string.Empty;
    public List<SurveyGroup> SurveyGroups = new();
}
public sealed record SurveyGroup
{
    public string Title { get; set; } = string.Empty;
    public List<SurveyScoringSet>? ScoringSets = new();
}

public sealed record SurveyScoringSet
{
    public string Question { get; init; } = string.Empty;
    public string SurveyAnswer { get; init; } = string.Empty;
    public string AssistedAnswer { get; init; } = string.Empty;
    public string FreeTextAnswer { get; init; } = string.Empty;
    public string NpsTextAnswer { get; init; } = string.Empty;
    public string Score { get; init; } = string.Empty;
    public string NpsScore { get; init; } = string.Empty;
}