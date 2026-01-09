using DogWalkingApp.Domain.Entities;
using FluentAssertions;

namespace DogWalkingApp.Tests.Domain;

/// <summary>
/// Contains unit tests for the Client entity.
/// </summary>
public class ClientTests
{
    /// <summary>
    /// Tests that a Client initializes with default values.
    /// </summary>
    [Fact]
    public void Client_ShouldInitializeWithDefaults()
    {
        var client = new Client();

        client.Id.Should().Be(0);
        client.Name.Should().Be(string.Empty);
        client.Phone.Should().Be(string.Empty);
        client.CreatedDate.Should().Be(default);
        client.Dogs.Should().NotBeNull().And.BeEmpty();
        client.Walks.Should().NotBeNull().And.BeEmpty();
    }

    /// <summary>
    /// Tests that a Client accepts and retains valid property values.
    /// </summary>
    [Fact]
    public void Client_ShouldAcceptValidProperties()
    {
        var createdDate = DateTime.UtcNow;
        var client = new Client
        {
            Id = 1,
            Name = "John Doe",
            Phone = "555-0123",
            CreatedDate = createdDate
        };

        client.Id.Should().Be(1);
        client.Name.Should().Be("John Doe");
        client.Phone.Should().Be("555-0123");
        client.CreatedDate.Should().Be(createdDate);
    }


    /// <summary>
    /// Tests that a Client maintains its navigation properties for Dogs and Walks.
    /// </summary>
    [Fact]
    public void Client_ShouldMaintainNavigationProperties()
    {
        var client = new Client { Name = "Test Client" };
        var dog1 = new Dog { Name = "Buddy", ClientId = 1 };
        var dog2 = new Dog { Name = "Max", ClientId = 1 };
        var walk1 = new Walk { ClientId = 1, DogId = 1 };

        client.Dogs.Add(dog1);
        client.Dogs.Add(dog2);
        client.Walks.Add(walk1);

        client.Dogs.Should().HaveCount(2);
        client.Dogs.Should().Contain(dog1);
        client.Dogs.Should().Contain(dog2);
        client.Walks.Should().HaveCount(1);
        client.Walks.Should().Contain(walk1);
    }

    #region Validation Tests

    /// <summary>
    /// Tests that a valid Client passes validation.
    /// </summary>
    [Fact]
    public void Client_IsValid_ShouldReturnTrueForValidClient()
    {
        var client = new Client
        {
            Name = "John Doe",
            Phone = "555-123-4567"
        };

        var result = client.IsValid();

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that a Client with empty name fails validation.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Client_IsValid_ShouldReturnFalseForEmptyName(string name)
    {
        var client = new Client
        {
            Name = name,
            Phone = "555-123-4567"
        };

        var result = client.IsValid();

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain("Client name is required.");
    }

    /// <summary>
    /// Tests that a Client with name exceeding maximum length fails validation.
    /// </summary>
    [Fact]
    public void Client_IsValid_ShouldReturnFalseForNameExceedingMaxLength()
    {
        var client = new Client
        {
            Name = new string('a', 101), // 101 characters
            Phone = "555-123-4567"
        };

        var result = client.IsValid();

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain("Client name cannot exceed 100 characters.");
    }

    /// <summary>
    /// Tests that a Client with empty phone fails validation.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Client_IsValid_ShouldReturnFalseForEmptyPhone(string phone)
    {
        var client = new Client
        {
            Name = "John Doe",
            Phone = phone
        };

        var result = client.IsValid();

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain("Phone number is required.");
    }

    /// <summary>
    /// Tests that a Client with invalid phone format fails validation.
    /// </summary>
    [Theory]
    [InlineData("123")]
    [InlineData("abcdefghij")]
    [InlineData("555-555-555")]
    [InlineData("555-555-555555")]
    public void Client_IsValid_ShouldReturnFalseForInvalidPhoneFormat(string phone)
    {
        var client = new Client
        {
            Name = "John Doe",
            Phone = phone
        };

        var result = client.IsValid();

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain("Phone number format is invalid.");
    }

    /// <summary>
    /// Tests that a Client with valid phone formats passes validation.
    /// </summary>
    [Theory]
    [InlineData("5551234567")]
    [InlineData("555-123-4567")]
    [InlineData("(555) 123-4567")]
    [InlineData("+1 555 123 4567")]
    [InlineData("15551234567")]
    public void Client_IsValid_ShouldReturnTrueForValidPhoneFormats(string phone)
    {
        var client = new Client
        {
            Name = "John Doe",
            Phone = phone
        };

        var result = client.IsValid();

        result.IsValid.Should().BeTrue();
    }

    /// <summary>
    /// Tests that multiple validation errors are returned correctly.
    /// </summary>
    [Fact]
    public void Client_IsValid_ShouldReturnMultipleErrors()
    {
        var client = new Client
        {
            Name = "", // Empty name
            Phone = "" // Empty phone
        };

        var result = client.IsValid();

        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
        result.Errors.Should().Contain("Client name is required.");
        result.Errors.Should().Contain("Phone number is required.");
    }

    #endregion
}