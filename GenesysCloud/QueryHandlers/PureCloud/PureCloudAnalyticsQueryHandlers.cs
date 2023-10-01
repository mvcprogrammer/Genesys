using GenesysCloud.QueryHandlers.Contracts;
using PureCloudPlatform.Client.V2.Api;

namespace GenesysCloud.QueryHandlers.PureCloud;

/// <summary>
/// PureCloud Analytics Query Handler is responsible for submitting an PureCloud.Client.V2.Model AnalyticsApi query to PureCloud.
/// The API can be called multiple times if the response is paged.
/// Handlers are not dependent on query filters; 1 handler, many queries.
/// The query handler always returns Genesys a Client.V2.Model in the data field of a ServiceResponse.
/// DTO must never be done at this level for dependency segmentation.
/// </summary>
internal sealed class PureCloudAnalyticsQueryHandlers : IAnalyticsQueryHandlers
{
    private readonly AnalyticsApi _analyticsApi = new();

    public List<ConversationAggregateDataContainer> GenesysConversationsAggregatesQuery(ConversationAggregationQuery query)
    {
        try
        {
            var response = _analyticsApi.PostAnalyticsConversationsAggregatesQuery(body: query);
            return response.Results ?? new List<ConversationAggregateDataContainer>();
        }
        catch (Exception exception)
        {
            ServiceResponse.ExceptionHandler.HandleException<List<ConversationAggregateDataContainer>>(exception);
            throw;
        }
    }
    
    public List<AnalyticsConversationWithoutAttributes> ConversationDetailQuery(ConversationQuery query)
    {
        var pageCount = Constants.Unknown;
        var currentPage = Constants.FirstPage;
        var analyticsConversationWithoutAttributesList = new List<AnalyticsConversationWithoutAttributes>();
        
        try
        {
            while (pageCount >= currentPage || pageCount is Constants.Unknown)
            {
                var response = _analyticsApi.PostAnalyticsConversationsDetailsQuery(body: query);

                if (response.TotalHits is 0)
                    return new List<AnalyticsConversationWithoutAttributes>();
                
                analyticsConversationWithoutAttributesList.AddRange(response.Conversations ?? Enumerable.Empty<AnalyticsConversationWithoutAttributes>());
                
                if (pageCount is Constants.Unknown)
                    pageCount = (int)Math.Ceiling(response.TotalHits.GetValueOrDefault(0) / 100.0);
                
                query.Paging.PageNumber = ++currentPage;
            }
            
            return analyticsConversationWithoutAttributesList;
        }
        catch (Exception exception)
        {
            ServiceResponse.ExceptionHandler.HandleException<List<AnalyticsConversationWithoutAttributes>>(exception, query.JsonSerializeToString(query.GetType()));
            throw;
        }
    }
    
    public List<SurveyAggregateDataContainer> SurveyAggregatesQuery(SurveyAggregationQuery query)
    {
        try
        {
            var response = _analyticsApi.PostAnalyticsSurveysAggregatesQuery(body: query);
            return response.Results ?? new List<SurveyAggregateDataContainer>();
        }
        catch (Exception exception)
        {
            ServiceResponse.ExceptionHandler.HandleException<List<SurveyAggregateDataContainer>>(exception, query.JsonSerializeToString(query.GetType()));
            throw;
        }
    }

    public List<EvaluationAggregateDataContainer> EvaluationAggregationQuery(EvaluationAggregationQuery query)
    {
        try
        {
            var response = _analyticsApi.PostAnalyticsEvaluationsAggregatesQuery(body: query);
            return response.Results ?? new List<EvaluationAggregateDataContainer>();
        }
        catch (Exception exception)
        {
            ServiceResponse.ExceptionHandler.HandleException<List<EvaluationAggregateDataContainer>>(exception, query.JsonSerializeToString(query.GetType()));
            throw;
        }
    }
}