using TaskWorkerSample.Common;
using TaskWorkerSample.Services.Execution;
using TaskWorkerSample.Tests.TestDoubles;
using Xunit;

namespace TaskWorkerSample.Tests.Services;

/// <summary>
/// ImmediateExitService のテストです。
/// </summary>
public sealed class ImmediateExitServiceTests
{
    [Fact]
    public async Task ExecuteAsync_Should_Return_Success()
    {
        // Arrange
        var logger = TestLogger.Create();
        var service = new ImmediateExitService(logger);

        // Act
        int result = await service.ExecuteAsync(CancellationToken.None);

        // Assert
        Assert.Equal(ExitCodes.Success, result);
    }
}