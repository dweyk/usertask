using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using UserTaskManagement.Application;
using UserTaskManagement.Application.DrivenPorts.DomainModel;
using UserTaskManagement.Application.Errors;
using UserTaskManagement.Application.IntegrationEvents;
using UserTaskManagement.Application.UseCases.Mediatr.DeleteUserTask;
using UserTaskManagement.Domain.Models.UserTask;
using UserTaskManagement.Tests.Utils;
using Xunit;

namespace UserTaskManagement.Tests.MediatrTests.DeleteUserTaskTests;

public partial class DeleteUserTaskTests
{
    [Theory, AutoMoqData]
    public async Task Handle_ShouldSuccess(
        [Frozen] Mock<IUserTaskRepository> userTaskRepository,
        [Frozen] Mock<IOutboxRepository> outboxRepository,
        [Frozen] Mock<IUnitOfWork> unitOfWork,
        DeleteUserTaskUseCase.Handler handler
    )
    {
        // Arrange
        var command = CreateValidCommand();
        var userTask = Constants.CreateUserTask();

        userTaskRepository
            .Setup(
                x => x.GetById(
                    command.UserTaskId,
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(userTask);

        outboxRepository
            .Setup(
                x => x.Add(
                    It.IsAny<UserTaskRemovedEvent>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .Returns(Task.CompletedTask);

        unitOfWork
            .Setup(x => x.SaveChanges(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await handler.Handle(
            command,
            CancellationToken.None
        );

        // Assert
        result.IsSuccess.Should().BeTrue();

        userTaskRepository.Verify(
            x => x.Remove(userTask),
            Times.Once
        );

        outboxRepository.Verify(
            x => x.Add(
                It.Is<UserTaskRemovedEvent>(
                    e =>
                        e.TaskId == userTask.Id &&
                        e.UserId == userTask.UserId
                ),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );

        unitOfWork.Verify(
            x => x.SaveChanges(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Theory, AutoMoqData]
    public async Task Handle_ShouldFail_WhenUserTaskNotFound(
        [Frozen] Mock<IUserTaskRepository> userTaskRepository,
        DeleteUserTaskUseCase.Handler handler
    )
    {
        // Arrange
        var command = CreateValidCommand();

        userTaskRepository
            .Setup(
                x => x.GetById(
                    command.UserTaskId,
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync((UserTask?)null);

        // Act
        var result = await handler.Handle(
            command,
            CancellationToken.None
        );

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.HasError<NotFoundError>().Should().BeTrue();
    }
}
