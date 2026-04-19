using Serilog;
using TaskWorkerSample.Common;
using TaskWorkerSample.Services.Interfaces;

namespace TaskWorkerSample.Services.Execution;

/// <summary>
/// 起動後にメッセージを出力して即時終了する処理です。
/// </summary>
public sealed class ImmediateExitService : IExecutionService
{
    private readonly ILogger _logger;

    public ImmediateExitService(ILogger logger)
    {
        _logger = logger;
    }

    public Task<int> ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.Information("即時終了処理を実行します。");
        return Task.FromResult(ExitCodes.Success);
    }
}