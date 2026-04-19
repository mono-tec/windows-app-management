namespace TaskWorkerSample.Models;

/// <summary>
/// 日本語の挨拶を提供するモデルです。
/// </summary>
public sealed class JapanesePerson : Person
{
    /// <inheritdoc />
    public override string GetGreeting() => "こんにちは";
}