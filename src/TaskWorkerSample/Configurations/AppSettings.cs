namespace TaskWorkerSample.Configurations;

/// <summary>
/// アプリケーション全体で使用する設定値を表します。
/// </summary>
public sealed class AppSettings
{
    /// <summary>
    /// 実行モードを表します。
    /// </summary>
    public string ExecutionMode { get; set; } = "Immediate";

    /// <summary>
    /// 挨拶に使用する言語を表します。
    /// </summary>
    public string Language { get; set; } = "ja";

    /// <summary>
    /// 時間指定終了処理で使用する制限時間（分）を表します。
    /// </summary>
    public int TimeoutMinutes { get; set; } = 3;

    /// <summary>
    /// 継続処理の実行間隔（秒）を表します。
    /// </summary>
    public int IntervalSeconds { get; set; } = 10;

    /// <summary>
    /// 多重起動防止に使用する Mutex キーを表します。
    /// </summary>
    public string MutexKey { get; set; } = "Global\\SAMPLE_TaskWorkerSample";

}