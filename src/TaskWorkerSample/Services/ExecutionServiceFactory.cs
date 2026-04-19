using TaskWorkerSample.Common;
using TaskWorkerSample.Services.Execution;
using TaskWorkerSample.Services.Interfaces;

namespace TaskWorkerSample.Services;

/// <summary>
/// 実行モードに応じて実行サービスを切り替えるファクトリです。
/// </summary>
public sealed class ExecutionServiceFactory : IExecutionServiceFactory
{
    private readonly ContinuousExecutionService _continuous;
    private readonly ImmediateExitService _immediate;
    private readonly TimedExitExecutionService _timedExit;
    private readonly ExceptionExecutionService _exception;

    /// <summary>
    /// ExecutionServiceFactory を初期化します。
    /// </summary>
    public ExecutionServiceFactory(
        ContinuousExecutionService continuous,
        ImmediateExitService immediate,
        TimedExitExecutionService timedExit,
        ExceptionExecutionService exception)
    {
        _continuous = continuous;
        _immediate = immediate;
        _timedExit = timedExit;
        _exception = exception;
    }

    /// <summary>
    /// 実行モードに応じたサービスを取得します。
    /// </summary>
    /// <param name="mode">実行モードです。</param>
    /// <returns>実行サービスを返します。</returns>
    public IExecutionService Create(ExecutionMode mode)
    {
        return mode switch
        {
            ExecutionMode.Continuous => _continuous,
            ExecutionMode.Immediate => _immediate,
            ExecutionMode.TimedExit => _timedExit,
            ExecutionMode.Exception => _exception,
            _ => _immediate
        };
    }
}