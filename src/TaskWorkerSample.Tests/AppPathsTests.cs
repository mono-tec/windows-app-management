using TaskWorkerSample.Common;
using Xunit;

namespace TaskWorkerSample.Tests.Common;

/// <summary>
/// AppPaths のテストです。
/// </summary>
public sealed class AppPathsTests
{
    [Fact]
    public void FromConfigPath_Should_CreateBaseDir_From_ConfigPath()
    {
        // Arrange
        string configPath = @"C:\ProgramData\Company\TaskWorker10Min\config\appsettings.json";

        // Act
        AppPaths paths = AppPaths.FromConfigPath(configPath);

        // Assert
        Assert.Equal(@"C:\ProgramData\Company\TaskWorker10Min", paths.BaseDir);
        Assert.Equal(@"C:\ProgramData\Company\TaskWorker10Min\config", paths.ConfigDir);
        Assert.Equal(@"C:\ProgramData\Company\TaskWorker10Min\logs", paths.LogsDir);
        Assert.Equal(@"C:\ProgramData\Company\TaskWorker10Min\control", paths.ControlDir);
        Assert.Equal(@"C:\ProgramData\Company\TaskWorker10Min\control\stop_sync.flag", paths.StopFlagPath);
    }

    [Fact]
    public void FromConfigPath_Should_Throw_When_PathIsEmpty()
    {
        Assert.Throws<ArgumentException>(() => AppPaths.FromConfigPath(string.Empty));
    }
}