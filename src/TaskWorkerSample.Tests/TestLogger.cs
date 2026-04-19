using Serilog;

namespace TaskWorkerSample.Tests.TestDoubles;

/// <summary>
/// テスト用ロガーを生成します。
/// </summary>
public static class TestLogger
{
    /// <summary>
    /// 何も出力しないロガーを返します。
    /// </summary>
    public static ILogger Create()
    {
        return new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .CreateLogger();
    }
}