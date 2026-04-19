using TaskWorkerSample.Models;
using TaskWorkerSample.Services;
using Xunit;

namespace TaskWorkerSample.Tests.Services;

/// <summary>
/// PersonFactory のテストです。
/// </summary>
public sealed class PersonFactoryTests
{
    [Fact]
    public void Create_Should_Return_JapanesePerson_When_LanguageIsJa()
    {
        // Arrange
        var factory = new PersonFactory();

        // Act
        Person result = factory.Create("ja");

        // Assert
        Assert.IsType<JapanesePerson>(result);
        Assert.Equal("こんにちは", result.GetGreeting());
    }

    [Fact]
    public void Create_Should_Return_AmericanPerson_When_LanguageIsEn()
    {
        // Arrange
        var factory = new PersonFactory();

        // Act
        Person result = factory.Create("en");

        // Assert
        Assert.IsType<AmericanPerson>(result);
        Assert.Equal("Hello", result.GetGreeting());
    }

    [Fact]
    public void Create_Should_Return_JapanesePerson_When_LanguageIsUnknown()
    {
        // Arrange
        var factory = new PersonFactory();

        // Act
        Person result = factory.Create("xx");

        // Assert
        Assert.IsType<JapanesePerson>(result);
    }
}