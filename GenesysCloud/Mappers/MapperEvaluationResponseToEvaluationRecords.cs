using GenesysCloud.DTO.Response.Reports;

namespace GenesysCloud.Mappers;

public sealed class MapperEvaluationResponseToEvaluationRecords
{
    private readonly MetricsInterval _interval;
    private readonly string _conversationId;
    private readonly string _evaluationId;
    private readonly EvaluationResponse _evaluationData;
    public MapperEvaluationResponseToEvaluationRecords(MetricsInterval interval, string conversationId, string evaluationId, EvaluationResponse evaluationData)
    {
        _interval = interval;
        _conversationId = conversationId;
        _evaluationId = evaluationId;
        _evaluationData = evaluationData;
    }
    public ServiceResponse<EvaluationRecord> Map()
    {
        TimeSpan? conversationTimeSpan;

        if (_evaluationData.ConversationDate is null || _evaluationData.ConversationEndDate is null)
            conversationTimeSpan = new TimeSpan(0);
        else
            conversationTimeSpan = _evaluationData.ConversationEndDate - _evaluationData.ConversationDate;

        var dictionaryQuestionIdToQuestion = _evaluationData.EvaluationForm.QuestionGroups
            .SelectMany(questionGroup => questionGroup.Questions.ToDictionary(x => x.Id, x => x))
            .ToDictionary(x => x.Key, x => x.Value.Text);

        var dictionaryAnswerOptionIdToText = _evaluationData.EvaluationForm.QuestionGroups
            .SelectMany(x => x.Questions
                .SelectMany(y => y.AnswerOptions))
            .ToDictionary(y => y.Id, y => y.Text);

        var dictionaryQuestionGroups = _evaluationData.EvaluationForm.QuestionGroups
            .ToDictionary(x => x.Id, x => x.Name);

        var answerGroups = _evaluationData.Answers.QuestionGroupScores
            .Select(answerGroup => answerGroup.QuestionScores.ToDictionary(x => x.QuestionId, x => x))
            .ToArray();

        var evaluationGroups = new List<EvaluationGroup>();
        var curGroup = 0;

        foreach (var questionGroup in dictionaryQuestionGroups)
        {
            var evaluationGroup = new EvaluationGroup
            {
                Title = questionGroup.Value
            };

            var answerGroup = answerGroups[curGroup++];

            foreach (var answer in answerGroup.Where(answer => answer.Value.MarkedNA is not true))
            {
                if (dictionaryQuestionIdToQuestion.TryGetValue(answer.Value.QuestionId, out var evaluationQuestion) is false)
                    return SystemResponse.FailureResponse<EvaluationRecord>($"Failed to find evaluation question");

                if (dictionaryAnswerOptionIdToText.TryGetValue(answer.Value.AnswerId, out var evaluationAnswer) is false)
                    return SystemResponse.FailureResponse<EvaluationRecord>($"Failed to find evaluation answer");
                
                var scoringSet = new GenesysCloud.DTO.Response.Reports.EvaluationScoringSet
                {
                    Question = evaluationQuestion ?? string.Empty,
                    Answer = evaluationAnswer ?? string.Empty ,
                    Comment = answer.Value.Comments ?? string.Empty,
                    Score = answer.Value.Score
                };
                
                evaluationGroup.ScoringSets?.Add(scoringSet);
            }
            
            evaluationGroups.Add(evaluationGroup);
        }
        
        var evaluationRecord = new EvaluationRecord
        {
            IntervalSpan = _interval,
            Status = _evaluationData.Status?.ToString(),
            AgentHasRead = _evaluationData.AgentHasRead ?? false,
            /*AgentProfile = new GenesysCloud.DTO.Response.Users.UserProfile
            {
                Email = evaluationData.Agent.Email,
                Name = evaluationData.Agent.Name
            },*/
            EvaluationFormName = _evaluationData.EvaluationForm.Name,
            EvaluationFormModifiedDate = _evaluationData.EvaluationForm.ModifiedDate,
            EvaluationFormPublished = _evaluationData.EvaluationForm.Published,
            ConversationTimeSpan = conversationTimeSpan,
            MediaType =  string.Join(',', _evaluationData.MediaType?.ToArray() ?? Array.Empty<EvaluationResponse.MediaTypeEnum>()),
            AgentComments = _evaluationData.Answers?.AgentComments ?? string.Empty,
            EvaluatorComments = _evaluationData.Answers?.Comments ?? string.Empty,
            EvaluatorPrivateComments = _evaluationData.Answers?.PrivateComments ?? string.Empty,
            TotalScore = _evaluationData.Answers?.TotalScore ?? 0,
            TotalNonCriticalScore = _evaluationData.Answers?.TotalNonCriticalScore ?? 0,
            TotalCriticalScore = _evaluationData.Answers?.TotalCriticalScore ?? 0,
            EvaluationGroups = evaluationGroups
        };

        return SystemResponse.SuccessResponse(evaluationRecord);
    }
}