namespace TaskWorkerSample.Common;

/// <summary>
/// 実行モードの種類を表します。
/// </summary>
public enum ExecutionMode
{
    Continuous,
    Immediate,
    TimedExit,
    Exception
}