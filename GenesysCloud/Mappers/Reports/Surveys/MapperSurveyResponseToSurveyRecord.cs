using GenesysCloud.DTO.Response.Reports.Survey;
using SurveyForm = GenesysCloud.DTO.Response.Reports.Survey.SurveyForm;
using SurveyScoringSet = GenesysCloud.DTO.Response.Reports.Survey.SurveyScoringSet;
using UserProfile = GenesysCloud.DTO.Response.Users.UserProfile;

namespace GenesysCloud.Mappers.Reports.Surveys;

public class MapperSurveyResponseToSurveyRecord
{
    private readonly List<Survey> _surveys;
    private readonly UserProfile _agentProfile;
    private readonly string _conversationId;
    

    public MapperSurveyResponseToSurveyRecord(List<Survey> surveys, UserProfile agentProfile, string conversationId)
    {
        _surveys = surveys ?? throw new ArgumentException("Surveys are required. Array.Empty<string> is ok" );
        _agentProfile = agentProfile ?? throw new ArgumentException("Agent Profile is required.");
        _conversationId = conversationId ?? throw new ArgumentException("Conversation Id is required.");
    }

    public ServiceResponse<List<SurveyRecord>> Map()
    {
        var surveyRecords = new List<SurveyRecord>();
        
        foreach (var survey in _surveys)
        {
            if (survey.SurveyForm is null)
                continue;
            
            var form = survey.SurveyForm;
            
            var questionGroups = survey.SurveyForm.QuestionGroups;

            if (questionGroups is null) continue;
            
            var dictionaryQuestionIdToQuestion = questionGroups
                .SelectMany(questionGroup => questionGroup?.Questions ?? Enumerable.Empty<SurveyQuestion>())
                .ToDictionary(x => x.Id, x => x.Text);
            
            var dictionaryAnswerOptionIdToText = questionGroups
                .SelectMany(x => x?.Questions ?? Enumerable.Empty<SurveyQuestion>())
                .SelectMany(y => y.AnswerOptions ?? Enumerable.Empty<AnswerOption>())
                .ToDictionary(y => y.Id, y => y.Text);
            
            var dictionaryQuestionGroups = questionGroups
                .ToDictionary(x => x.Id, x => x.Name);
            
            var answerGroups = survey.Answers?.QuestionGroupScores?
                .Select(answerGroup => answerGroup?.QuestionScores?.ToDictionary(x => x.QuestionId, x => x))
                .ToArray();
            
            var curGroup = 0;

            var surveyGroups = new List<SurveyGroup>();
            foreach (var questionGroup in dictionaryQuestionGroups)
            {
                var surveyGroup = new SurveyGroup { Title = questionGroup.Value };
                var surveyQuestionScores = answerGroups?[curGroup++];
                if (surveyQuestionScores is null) continue;

                foreach (var surveyQuestionScore in surveyQuestionScores.Values.Where(score => score.MarkedNA is not true))
                {
                    if (!dictionaryQuestionIdToQuestion.TryGetValue(surveyQuestionScore.QuestionId, out var surveyQuestion))
                        continue;

                    var surveyAnswer = string.Empty;
                    if(surveyQuestionScore.AnswerId is not null)
                        dictionaryAnswerOptionIdToText.TryGetValue(surveyQuestionScore.AnswerId, out surveyAnswer);

                    var assistedAnswer = string.Empty;
                    if(surveyQuestionScore.AssistedAnswerId is not null)
                        dictionaryAnswerOptionIdToText.TryGetValue(surveyQuestionScore.AssistedAnswerId, out assistedAnswer);

                    var scoringSet = new SurveyScoringSet
                    {
                        Question = surveyQuestion,
                        SurveyAnswer = $"{surveyAnswer}",
                        AssistedAnswer = $"{assistedAnswer}",
                        FreeTextAnswer = $"{surveyQuestionScore.FreeTextAnswer}",
                        NpsTextAnswer = $"{surveyQuestionScore.NpsTextAnswer}",
                        Score = $"{surveyQuestionScore.Score}",
                        NpsScore = $"{surveyQuestionScore.NpsScore}"
                    };
                    
                    surveyGroup.ScoringSets?.Add(scoringSet);
                }
                
                surveyGroups.Add(surveyGroup);
            }
            
            var surveyForm = new SurveyForm
            {
                Name = form.Name,
                Header = form.Header,
                Footer = form.Footer,
                Disabled = form.Disabled.GetValueOrDefault(false),
                Language = form.Language,
                ModifiedDate = form.ModifiedDate?.ToString() ?? string.Empty,
                Published = form.Published.GetValueOrDefault(false),
                SurveyGroups = surveyGroups
            };
            
            var surveyRecord = new SurveyRecord
            {
                AgentProfile = _agentProfile,
                Form = surveyForm,
                InteractionLink = $"https://apps.euw2.pure.cloud/directory/#/engage/admin/interactions/{_conversationId}/qualitySummary",
                CompletedDate = $"{survey.CompletedDate}"
            };
            
            surveyRecords.Add(surveyRecord);
        }
        
        return SystemResponse.SuccessResponse(surveyRecords);
    }
}