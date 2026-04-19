using TaskWorkerSample.Models;

namespace TaskWorkerSample.Services.Interfaces;

/// <summary>
/// 言語設定に応じて Person モデルを生成します。
/// </summary>
public interface IPersonFactory
{
    /// <summary>
    /// 指定言語に対応する Person を生成します。
    /// </summary>
    /// <param name="language">言語コードです。</param>
    /// <returns>Person インスタンスです。</returns>
    Person Create(string language);
}