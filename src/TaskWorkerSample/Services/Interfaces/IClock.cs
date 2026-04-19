namespace TaskWorkerSample.Services.Interfaces;

/// <summary>
/// 現在時刻を提供するインターフェースです。
/// </summary>
public interface IClock
{
    /// <summary>
    /// 現在時刻を返します。
    /// </summary>
    DateTimeOffset Now { get; }
}