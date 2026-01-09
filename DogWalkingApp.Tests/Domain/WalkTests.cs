using DogWalkingApp.Domain.Entities;
using FluentAssertions;

namespace DogWalkingApp.Tests.Domain;

/// <summary>
/// Contains unit tests for the <see cref="Walk"/> entity.
/// </summary>
public class WalkTests
{
    /// <summary>
    /// Verifies that a new <see cref="Walk"/> instance initializes with default values.
    /// </summary>
    [Fact]
    public void Walk_ShouldInitializeWithDefaults()
    {
        var walk = new Walk();

        walk.Id.Should().Be(0);
        walk.ClientId.Should().Be(0);
        walk.DogId.Should().Be(0);
        walk.WalkDateTime.Should().Be(default);
        walk.DurationMinutes.Should().Be(0);
        walk.Notes.Should().BeNull();
        walk.CreatedDate.Should().Be(default);
    }

    /// <summary>
    /// Verifies that a <see cref="Walk"/> instance accepts and retains valid property values.
    /// </summary>
    [Fact]
    public void Walk_ShouldAcceptValidProperties()
    {
        var walkDateTime = DateTime.UtcNow;
        var createdDate = DateTime.UtcNow.AddMinutes(-5);
        
        var walk = new Walk
        {
            Id = 1,
            ClientId = 123,
            DogId = 456,
            WalkDateTime = walkDateTime,
            DurationMinutes = 30,
            Notes = "Great walk in the park",
            CreatedDate = createdDate
        };

        walk.Id.Should().Be(1);
        walk.ClientId.Should().Be(123);
        walk.DogId.Should().Be(456);
        walk.WalkDateTime.Should().Be(walkDateTime);
        walk.DurationMinutes.Should().Be(30);
        walk.Notes.Should().Be("Great walk in the park");
        walk.CreatedDate.Should().Be(createdDate);
    }

    /// <summary>
    /// Verifies that the <see cref="Walk.Notes"/> property can be set to null.
    /// </summary>
    [Fact]
    public void Walk_ShouldAllowNullNotes()
    {
        var walk = new Walk
        {
            ClientId = 1,
            DogId = 1,
            WalkDateTime = DateTime.UtcNow,
            DurationMinutes = 15,
            Notes = null
        };

        walk.Notes.Should().BeNull();
    }

    /// <summary>
    /// Verifies that the <see cref="Walk.Notes"/> property can be set to an empty string.
    /// </summary>
    [Fact]
    public void Walk_ShouldAllowEmptyNotes()
    {
        var walk = new Walk
        {
            ClientId = 1,
            DogId = 1,
            WalkDateTime = DateTime.UtcNow,
            DurationMinutes = 15,
            Notes = string.Empty
        };

        walk.Notes.Should().Be(string.Empty);
    }


    /// <summary>
    /// Verifies that the <see cref="Walk.DurationMinutes"/> property accepts various valid duration values.
    /// </summary>
    /// <param name="durationMinutes">The duration in minutes to test.</param>
    [Theory]
    [InlineData(5)]
    [InlineData(30)]
    [InlineData(60)]
    [InlineData(120)]
    public void Walk_ShouldAcceptValidDurations(int durationMinutes)
    {
        var walk = new Walk
        {
            DurationMinutes = durationMinutes
        };

        walk.DurationMinutes.Should().Be(durationMinutes);
    }

    /// <summary>
    /// Verifies that the <see cref="Walk.WalkDateTime"/> property can be set to a future date.
    /// </summary>
    [Fact]
    public void Walk_ShouldAllowFutureDates()
    {
        var futureDate = DateTime.UtcNow.AddDays(1);
        var walk = new Walk
        {
            WalkDateTime = futureDate
        };

        walk.WalkDateTime.Should().Be(futureDate);
    }

    /// <summary>
    /// Verifies that the <see cref="Walk.WalkDateTime"/> property can be set to a past date.
    /// </summary>
    [Fact]
    public void Walk_ShouldAllowPastDates()
    {
        var pastDate = DateTime.UtcNow.AddDays(-1);
        var walk = new Walk
        {
            WalkDateTime = pastDate
        };

        walk.WalkDateTime.Should().Be(pastDate);
    }

    #region Validation Tests

    /// <summary>
    /// Tests that a valid Walk passes validation.
    /// </summary>
    [Fact]
    public void Walk_IsValid_ShouldReturnTrueForValidWalk()
    {
        var walk = new Walk
        {
            ClientId = 1,
            DogId = 1,
            WalkDateTime = DateTime.Now,
            DurationMinutes = 30,
            Notes = "Great walk in the park"
        };

        var result = walk.IsValid();

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that a Walk with invalid client ID fails validation.
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Walk_IsValid_ShouldReturnFalseForInvalidClientId(int clientId)
    {
        var walk = new Walk
        {
            ClientId = clientId,
            DogId = 1,
            WalkDateTime = DateTime.Now,
            DurationMinutes = 30
        };

        var result = walk.IsValid();

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain("Valid client ID is required.");
    }

    /// <summary>
    /// Tests that a Walk with invalid dog ID fails validation.
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Walk_IsValid_ShouldReturnFalseForInvalidDogId(int dogId)
    {
        var walk = new Walk
        {
            ClientId = 1,
            DogId = dogId,
            WalkDateTime = DateTime.Now,
            DurationMinutes = 30
        };

        var result = walk.IsValid();

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain("Valid dog ID is required.");
    }

    /// <summary>
    /// Tests that a Walk with future date beyond limit fails validation.
    /// </summary>
    [Fact]
    public void Walk_IsValid_ShouldReturnFalseForFutureDateBeyondLimit()
    {
        var walk = new Walk
        {
            ClientId = 1,
            DogId = 1,
            WalkDateTime = DateTime.Now.AddDays(2), // More than 1 day in future
            DurationMinutes = 30
        };

        var result = walk.IsValid();

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain("Walk date/time cannot be more than 1 day in the future.");
    }

    /// <summary>
    /// Tests that a Walk with past date beyond limit fails validation.
    /// </summary>
    [Fact]
    public void Walk_IsValid_ShouldReturnFalseForPastDateBeyondLimit()
    {
        var walk = new Walk
        {
            ClientId = 1,
            DogId = 1,
            WalkDateTime = DateTime.Now.AddYears(-2), // More than 1 year in past
            DurationMinutes = 30
        };

        var result = walk.IsValid();

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain("Walk date/time cannot be more than 1 year in the past.");
    }

    /// <summary>
    /// Tests that a Walk with invalid duration fails validation.
    /// </summary>
    [Theory]
    [InlineData(4)]   // Less than 5 minutes
    [InlineData(241)] // More than 240 minutes
    [InlineData(0)]
    [InlineData(-1)]
    public void Walk_IsValid_ShouldReturnFalseForInvalidDuration(int duration)
    {
        var walk = new Walk
        {
            ClientId = 1,
            DogId = 1,
            WalkDateTime = DateTime.Now,
            DurationMinutes = duration
        };

        var result = walk.IsValid();

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain("Walk duration must be between 5 and 240 minutes.");
    }

    /// <summary>
    /// Tests that a Walk with valid duration passes validation.
    /// </summary>
    [Theory]
    [InlineData(5)]
    [InlineData(30)]
    [InlineData(60)]
    [InlineData(120)]
    [InlineData(240)]
    public void Walk_IsValid_ShouldReturnTrueForValidDuration(int duration)
    {
        var walk = new Walk
        {
            ClientId = 1,
            DogId = 1,
            WalkDateTime = DateTime.Now,
            DurationMinutes = duration
        };

        var result = walk.IsValid();

        result.IsValid.Should().BeTrue();
    }

    /// <summary>
    /// Tests that a Walk with notes exceeding maximum length fails validation.
    /// </summary>
    [Fact]
    public void Walk_IsValid_ShouldReturnFalseForNotesExceedingMaxLength()
    {
        var walk = new Walk
        {
            ClientId = 1,
            DogId = 1,
            WalkDateTime = DateTime.Now,
            DurationMinutes = 30,
            Notes = new string('a', 501) // 501 characters
        };

        var result = walk.IsValid();

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain("Walk notes cannot exceed 500 characters.");
    }

    /// <summary>
    /// Tests that a Walk with valid notes passes validation.
    /// </summary>
    [Fact]
    public void Walk_IsValid_ShouldReturnTrueForValidNotes()
    {
        var walk = new Walk
        {
            ClientId = 1,
            DogId = 1,
            WalkDateTime = DateTime.Now,
            DurationMinutes = 30,
            Notes = new string('a', 500) // Exactly 500 characters
        };

        var result = walk.IsValid();

        result.IsValid.Should().BeTrue();
    }

    /// <summary>
    /// Tests that a Walk with null notes passes validation.
    /// </summary>
    [Fact]
    public void Walk_IsValid_ShouldReturnTrueForNullNotes()
    {
        var walk = new Walk
        {
            ClientId = 1,
            DogId = 1,
            WalkDateTime = DateTime.Now,
            DurationMinutes = 30,
            Notes = null
        };

        var result = walk.IsValid();

        result.IsValid.Should().BeTrue();
    }

    /// <summary>
    /// Tests that multiple validation errors are returned correctly.
    /// </summary>
    [Fact]
    public void Walk_IsValid_ShouldReturnMultipleErrors()
    {
        var walk = new Walk
        {
            ClientId = 0, // Invalid client ID
            DogId = 0,    // Invalid dog ID
            WalkDateTime = DateTime.Now.AddYears(-2), // Too far in past
            DurationMinutes = 2 // Invalid duration
        };

        var result = walk.IsValid();

        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(4);
        result.Errors.Should().Contain("Valid client ID is required.");
        result.Errors.Should().Contain("Valid dog ID is required.");
        result.Errors.Should().Contain("Walk date/time cannot be more than 1 year in the past.");
        result.Errors.Should().Contain("Walk duration must be between 5 and 240 minutes.");
    }

    #endregion
}