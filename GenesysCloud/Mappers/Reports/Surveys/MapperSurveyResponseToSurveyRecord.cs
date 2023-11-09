using System.ComponentModel.DataAnnotations;
using GenesysCloud.DTO.Response;
using GenesysCloud.DTO.Response.Reports.Survey;
using SurveyForm = GenesysCloud.DTO.Response.Reports.Survey.SurveyForm;
using SurveyScoringSet = GenesysCloud.DTO.Response.Reports.Survey.SurveyScoringSet;
using UserProfile = GenesysCloud.DTO.Response.Users.UserProfile;

namespace GenesysCloud.Mappers.Reports.Surveys;

public class MapperSurveyResponseToSurveyRecord
{
    private readonly SurveyAggregateDataContainer _surveyAggregateDataContainer;
    private readonly IReadOnlyList<Survey> _surveys;
    private readonly IReadOnlyDictionary<string, UserProfile> _userProfiles;
    private readonly IReadOnlyDictionary<string, QueueProfile> _queueProfiles;
    private readonly IReadOnlyDictionary<string, AnalyticsConversationWithoutAttributes> _conversationDetails;
    
    private const string UserIdKey = "userId";
    private const string MediaTypeKey = "mediaType";
    private const string QueueIdKey = "queueId";
    private const string ConversationIdKey = "conversationId";
    
    public MapperSurveyResponseToSurveyRecord(
        SurveyAggregateDataContainer surveyAggregateDataContainer, 
        List<Survey> surveys, 
        IReadOnlyDictionary<string, UserProfile> userProfiles, 
        IReadOnlyDictionary<string, QueueProfile> queueProfiles,
        IReadOnlyDictionary<string, AnalyticsConversationWithoutAttributes> conversationDetails)
    {
        _surveyAggregateDataContainer = surveyAggregateDataContainer ?? throw new ArgumentException("Survey Summary is required.");
        _surveys = surveys ?? throw new ArgumentException("Surveys are required.");
        _userProfiles = userProfiles ?? throw new ArgumentException("User Profiles are required.");
        _queueProfiles = queueProfiles ?? throw new ArgumentException("Queue Profiles are required.");
        _conversationDetails = conversationDetails ?? throw new ArgumentException("Conversation Details are required.");
    }

    public List<SurveyRecord> Map()
    {
        var surveyAggregateDataGroupDictionary = _surveyAggregateDataContainer.Group;
        
        if (surveyAggregateDataGroupDictionary.TryGetValue(ConversationIdKey, out var conversationId) is false)
            throw new ValidationException("Failed to find interaction key.");
        
        if (surveyAggregateDataGroupDictionary.TryGetValue(UserIdKey, out var userId) is false) 
            throw new ValidationException("Failed to find user key.");

        if (_userProfiles.TryGetValue(userId, out var agentProfile) is false) 
            throw new ValidationException("Failed to lookup user by key.");
            
        if(surveyAggregateDataGroupDictionary.TryGetValue(MediaTypeKey, out var mediaType) is false)
            throw new ValidationException("Failed to get media type.");

        if (surveyAggregateDataGroupDictionary.TryGetValue(QueueIdKey, out var queueId) is false)
            throw new ValidationException("Failed to get queue id.");
            
        if(_queueProfiles.TryGetValue(queueId, out var queueProfile) is false)
        {
            queueProfile = new QueueProfile();
            //throw new ValidationException("Failed to get queue name.");
            //ToDo: make this work correctly
        }
            

        if (_conversationDetails.TryGetValue(conversationId, out var conversationDetail) is false)
            throw new ValidationException($"Failed to get conversation detail: {conversationId}");
        
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
                SurveyGroups = surveyGroups,
                TotalScore = ""
            };
            
            var surveyRecord = new SurveyRecord
            {
                AgentProfile = agentProfile,
                MediaType = mediaType,
                QueueName = queueProfile.QueueName,
                Form = surveyForm,
                InteractionLink = $"https://apps.euw2.pure.cloud/directory/#/engage/admin/interactions/{conversationId}/qualitySummary",
                CompletedDate = $"{survey.CompletedDate}",
                ExternalTag = conversationDetail.ExternalTag,
                InteractionStartTime = $"{conversationDetail.ConversationStart.GetValueOrDefault().ToUniversalTime()}"
            };
            
            surveyRecords.Add(surveyRecord);
        }
        
        return surveyRecords;
    }
}