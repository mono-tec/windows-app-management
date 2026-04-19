namespace TaskWorkerSample.Common;

/// <summary>
/// アプリケーション終了コードを定義します。
/// </summary>
public static class ExitCodes
{
    /// <summary>
    /// 正常終了を表します。
    /// </summary>
    public const int Success = 0;

    /// <summary>
    /// 一般的な異常終了を表します。
    /// </summary>
    public const int Failure = 1;

    /// <summary>
    /// 多重起動により処理を中止したことを表します。
    /// </summary>
    public const int Duplicate = 2;
}