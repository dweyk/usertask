using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using UserTaskManagement.Application;
using UserTaskManagement.Application.DrivenPorts.DomainModel;
using UserTaskManagement.Application.Errors;
using UserTaskManagement.Application.IntegrationEvents;
using UserTaskManagement.Application.UseCases.Mediatr.UpdateUserTask;
using UserTaskManagement.Domain.Models.UserTask;
using UserTaskManagement.Tests.Utils;
using Xunit;

namespace UserTaskManagement.Tests.MediatrTests.UpdateUserTaskTests;

public partial class UpdateUserTaskTests
{
    [Theory, AutoMoqData]
    public async Task Handle_ShouldSuccess(
        [Frozen] Mock<IUserTaskRepository> userTaskRepository,
        [Frozen] Mock<IOutboxRepository> outboxRepository,
        [Frozen] Mock<IUnitOfWork> unitOfWork,
        UpdateUserTaskUseCase.Handler handler
    )
    {
        // Arrange
        var command = CreateValidCommand();
        var userTask = Constants.CreateUserTask(status: UserTaskStatus.New);

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
                    It.IsAny<UserTaskStatusChangedEvent>(),
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
        userTask.Status.Should().Be(command.Status);
        
        outboxRepository.Verify(
            x => x.Add(
                It.Is<UserTaskStatusChangedEvent>(
                    e =>
                        e.TaskId == userTask.Id &&
                        e.UserId == userTask.UserId &&
                        e.OldStatus == nameof(UserTaskStatus.New) &&
                        e.NewStatus == command.Status.ToString() &&
                        e.UpdatedAt == userTask.UpdatedAt
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
        UpdateUserTaskUseCase.Handler handler
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
    
    [Theory, AutoMoqData]
    public async Task Handle_ShouldFail_WhenStatusTransitionInvalid(
        [Frozen] Mock<IUserTaskRepository> userTaskRepository,
        UpdateUserTaskUseCase.Handler handler)
    {
        // Arrange
        var command = CreateValidCommand(status: UserTaskStatus.New);
        var userTask = Constants.CreateUserTask(status: UserTaskStatus.InProgress);

        userTaskRepository
            .Setup(x => x.GetById(command.UserTaskId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(userTask);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.Message == Constants.Error);
    }
}
