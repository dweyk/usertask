using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using UserTaskManagement.Application.DrivenPorts.DomainModel;
using UserTaskManagement.Application.UseCases.Mediatr.GetUserTasks;
using UserTaskManagement.Domain.Models.UserTask;
using UserTaskManagement.Tests.Utils;
using Xunit;

namespace UserTaskManagement.Tests.MediatrTests.GetUserTasksTests;

public partial class GetUserTasksTests
{
    [Theory, AutoMoqData]
    public async Task Handle_ShouldSuccess(
        [Frozen] Mock<IUserTaskRepository> userTaskRepository,
        GetUserTasksUseCase.Handler handler
    )
    {
        // Arrange
        var query = CreateValidQuery();

        var tasks = new List<UserTask>
        {
            Constants.CreateUserTask(
                Constants.ValidTaskId2,
                Constants.ValidUserId2,
                Constants.ValidTitle2,
                Constants.ValidDescription2
            ),
            
            Constants.CreateUserTask(
                Constants.ValidTaskId1,
                Constants.ValidUserId1,
                Constants.ValidTitle1,
                Constants.ValidDescription1
            )
        };

        userTaskRepository
            .Setup(x => x.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(tasks);

        // Act
        var result = await handler.Handle(
            query,
            CancellationToken.None
        );

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.UserTasks.Should().HaveCount(2);
        
        var tasksList = result.Value.UserTasks.ToList();
        tasksList[0].TaskId.Should().Be(Constants.ValidTaskId1);
        tasksList[1].TaskId.Should().Be(Constants.ValidTaskId2);
    }

    [Theory, AutoMoqData]
    public async Task Handle_ShouldReturnEmptyCollection_WhenNoTasksFound(
        [Frozen] Mock<IUserTaskRepository> userTaskRepository,
        GetUserTasksUseCase.Handler handler
    )
    {
        // Arrange
        var query = CreateValidQuery();

        userTaskRepository
            .Setup(x => x.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<UserTask>());

        // Act
        var result = await handler.Handle(
            query,
            CancellationToken.None
        );

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.UserTasks.Should().BeEmpty();
    }
}
