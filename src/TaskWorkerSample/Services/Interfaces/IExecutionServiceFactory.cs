using TaskWorkerSample.Common;

namespace TaskWorkerSample.Services.Interfaces;

/// <summary>
/// 実行モードに応じた実行サービスを生成します。
/// </summary>
public interface IExecutionServiceFactory
{
    /// <summary>
    /// 実行モードに応じたサービスを取得します。
    /// </summary>
    /// <param name="mode">実行モードです。</param>
    /// <returns>実行サービスです。</returns>
    public IExecutionService Create(ExecutionMode mode);
}