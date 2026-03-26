using FluentAssertions;
using UserTaskManagement.Application.UseCases.Mediatr.CreateUserTask;
using Xunit;

namespace UserTaskManagement.Tests.MediatrTests.CreateUserTaskTests;

public partial class CreateUserTaskTests
{
    [Theory]
    [MemberData(nameof(GetUserIdData), MemberType = typeof(CreateUserTaskTests)
    )]
    public void Validate_ShouldRespect_UserIdRules(long userId, bool expected, string expectedMessage)
    {
        // Arrange
        var sut = new CreateUserTaskUseCase.CommandValidator();
        var command = CreateValidCommand() with { UserId = userId };

        // Act
        var result = sut.Validate(command);

        // Assert
        result.IsValid.Should().Be(expected);

        if (!expected)
        {
            result.Errors.Should().ContainSingle(e => e.ErrorMessage == expectedMessage);
        }
    }

    [Theory]
    [MemberData(nameof(GetTitleData), MemberType = typeof(CreateUserTaskTests))]
    public void Validate_ShouldRespect_TitleRules(string title, bool expected, string expectedMessage)
    {
        // Arrange
        var sut = new CreateUserTaskUseCase.CommandValidator();
        var command = CreateValidCommand() with { Title = title };

        // Act
        var result = sut.Validate(command);

        // Assert
        result.IsValid.Should().Be(expected);

        if (!expected)
        {
            result.Errors.Should().Contain(e => e.ErrorMessage == expectedMessage);
        }
    }

    [Theory]
    [MemberData(nameof(GetDescriptionData), MemberType = typeof(CreateUserTaskTests))]
    public void Validate_ShouldRespect_DescriptionRules(string description, bool expected, string expectedMessage
    )
    {
        // Arrange
        var sut = new CreateUserTaskUseCase.CommandValidator();
        var command = CreateValidCommand() with { Description = description };

        // Act
        var result = sut.Validate(command);

        // Assert
        result.IsValid.Should().Be(expected);

        if (!expected)
        {
            result.Errors.Should().Contain(e => e.ErrorMessage == expectedMessage);
        }
    }

    [Fact]
    public void Validate_ShouldSuccess_WhenAllFieldsValid()
    {
        // Arrange
        var sut = new CreateUserTaskUseCase.CommandValidator();
        var command = CreateValidCommand();

        // Act
        var result = sut.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}
