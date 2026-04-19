using TaskWorkerSample.Common;
using Xunit;

namespace TaskWorkerSample.Tests.Common;

/// <summary>
/// ConfigPathResolver のテストです。
/// </summary>
public sealed class ConfigPathResolverTests
{
    [Fact]
    public void Resolve_Should_Return_ArgumentPath_When_ConfigArgumentExists()
    {
        string[] args =
        [
            "--config",
        @"C:\ProgramData\Company\TaskWorker10Min\config\appsettings.json"
        ];

        string result = ConfigPathResolver.Resolve(args);

        Assert.Equal(
            @"C:\ProgramData\Company\TaskWorker10Min\config\appsettings.json",
            result);
    }

    [Fact]
    public void Resolve_Should_Return_BaseDirectory_AppSettings_When_NoArgumentAndNoEnvironmentVariable()
    {
        // Arrange
        Environment.SetEnvironmentVariable("TASKWORKERSAMPLE_CONFIG_PATH", null);

        string[] args = [];

        // Act
        string result = ConfigPathResolver.Resolve(args);

        // Assert
        string expected = Path.Combine(AppContext.BaseDirectory, "appsettings.json");

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Resolve_Should_Return_EnvironmentVariable_When_NoArgument()
    {
        // Arrange
        Environment.SetEnvironmentVariable(
            "TASKWORKERSAMPLE_CONFIG_PATH",
            @"C:\ProgramData\Company\TaskWorkerHourly\config\appsettings.json");

        string[] args = [];
        string defaultPath = @"C:\app\appsettings.json";

        try
        {
            // Act
            string result = ConfigPathResolver.Resolve(args);

            // Assert
            Assert.Equal(
                @"C:\ProgramData\Company\TaskWorkerHourly\config\appsettings.json",
                result);
        }
        finally
        {
            Environment.SetEnvironmentVariable("TASKWORKERSAMPLE_CONFIG_PATH", null);
        }
    }
}