using FluentAssertions;
using UserTaskManagement.Domain.Errors;
using UserTaskManagement.Domain.Models.UserTask;
using UserTaskManagement.Domain.Rules.UserTask;
using Xunit;

namespace UserTaskManagement.Tests.RulesTests;

public partial class IsAvailableForChangingStatusRuleTests
{
    [Theory]
    [MemberData(nameof(GetValidTransitionsData), MemberType = typeof(IsAvailableForChangingStatusRuleTests))]
    public void Apply_ShouldSuccess_WhenTransitionIsValid(
        UserTaskStatus currentStatus,
        UserTaskStatus newStatus,
        bool expected)
    {
        // Arrange
        var userTask = Constants.CreateUserTask(status: currentStatus);
        var sut = new IsAvailableForChangingStatusRule(newStatus);

        // Act
        var result = sut.Apply(userTask);

        // Assert
        result.IsSuccess.Should().Be(expected);
        result.IsFailed.Should().Be(!expected);
    }

    [Theory]
    [MemberData(nameof(GetInvalidTransitionsData), MemberType = typeof(IsAvailableForChangingStatusRuleTests))]
    public void Apply_ShouldFail_WhenTransitionIsInvalid(
        UserTaskStatus currentStatus,
        UserTaskStatus newStatus)
    {
        // Arrange
        var userTask = Constants.CreateUserTask(status: currentStatus);
        var sut = new IsAvailableForChangingStatusRule(newStatus);

        // Act
        var result = sut.Apply(userTask);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.HasError<UserTaskChangingStatusError>().Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(GetSameStatusData), MemberType = typeof(IsAvailableForChangingStatusRuleTests))]
    public void Apply_ShouldFail_WhenNewStatusEqualsCurrentStatus(UserTaskStatus status)
    {
        // Arrange
        var userTask = Constants.CreateUserTask(status: status);
        var sut = new IsAvailableForChangingStatusRule(status);

        // Act
        var result = sut.Apply(userTask);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.HasError<UserTaskChangingStatusError>().Should().BeTrue();
    }

    [Fact]
    public void Apply_ShouldSuccess_WhenTransitionFromNewToInProgress()
    {
        // Arrange
        var userTask = Constants.CreateUserTask(status: UserTaskStatus.New);
        var sut = new IsAvailableForChangingStatusRule(UserTaskStatus.InProgress);

        // Act
        var result = sut.Apply(userTask);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Apply_ShouldSuccess_WhenTransitionFromInProgressToCompleted()
    {
        // Arrange
        var userTask = Constants.CreateUserTask(status: UserTaskStatus.InProgress);
        var sut = new IsAvailableForChangingStatusRule(UserTaskStatus.Completed);

        // Act
        var result = sut.Apply(userTask);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Apply_ShouldFail_WhenTransitionFromNewToCompleted_Directly()
    {
        // Arrange
        var userTask = Constants.CreateUserTask(status: UserTaskStatus.New);
        var sut = new IsAvailableForChangingStatusRule(UserTaskStatus.Completed);

        // Act
        var result = sut.Apply(userTask);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.HasError<UserTaskChangingStatusError>().Should().BeTrue();
    }

    [Fact]
    public void Apply_ShouldFail_WhenTaskIsCompleted_AndTryingToChangeToAnyStatus()
    {
        // Arrange
        var userTask = Constants.CreateUserTask(status: UserTaskStatus.Completed);
        var sut = new IsAvailableForChangingStatusRule(UserTaskStatus.New);

        // Act
        var result = sut.Apply(userTask);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.HasError<UserTaskChangingStatusError>().Should().BeTrue();
    }
}
