using System.Transactions;
using AutoFixture.Xunit2;
using FluentAssertions;
using FluentResults;
using Moq;
using UserTaskManagement.Application;
using UserTaskManagement.Application.DrivenPorts.DomainModel;
using UserTaskManagement.Application.DrivenPorts.UserData;
using UserTaskManagement.Application.Errors;
using UserTaskManagement.Application.IntegrationEvents;
using UserTaskManagement.Application.UseCases.Mediatr.CreateUserTask;
using UserTaskManagement.Domain.Factories;
using UserTaskManagement.Tests.Utils;
using Xunit;

namespace UserTaskManagement.Tests.MediatrTests.CreateUserTaskTests;

public partial class CreateUserTaskTests
{
    [Theory, AutoMoqData]
public async Task Handle_ShouldSuccess(
    [Frozen] Mock<IUserDataPort> userDataPort,
    [Frozen] Mock<IUserTaskFactory> factory,
    [Frozen] Mock<IUserTaskRepository> userTaskRepository,
    [Frozen] Mock<IOutboxRepository> outboxRepository,
    [Frozen] Mock<IUnitOfWork> unitOfWork,
    CreateUserTaskUseCase.Handler handler)
    {
        // Arrange
        var command = CreateValidCommand();
        var userData = CreateUserData(command);
        
        userDataPort.Setup(x => x.GetUserData(command.UserId))
            .ReturnsAsync(Result.Ok(userData));

        var userTask = CreateUserTask(command);
        
        factory.Setup(x => x.CreateUserTask(command.UserId, command.Title, command.Description))
            .Returns(Result.Ok(userTask));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.UserTaskId.Should().Be(Constants.CreatedUserTaskId);

        // Проверка сохранения задачи в репозиторий
        userTaskRepository.Verify(x => x.Add(userTask), Times.Once);

        // Проверка сохранения события в Outbox
        outboxRepository.Verify(
            x => x.Add(
                It.Is<UserTaskCreatedEvent>(
                    e =>
                        e.TaskId == userTask.Id &&
                        e.UserId == userTask.UserId &&
                        e.Title == userTask.Title &&
                        e.Description == userTask.Description
                ),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );

        // Проверка вызова SaveChanges (для получения ID в рамках транзакции)
        unitOfWork.Verify(x => x.SaveChanges(It.IsAny<CancellationToken>()), Times.Once);

        // Проверка коммита транзакции
        unitOfWork.Verify(x => x.Commit(It.IsAny<TransactionScope>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory, AutoMoqData]
    public async Task Handle_ShouldFail_WhenUserAdapterFailed_NotFoundError(
        [Frozen] Mock<IUserDataPort> userDataPort,
        CreateUserTaskUseCase.Handler handler
    )
    {
        // Arrange
        var command = CreateValidCommand();

        userDataPort.Setup(x => x.GetUserData(command.UserId))
            .ReturnsAsync(Result.Fail(new NotFoundError()));

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
    public async Task Handle_ShouldFail_WhenUserAdapterFailed(
        [Frozen] Mock<IUserDataPort> userDataPort,
        CreateUserTaskUseCase.Handler handler
    )
    {
        // Arrange
        var command = CreateValidCommand();

        userDataPort
            .Setup(x => x.GetUserData(command.UserId))
            .ReturnsAsync(Result.Fail("error"));

        // Act
        var result = await handler.Handle(
            command,
            CancellationToken.None
        );

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.Message == Constants.Error);
    }

    [Theory, AutoMoqData]
    public async Task Handle_ShouldFail_WhenFactoryCreationFails(
        [Frozen] Mock<IUserDataPort> userDataPort,
        [Frozen] Mock<IUserTaskFactory> factory,
        CreateUserTaskUseCase.Handler handler
    )
    {
        // Arrange
        var command = CreateValidCommand();

        userDataPort.Setup(x => x.GetUserData(command.UserId))
            .ReturnsAsync(Result.Ok());

        factory
            .Setup(
                x => x.CreateUserTask(
                    command.UserId,
                    command.Title,
                    command.Description
                )
            )
            .Returns(Result.Fail("error"));

        // Act
        var result = await handler.Handle(
            command,
            CancellationToken.None
        );

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.Message == Constants.Error);
    }
}
