using GenesysCloud.QueryHandlers.Contracts;
using PureCloudPlatform.Client.V2.Api;

namespace GenesysCloud.QueryHandlers.PureCloud;

internal sealed class PureCloudAnalyticsQueryHandlers : IAnalyticsQueryHandlers
{
    private readonly AnalyticsApi _analyticsApi = new();

    public ServiceResponse<List<ConversationAggregateDataContainer>> GenesysConversationsAggregatesQuery(ConversationAggregationQuery query)
    {
        try
        {
            var response = _analyticsApi.PostAnalyticsConversationsAggregatesQuery(query);
            return SystemResponse.SuccessResponse(response.Results ?? new List<ConversationAggregateDataContainer>());
        }
        catch (Exception exception)
        {
            return SystemResponse.ExceptionHandler.HandleException<List<ConversationAggregateDataContainer>>(exception);
        }
    }
    
    public ServiceResponse<List<AnalyticsConversationWithoutAttributes>> ConversationDetailQuery(ConversationQuery query)
    {
        var pageCount = Constants.Unknown;
        var currentPage = Constants.FirstPage;
        var analyticsConversationWithoutAttributesList = new List<AnalyticsConversationWithoutAttributes>();
        
        try
        {
            while (pageCount >= currentPage || pageCount is Constants.Unknown)
            {
                var response = _analyticsApi.PostAnalyticsConversationsDetailsQuery(query);
                
                if (response.TotalHits is 0)
                    return SystemResponse.SuccessResponse(analyticsConversationWithoutAttributesList);
                
                analyticsConversationWithoutAttributesList.AddRange(response.Conversations ?? Enumerable.Empty<AnalyticsConversationWithoutAttributes>());
                
                if (pageCount is Constants.Unknown)
                    pageCount = (int)Math.Ceiling(response.TotalHits.GetValueOrDefault(0) / 100.0);
                
                query.Paging.PageNumber = ++currentPage;
            }

            return SystemResponse.SuccessResponse(analyticsConversationWithoutAttributesList);
        }
        catch (Exception exception)
        {
            return SystemResponse.ExceptionHandler.HandleException<List<AnalyticsConversationWithoutAttributes>>(exception,
                query.JsonSerializeToString(query.GetType()));
        }
    }

    public ServiceResponse<List<SurveyAggregateDataContainer>> SurveyAggregatesQuery(SurveyAggregationQuery query)
    {
        try
        {
            var response = _analyticsApi.PostAnalyticsSurveysAggregatesQuery(query);
            return SystemResponse.SuccessResponse(response.Results ?? new List<SurveyAggregateDataContainer>());
        }
        catch (Exception exception)
        {
            return SystemResponse.ExceptionHandler.HandleException<List<SurveyAggregateDataContainer>>(exception,
                query.JsonSerializeToString(query.GetType()));
        }
    }

    public ServiceResponse<List<EvaluationAggregateDataContainer>> EvaluationAggregationQuery(EvaluationAggregationQuery query)
    {
        try
        {
            var response = _analyticsApi.PostAnalyticsEvaluationsAggregatesQuery(query);
            return SystemResponse.SuccessResponse(response.Results ?? new List<EvaluationAggregateDataContainer>());
        }
        catch (Exception exception)
        {
            return SystemResponse.ExceptionHandler.HandleException<List<EvaluationAggregateDataContainer>>(exception,
                query.JsonSerializeToString(query.GetType()));
        }
    }
}