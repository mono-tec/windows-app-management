using TaskWorkerSample.Models;
using TaskWorkerSample.Services.Interfaces;

namespace TaskWorkerSample.Services;

/// <summary>
/// 言語設定に応じて Person モデルを生成する実装です。
/// </summary>
public sealed class PersonFactory : IPersonFactory
{
    /// <inheritdoc />
    public Person Create(string language)
    {
        return language.ToLowerInvariant() switch
        {
            "ja" => new JapanesePerson(),
            "en" => new AmericanPerson(),
            _ => new JapanesePerson()
        };
    }
}