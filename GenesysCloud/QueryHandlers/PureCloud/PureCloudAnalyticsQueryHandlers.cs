using GenesysCloud.QueryHandlers.Contracts;
using PureCloudPlatform.Client.V2.Api;
using PureCloudPlatform.Client.V2.Client;

namespace GenesysCloud.QueryHandlers.PureCloud;

public class PureCloudAnalyticsQueryHandlers : IAnalyticsQueryHandlers
{
    private readonly AnalyticsApi _analyticsApi;

    public PureCloudAnalyticsQueryHandlers(AnalyticsApi analyticsApi)
    {
        _analyticsApi = analyticsApi;
    }
    
    public GenesysServiceResponse<List<ConversationAggregateDataContainer>> GenesysConversationsAggregatesQuery(ConversationAggregationQuery query)
    {
        try
        {
            var response = _analyticsApi.PostAnalyticsConversationsAggregatesQuery(query);
            
            if (response is null)
                return GenesysResponse.FailureResponse<List<ConversationAggregateDataContainer>>(
                    errorMessage: "Constants.NullResponse", query: query.JsonSerializeToString(query.GetType()));

            return GenesysResponse.SuccessResponse(response.Results ?? new List<ConversationAggregateDataContainer>());
        }
        catch (Exception exception)
        {
            return GenesysResponse.ExceptionHandler.HandleException<List<ConversationAggregateDataContainer>>(exception);
        }
    }
    
    public GenesysServiceResponse<List<AnalyticsConversationWithoutAttributes>> ConversationDetailQuery(ConversationQuery query)
    {
        var pageCount = Constants.Unknown;
        var currentPage = Constants.FirstPage;
        var analyticsConversationWithoutAttributesList = new List<AnalyticsConversationWithoutAttributes>();
        
        try
        {
            while (pageCount >= currentPage || pageCount is Constants.Unknown)
            {
                var response = _analyticsApi.PostAnalyticsConversationsDetailsQuery(query);

                if (response is null)
                {
                    return GenesysResponse.FailureResponse<List<AnalyticsConversationWithoutAttributes>>(
                        Constants.NullResponse,
                        Constants.HtmlServerError,
                        query.JsonSerializeToString(query.GetType()));
                }
                
                if (response.TotalHits is 0)
                    return GenesysResponse.SuccessResponse(analyticsConversationWithoutAttributesList);
                
                analyticsConversationWithoutAttributesList.AddRange(response.Conversations ?? Enumerable.Empty<AnalyticsConversationWithoutAttributes>());
                
                if (pageCount is Constants.Unknown)
                    pageCount = (int)Math.Ceiling(response.TotalHits.GetValueOrDefault(0) / 100.0);
                
                query.Paging.PageNumber = ++currentPage;
            }

            return GenesysResponse.SuccessResponse(analyticsConversationWithoutAttributesList);
        }
        catch (Exception exception)
        {
            return GenesysResponse.ExceptionHandler.HandleException<List<AnalyticsConversationWithoutAttributes>>(exception);
        }
    }

    public GenesysServiceResponse<List<SurveyAggregateDataContainer>> SurveyAggregatesQuery(SurveyAggregationQuery query)
    {
        try
        {
            var response = _analyticsApi.PostAnalyticsSurveysAggregatesQuery(query);

            if (response is null)
                return GenesysResponse.FailureResponse<List<SurveyAggregateDataContainer>>(
                    errorMessage: Constants.NullResponse, query: query.JsonSerializeToString(query.GetType()));

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
            return GenesysResponse.ExceptionHandler.HandleException<List<SurveyAggregateDataContainer>>(exception);
        }
    }

    public GenesysServiceResponse<List<EvaluationAggregateDataContainer>> EvaluationAggregationQuery(EvaluationAggregationQuery query)
    {
        try
        {
            var response = _analyticsApi.PostAnalyticsEvaluationsAggregatesQuery(query);
            
            if (response is null)
                return GenesysResponse.FailureResponse<List<EvaluationAggregateDataContainer>>(
                    errorMessage: Constants.NullResponse, query: query.JsonSerializeToString(query.GetType()));

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
            return GenesysResponse.ExceptionHandler.HandleException<List<EvaluationAggregateDataContainer>>(exception);
        }
    }
}