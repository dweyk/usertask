using FluentAssertions;
using UserTaskManagement.Application.UseCases.Mediatr.DeleteUserTask;
using Xunit;

namespace UserTaskManagement.Tests.MediatrTests.DeleteUserTaskTests;

public partial class DeleteUserTaskTests
{
    [Theory]
    [MemberData(nameof(GetUserTaskIdData), MemberType = typeof(DeleteUserTaskTests))]
    public void Validate_ShouldRespect_UserTaskIdRules(
        long userTaskId,
        bool expected,
        string expectedMessage)
    {
        // Arrange
        var sut = new DeleteUserTaskUseCase.CommandValidator();
        var command = CreateValidCommand() with { UserTaskId = userTaskId };

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
