using TaskWorkerSample.Common;
using TaskWorkerSample.Configurations;
using TaskWorkerSample.Services;
using TaskWorkerSample.Services.Execution;
using TaskWorkerSample.Tests.TestDoubles;
using Xunit;

namespace TaskWorkerSample.Tests.Services;

/// <summary>
/// ContinuousExecutionService のテストです。
/// </summary>
public sealed class ContinuousExecutionServiceTests
{
    [Fact]
    public async Task ExecuteAsync_Should_Return_Success_When_MaxTimeReached()
    {
        // Arrange
        var logger = TestLogger.Create();

        var settings = new AppSettings
        {
            Language = "ja",
            IntervalSeconds = 0
        };

        var appPaths = new AppPaths(@"C:\ProgramData\Company\TaskWorker10Min");
        var personFactory = new PersonFactory();

        // Now を呼ぶたびに 2 分進める
        var clock = new FakeClock(
            start: new DateTimeOffset(2026, 4, 19, 12, 0, 0, TimeSpan.Zero),
            step: TimeSpan.FromMinutes(2));

        var service = new ContinuousExecutionService(
            logger,
            settings,
            appPaths,
            personFactory,
            clock);

        // Act
        int result = await service.ExecuteAsync(CancellationToken.None);

        // Assert
        Assert.Equal(ExitCodes.Success, result);
    }
}