namespace TaskWorkerSample.Models;

/// <summary>
/// 挨拶を提供する人間モデルの基底クラスです。
/// </summary>
public abstract class Person
{
    /// <summary>
    /// 挨拶メッセージを返します。
    /// </summary>
    /// <returns>挨拶文字列です。</returns>
    public abstract string GetGreeting();
}