using GenesysCloud.DTO.Response.Reports.Evaluation;
using EvaluationScoringSet = GenesysCloud.DTO.Response.Reports.Evaluation.EvaluationScoringSet;
using UserProfile = GenesysCloud.DTO.Response.Users.UserProfile;

namespace GenesysCloud.Mappers.Reports.Evaluations;

public sealed class MapperEvaluationResponseToEvaluationRecords
{
    private readonly MetricsInterval _interval;
    private readonly EvaluationResponse _evaluationData;
    private readonly ConversationMetrics _conversationMetrics;

    public MapperEvaluationResponseToEvaluationRecords(MetricsInterval interval, EvaluationResponse evaluationData, ConversationMetrics conversationMetrics)
    {
        _interval = interval;
        _evaluationData = evaluationData;
        _conversationMetrics = conversationMetrics;
    }

    public EvaluationRecord Map()
    {
        TimeSpan? conversationTimeSpan =
            _evaluationData.ConversationEndDate - _evaluationData.ConversationDate ?? new TimeSpan(0);

        var questionGroups = _evaluationData.EvaluationForm.QuestionGroups;

        var dictionaryQuestionIdToQuestion = questionGroups
            .SelectMany(questionGroup => questionGroup.Questions)
            .ToDictionary(x => x.Id, x => x.Text);

        var dictionaryAnswerOptionIdToText = questionGroups
            .SelectMany(x => x.Questions)
            .SelectMany(y => y.AnswerOptions)
            .ToDictionary(y => y.Id, y => y.Text);

        var dictionaryQuestionGroups = questionGroups
            .ToDictionary(x => x.Id, x => x.Name);

        var answerGroups = _evaluationData.Answers.QuestionGroupScores
            .Select(answerGroup => answerGroup.QuestionScores.ToDictionary(x => x.QuestionId, x => x))
            .ToArray();

        var evaluationGroups = new List<EvaluationGroup>();
        var curGroup = 0;

        foreach (var questionGroup in dictionaryQuestionGroups)
        {
            var evaluationGroup = new EvaluationGroup { Title = questionGroup.Value };

            var answerGroup = answerGroups[curGroup++];

            foreach (var answer in answerGroup.Values.Where(answer => answer.MarkedNA is not true))
            {
                if (!dictionaryQuestionIdToQuestion.TryGetValue(answer.QuestionId, out var evaluationQuestion) ||
                    !dictionaryAnswerOptionIdToText.TryGetValue(answer.AnswerId, out var evaluationAnswer)) { continue; }

                var scoringSet = new EvaluationScoringSet
                {
                    Question = evaluationQuestion,
                    Answer = evaluationAnswer,
                    Comment = answer.Comments ?? string.Empty,
                    Score = answer.Score
                };

                evaluationGroup.ScoringSets?.Add(scoringSet);
            }

            evaluationGroups.Add(evaluationGroup);
        }

        var silenceDurationPercentage = _conversationMetrics.ParticipantMetrics?.SilenceDurationPercentage is null 
            ? "N/A" 
            : $"{_conversationMetrics.ParticipantMetrics.SilenceDurationPercentage.ToString()}%";
        
        var evaluationRecord = new EvaluationRecord
        {
            IntervalSpan = _interval,
            Status = _evaluationData.Status?.ToString(),
            AgentHasRead = _evaluationData.AgentHasRead ?? false,
            AgentProfile = new UserProfile
            {
                Email = _evaluationData.Agent.Email,
                Name = _evaluationData.Agent.Name
            },
            EvaluatorProfile = new UserProfile
            {
                Email = _evaluationData.Evaluator.Email,
                Name = _evaluationData.Evaluator.Name
            },
            EvaluationFormName = _evaluationData.EvaluationForm.Name,
            EvaluationFormModifiedDate = _evaluationData.EvaluationForm.ModifiedDate,
            EvaluationFormPublished = _evaluationData.EvaluationForm.Published,
            ConversationTimeSpan = conversationTimeSpan,
            MediaType = string.Join(',', _evaluationData.MediaType?.ToArray() ?? Array.Empty<EvaluationResponse.MediaTypeEnum>()),
            AgentComments = _evaluationData.Answers?.AgentComments ?? string.Empty,
            EvaluatorComments = _evaluationData.Answers?.Comments ?? string.Empty,
            EvaluatorPrivateComments = _evaluationData.Answers?.PrivateComments ?? string.Empty,
            TotalScore = _evaluationData.Answers?.TotalScore ?? 0,
            TotalNonCriticalScore = _evaluationData.Answers?.TotalNonCriticalScore ?? 0,
            TotalCriticalScore = _evaluationData.Answers?.TotalCriticalScore ?? 0,
            SilenceDurationPercentage = silenceDurationPercentage,
            EvaluationGroups = evaluationGroups
        };

        return evaluationRecord;
    }
}