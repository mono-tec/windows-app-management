using System.IO;
using Serilog;
using TaskWorkerSample.Common;
using TaskWorkerSample.Configurations;
using TaskWorkerSample.Services.Interfaces;

namespace TaskWorkerSample.Services.Execution;

/// <summary>
/// 最大10分間、一定間隔で継続実行する処理です。
/// </summary>
public sealed class ContinuousExecutionService(
    ILogger logger,
    AppSettings settings,
    AppPaths appPaths,
    IPersonFactory personFactory,
    IClock clock) : IExecutionService
{
    /// <inheritdoc />
    public async Task<int> ExecuteAsync(CancellationToken cancellationToken)
    {
        DateTimeOffset startTime = clock.Now;
        bool greeted = false;

        while (!cancellationToken.IsCancellationRequested)
        {
            if (File.Exists(appPaths.StopFlagPath))
            {
                logger.Information("停止フラグを検出したため、継続処理を終了します。");
                break;
            }

            TimeSpan elapsed = clock.Now - startTime;
            if (elapsed >= TimeSpan.FromMinutes(10))
            {
                break;
            }

            logger.Information("継続実行処理を実行中です。経過秒数: {ElapsedSeconds}", (int)elapsed.TotalSeconds);

            if (!greeted && elapsed >= TimeSpan.FromMinutes(1))
            {
                var person = personFactory.Create(settings.Language);
                logger.Information("挨拶メッセージ: {Greeting}", person.GetGreeting());
                greeted = true;
            }

            await Task.Delay(TimeSpan.FromSeconds(settings.IntervalSeconds), cancellationToken);
        }

        return ExitCodes.Success;
    }
}