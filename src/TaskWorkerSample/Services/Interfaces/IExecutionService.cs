namespace TaskWorkerSample.Services.Interfaces;

/// <summary>
/// 実行モードごとの処理を提供するサービスです。
/// </summary>
public interface IExecutionService
{
    /// <summary>
    /// 処理を非同期に実行します。
    /// </summary>
    /// <param name="cancellationToken">停止要求を通知するトークンです。</param>
    /// <returns>終了コードを返します。</returns>
    Task<int> ExecuteAsync(CancellationToken cancellationToken);
}