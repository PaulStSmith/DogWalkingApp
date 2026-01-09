using DogWalkingApp.Domain.Entities;
using FluentAssertions;

namespace DogWalkingApp.Tests.Domain;

/// <summary>
/// Tests for the Dog domain entity.
/// </summary>
public class DogTests
{
    /// <summary>
    /// Tests that a Dog initializes with default values.
    /// </summary>
    [Fact]
    public void Dog_ShouldInitializeWithDefaults()
    {
        var dog = new Dog();

        dog.Id.Should().Be(0);
        dog.ClientId.Should().Be(0);
        dog.Name.Should().Be(string.Empty);
        dog.Breed.Should().Be(string.Empty);
        dog.Age.Should().Be(0);
        dog.CreatedDate.Should().Be(default(DateTime));
        dog.Walks.Should().NotBeNull().And.BeEmpty();
    }

    /// <summary>
    /// Tests that a Dog accepts valid property values.
    /// </summary>
    [Fact]
    public void Dog_ShouldAcceptValidProperties()
    {
        var createdDate = DateTime.UtcNow;
        var dog = new Dog
        {
            Id = 1,
            ClientId = 123,
            Name = "Buddy",
            Breed = "Golden Retriever",
            Age = 5,
            CreatedDate = createdDate
        };

        dog.Id.Should().Be(1);
        dog.ClientId.Should().Be(123);
        dog.Name.Should().Be("Buddy");
        dog.Breed.Should().Be("Golden Retriever");
        dog.Age.Should().Be(5);
        dog.CreatedDate.Should().Be(createdDate);
    }

    /// <summary>
    /// Tests that a Dog allows age updates.
    /// </summary>
    [Fact]
    public void Dog_ShouldAllowAgeUpdates()
    {
        var dog = new Dog
        {
            Name = "Max",
            Age = 2
        };

        dog.Age.Should().Be(2);

        dog.Age = 3;

        dog.Age.Should().Be(3);
    }


    /// <summary>
    /// Tests that a Dog maintains a collection of walks.
    /// </summary>
    [Fact]
    public void Dog_ShouldMaintainWalkCollection()
    {
        var dog = new Dog { Name = "Buddy" };
        var walk1 = new Walk { DogId = 1, WalkDateTime = DateTime.UtcNow };
        var walk2 = new Walk { DogId = 1, WalkDateTime = DateTime.UtcNow.AddHours(-1) };

        dog.Walks.Add(walk1);
        dog.Walks.Add(walk2);

        dog.Walks.Should().HaveCount(2);
        dog.Walks.Should().Contain(walk1);
        dog.Walks.Should().Contain(walk2);
    }

    /// <summary>
    /// Tests that a Dog accepts valid age values using theory data.
    /// </summary>
    /// <param name="age">The age value to test.</param>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(50)]
    public void Dog_ShouldAcceptValidAgeValues(int age)
    {
        var dog = new Dog { Age = age };
        
        dog.Age.Should().Be(age);
    }

    #region Validation Tests

    /// <summary>
    /// Tests that a valid Dog passes validation.
    /// </summary>
    [Fact]
    public void Dog_IsValid_ShouldReturnTrueForValidDog()
    {
        var dog = new Dog
        {
            ClientId = 1,
            Name = "Buddy",
            Breed = "Golden Retriever",
            Age = 5
        };

        var result = dog.IsValid();

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that a Dog with empty name fails validation.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Dog_IsValid_ShouldReturnFalseForEmptyName(string name)
    {
        var dog = new Dog
        {
            ClientId = 1,
            Name = name,
            Breed = "Golden Retriever",
            Age = 5
        };

        var result = dog.IsValid();

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain("Dog name is required.");
    }

    /// <summary>
    /// Tests that a Dog with name exceeding maximum length fails validation.
    /// </summary>
    [Fact]
    public void Dog_IsValid_ShouldReturnFalseForNameExceedingMaxLength()
    {
        var dog = new Dog
        {
            ClientId = 1,
            Name = new string('a', 51), // 51 characters
            Breed = "Golden Retriever",
            Age = 5
        };

        var result = dog.IsValid();

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain("Dog name cannot exceed 50 characters.");
    }

    /// <summary>
    /// Tests that a Dog with empty breed fails validation.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Dog_IsValid_ShouldReturnFalseForEmptyBreed(string breed)
    {
        var dog = new Dog
        {
            ClientId = 1,
            Name = "Buddy",
            Breed = breed,
            Age = 5
        };

        var result = dog.IsValid();

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain("Dog breed is required.");
    }

    /// <summary>
    /// Tests that a Dog with breed exceeding maximum length fails validation.
    /// </summary>
    [Fact]
    public void Dog_IsValid_ShouldReturnFalseForBreedExceedingMaxLength()
    {
        var dog = new Dog
        {
            ClientId = 1,
            Name = "Buddy",
            Breed = new string('a', 51), // 51 characters
            Age = 5
        };

        var result = dog.IsValid();

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain("Dog breed cannot exceed 50 characters.");
    }

    /// <summary>
    /// Tests that a Dog with invalid age fails validation.
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(31)]
    [InlineData(100)]
    public void Dog_IsValid_ShouldReturnFalseForInvalidAge(int age)
    {
        var dog = new Dog
        {
            ClientId = 1,
            Name = "Buddy",
            Breed = "Golden Retriever",
            Age = age
        };

        var result = dog.IsValid();

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain("Dog age must be between 1 and 30 years.");
    }

    /// <summary>
    /// Tests that a Dog with valid age passes validation.
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(15)]
    [InlineData(30)]
    public void Dog_IsValid_ShouldReturnTrueForValidAge(int age)
    {
        var dog = new Dog
        {
            ClientId = 1,
            Name = "Buddy",
            Breed = "Golden Retriever",
            Age = age
        };

        var result = dog.IsValid();

        result.IsValid.Should().BeTrue();
    }

    /// <summary>
    /// Tests that a Dog with invalid client ID fails validation.
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Dog_IsValid_ShouldReturnFalseForInvalidClientId(int clientId)
    {
        var dog = new Dog
        {
            ClientId = clientId,
            Name = "Buddy",
            Breed = "Golden Retriever",
            Age = 5
        };

        var result = dog.IsValid();

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain("Valid client ID is required.");
    }

    /// <summary>
    /// Tests that multiple validation errors are returned correctly.
    /// </summary>
    [Fact]
    public void Dog_IsValid_ShouldReturnMultipleErrors()
    {
        var dog = new Dog
        {
            ClientId = 0, // Invalid client ID
            Name = "", // Empty name
            Breed = "", // Empty breed
            Age = 0 // Invalid age
        };

        var result = dog.IsValid();

        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(4);
        result.Errors.Should().Contain("Dog name is required.");
        result.Errors.Should().Contain("Dog breed is required.");
        result.Errors.Should().Contain("Dog age must be between 1 and 30 years.");
        result.Errors.Should().Contain("Valid client ID is required.");
    }

    #endregion
}