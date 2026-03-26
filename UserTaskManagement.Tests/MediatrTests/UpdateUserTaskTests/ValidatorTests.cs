using FluentAssertions;
using UserTaskManagement.Application.UseCases.Mediatr.UpdateUserTask;
using Xunit;

namespace UserTaskManagement.Tests.MediatrTests.UpdateUserTaskTests;

public partial class UpdateUserTaskTests
{
    [Theory]
    [MemberData(nameof(GetUserTaskIdData), MemberType = typeof(UpdateUserTaskTests))]
    public void Validate_ShouldRespect_UserTaskIdRules(
        long userTaskId,
        bool expected,
        string expectedMessage)
    {
        // Arrange
        var sut = new UpdateUserTaskUseCase.CommandValidator();
        var command = CreateValidCommand(userTaskId: userTaskId);

        // Act
        var result = sut.Validate(command);

        // Assert
        result.IsValid.Should().Be(expected);

        if (!expected)
        {
            result.Errors.Should().ContainSingle(e => e.ErrorMessage == expectedMessage);
        }
    }
}
