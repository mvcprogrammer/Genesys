using UserProfile = GenesysCloud.DTO.Response.Users.UserProfile;

namespace GenesysCloud.DTO.Response.Reports.Evaluation;

public sealed record EvaluationRecord
{
    public MetricsInterval? IntervalSpan { get; init; }
    public UserProfile? AgentProfile { get; init; }
    public UserProfile? EvaluatorProfile { get; init; }
    public string? Status { get; init; } = string.Empty;
    public string EvaluationFormName { get; init; } = string.Empty;
    public DateTime? EvaluationFormModifiedDate { get; set; }
    public bool? EvaluationFormPublished { get; set; } = false;
    public TimeSpan? ConversationTimeSpan { get; set; }
    public string MediaType { get; init; } = string.Empty;
    public string AgentComments { get; init; } = string.Empty;
    public string EvaluatorComments { get; init; } = string.Empty;
    public string EvaluatorPrivateComments { get; set; } = string.Empty;
    public float TotalScore { get; init; }
    public float TotalCriticalScore { get; set; }
    public float TotalNonCriticalScore { get; set; }
    public bool AgentHasRead { get; set; } = false;
    public string SilenceDurationPercentage { get; set; } = string.Empty;
    
    public List<EvaluationGroup> EvaluationGroups = new();
}

public sealed record EvaluationGroup
{
    public string Title { get; set; } = string.Empty;
    public List<EvaluationScoringSet>? ScoringSets = new();
}

public sealed record EvaluationScoringSet
{
    public string Question { get; init; } = string.Empty;
    public string Answer { get; init; } = string.Empty;
    public string Comment { get; init; } = string.Empty;
    public int? Score { get; init; }
}