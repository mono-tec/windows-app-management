namespace TaskWorkerSample.Models;

/// <summary>
/// 英語の挨拶を提供するモデルです。
/// </summary>
public sealed class AmericanPerson : Person
{
    /// <inheritdoc />
    public override string GetGreeting() => "Hello";
}