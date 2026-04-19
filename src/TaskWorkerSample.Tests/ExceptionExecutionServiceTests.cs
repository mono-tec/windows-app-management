using TaskWorkerSample.Common;
using TaskWorkerSample.Services.Execution;
using TaskWorkerSample.Tests.TestDoubles;
using Xunit;

namespace TaskWorkerSample.Tests.Services;

/// <summary>
/// ExceptionExecutionService のテストです。
/// </summary>
public sealed class ExceptionExecutionServiceTests
{
    [Fact]
    public async Task ExecuteAsync_Should_Return_Failure()
    {
        // Arrange
        var logger = TestLogger.Create();
        var service = new ExceptionExecutionService(logger);

        // Act
        int result = await service.ExecuteAsync(CancellationToken.None);

        // Assert
        Assert.Equal(ExitCodes.Failure, result);
    }
}