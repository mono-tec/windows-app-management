using Serilog;
using TaskWorkerSample.Common;
using TaskWorkerSample.Services.Interfaces;

namespace TaskWorkerSample.Services.Execution;

/// <summary>
/// 意図的に例外を発生させる処理です。
/// </summary>
public sealed class ExceptionExecutionService(ILogger logger) : IExecutionService
{
    /// <inheritdoc />
    public Task<int> ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            throw new InvalidOperationException("異常終了処理が呼び出されました。");
        }
        catch (Exception ex)
        {
            logger.Error(ex, "異常終了処理で例外が発生しました。");
            return Task.FromResult(ExitCodes.Failure);
        }
    }
}