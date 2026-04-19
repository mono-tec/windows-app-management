using TaskWorkerSample.Services.Interfaces;

namespace TaskWorkerSample.Services;

/// <summary>
/// システム時刻を提供する実装です。
/// </summary>
public sealed class SystemClock : IClock
{
    /// <inheritdoc />
    public DateTimeOffset Now => DateTimeOffset.Now;
}