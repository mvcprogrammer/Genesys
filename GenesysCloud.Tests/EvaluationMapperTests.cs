using GenesysCloud.Helpers;
using GenesysCloud.Mappers.Reports.Evaluations;
using PureCloudPlatform.Client.V2.Model;

namespace GenesysCloud.Tests;

public class MapperEvaluationResponseToEvaluationRecordsTests
{
    [Fact]
    public void Map_WithValidInputs_ReturnsCorrectEvaluationRecord()
    {
        // Arrange
        var interval = new MetricsInterval(new DateTime(2023, 08, 01), new DateTime(2023, 08, 30));
        var evaluationData = new EvaluationResponse();
        var conversationMetrics = new ConversationMetrics();
    
        var mapper = new MapperEvaluationResponseToEvaluationRecords(interval, evaluationData, conversationMetrics);

        // Act
        var result = mapper.Map();

        // Assert
        
    }

}