using PureCloudPlatform.Client.V2.Api;
using PureCloudPlatform.Client.V2.Client;

namespace GenesysCloud.QueryHandlers.PureCloud.Analytics;

public class AnalyticsQueryHandlers : IAnalyticsQueryHandlers
{
    private readonly AnalyticsApi _analyticsApi = new();
    
    public GenesysServiceResponse<List<ConversationAggregateDataContainer>> GenesysConversationsAggregatesQuery(ConversationAggregationQuery query)
    {
        try
        {
            var response = _analyticsApi.PostAnalyticsConversationsAggregatesQuery(query);
            
            if (response is null)
                return GenesysResponse.FailureResponse<List<ConversationAggregateDataContainer>>(
                    errorMessage: "Null Response", query: query.JsonSerializeToString(query.GetType()));

            return GenesysResponse.SuccessResponse(response.Results ?? new List<ConversationAggregateDataContainer>());
        }
        catch (ApiException exception)
        {
            return GenesysResponse.FailureResponse<List<ConversationAggregateDataContainer>>(
                exception.Message, 
                exception.ErrorCode,
                query.JsonSerializeToString(query.GetType()));
        }
        catch (Exception exception)
        {
            return GenesysResponse.FailureResponse<List<ConversationAggregateDataContainer>>(exception.Message);
        }
    }
    
    public GenesysServiceResponse<List<AnalyticsConversationWithoutAttributes>> ConversationDetailQuery(ConversationQuery query)
    {
        var pageCount = Constants.Unknown;
        var curPage = Constants.FirstPage;
        var analyticsConversationWithoutAttributesList = new List<AnalyticsConversationWithoutAttributes>();
        
        try
        {
            while (pageCount >= curPage || pageCount is Constants.Unknown)
            {
                var response = _analyticsApi.PostAnalyticsConversationsDetailsQuery(query);

                if (response is null)
                {
                    return GenesysResponse.FailureResponse<List<AnalyticsConversationWithoutAttributes>>(
                        "Null Response",
                        Constants.HtmlServerError,
                        query.JsonSerializeToString(query.GetType()));
                }
                
                if (response.TotalHits is 0)
                    return GenesysResponse.SuccessResponse(new List<AnalyticsConversationWithoutAttributes>());
                
                analyticsConversationWithoutAttributesList.AddRange(response.Conversations);
                
                if (pageCount is Constants.Unknown)
                    pageCount = (int)Math.Ceiling((double)response.TotalHits.GetValueOrDefault(0) / 100);
                
                query.Paging.PageNumber = ++curPage;
            }

            return GenesysResponse.SuccessResponse(analyticsConversationWithoutAttributesList);
        }
        catch (ApiException exception)
        {
            return GenesysResponse.FailureResponse<List<AnalyticsConversationWithoutAttributes>>(
                exception.Message, 
                exception.ErrorCode,
                query.JsonSerializeToString(query.GetType()));
        }
        catch (Exception exception)
        {
            return GenesysResponse.FailureResponse<List<AnalyticsConversationWithoutAttributes>>(exception.Message);
        }
    }

    public GenesysServiceResponse<List<SurveyAggregateDataContainer>> SurveyAggregatesQuery(SurveyAggregationQuery query)
    {
        try
        {
            var response = _analyticsApi.PostAnalyticsSurveysAggregatesQuery(query);

            if (response is null)
                return GenesysResponse.FailureResponse<List<SurveyAggregateDataContainer>>(
                    errorMessage: "Null Response", query: query.JsonSerializeToString(query.GetType()));

            return GenesysResponse.SuccessResponse(response.Results ?? new List<SurveyAggregateDataContainer>());
        }
        catch (ApiException exception)
        {
            return GenesysResponse.FailureResponse<List<SurveyAggregateDataContainer>>(
                exception.Message, 
                exception.ErrorCode,
                query.JsonSerializeToString(query.GetType()));
        }
        catch (Exception exception)
        {
            return GenesysResponse.FailureResponse<List<SurveyAggregateDataContainer>>(exception.Message);
        }
    }

    public GenesysServiceResponse<List<EvaluationAggregateDataContainer>> EvaluationAggregationQuery(EvaluationAggregationQuery query)
    {
        try
        {
            var response = _analyticsApi.PostAnalyticsEvaluationsAggregatesQuery(query);
            
            if (response is null)
                return GenesysResponse.FailureResponse<List<EvaluationAggregateDataContainer>>(
                    errorMessage: "Null Response", query: query.JsonSerializeToString(query.GetType()));

            return GenesysResponse.SuccessResponse(response.Results ?? new List<EvaluationAggregateDataContainer>());
        }
        catch (ApiException exception)
        {
            return GenesysResponse.FailureResponse<List<EvaluationAggregateDataContainer>>(
                exception.Message, 
                exception.ErrorCode,
                query.JsonSerializeToString(query.GetType()));
        }
        catch (Exception exception)
        {
            return GenesysResponse.FailureResponse<List<EvaluationAggregateDataContainer>>(exception.Message);
        }
    }
}