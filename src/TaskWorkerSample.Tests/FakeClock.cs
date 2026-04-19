using TaskWorkerSample.Services.Interfaces;

namespace TaskWorkerSample.Tests.TestDoubles;

/// <summary>
/// 呼び出しごとに時刻を進めるテスト用時計です。
/// </summary>
public sealed class FakeClock : IClock
{
    private DateTimeOffset _current;
    private readonly TimeSpan _step;

    /// <summary>
    /// テスト用時計を初期化します。
    /// </summary>
    /// <param name="start">開始時刻です。</param>
    /// <param name="step">Now を呼ぶたびに進める時間です。</param>
    public FakeClock(DateTimeOffset start, TimeSpan step)
    {
        _current = start;
        _step = step;
    }

    /// <summary>
    /// 現在時刻を返します。
    /// 呼び出しごとに一定時間進みます。
    /// </summary>
    public DateTimeOffset Now
    {
        get
        {
            DateTimeOffset value = _current;
            _current = _current.Add(_step);
            return value;
        }
    }
}