using GenesysCloud.Services.Contracts.Fundamental;
using GenesysCloud.Services.PureCloud.Derived;

namespace GenesysCloud.Tests
{
    public class PureCloudReportDataServiceTests
    {
        private readonly Mock<IAnalyticsService> _mockAnalyticsService;
        private readonly Mock<IQualityService> _mockQualityService;
        private readonly PureCloudReportDataService _service;

        public PureCloudReportDataServiceTests()
        {
            _mockAnalyticsService = new Mock<IAnalyticsService>();
            _mockQualityService = new Mock<IQualityService>();
            
            _service = new PureCloudReportDataService(_mockAnalyticsService.Object, _mockQualityService.Object);
        }

        [Fact]
        public void GetEvaluationRecords_GetAgentProfileLookupFailure_ReturnsFailureResponse()
        {
            // Arrange
            var startTime = DateTime.UtcNow.AddHours(-1);
            var endTime = DateTime.UtcNow;
            IReadOnlyCollection<string> divisions = new List<string> { "Division1" };
            IReadOnlyCollection<string> queues = new List<string> { "Queue1" };

            // Act
            var result = _service.GetEvaluationRecords(startTime, endTime, divisions, queues);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Failure getting user profiles.", result.ErrorMessage);
        }
    }
}
