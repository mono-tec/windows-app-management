using TaskWorkerSample.Common;
using TaskWorkerSample.Configurations;
using TaskWorkerSample.Services;
using TaskWorkerSample.Services.Execution;
using TaskWorkerSample.Tests.TestDoubles;
using Xunit;

namespace TaskWorkerSample.Tests.Services;

/// <summary>
/// TimedExitExecutionService のテストです。
/// </summary>
public sealed class TimedExitExecutionServiceTests
{
    [Fact]
    public async Task ExecuteAsync_Should_Return_Success_When_TimeoutReached()
    {
        // Arrange
        var logger = TestLogger.Create();

        var settings = new AppSettings
        {
            Language = "ja",
            TimeoutMinutes = 1,
            IntervalSeconds = 0
        };

        var appPaths = new AppPaths(@"C:\ProgramData\Company\TaskWorker10Min");
        var personFactory = new PersonFactory();

        // Now を呼ぶたびに 30 秒進める
        var clock = new FakeClock(
            start: new DateTimeOffset(2026, 4, 19, 12, 0, 0, TimeSpan.Zero),
            step: TimeSpan.FromSeconds(30));

        var service = new TimedExitExecutionService(
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