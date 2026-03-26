using FluentAssertions;
using UserTaskManagement.Application.UseCases.Mediatr.GetUserTasks;
using UserTaskManagement.Domain.Models.UserTask;
using Xunit;

namespace UserTaskManagement.Tests.MediatrTests.GetUserTasksTests;

public partial class GetUserTasksTests
{
    [Fact]
    public void ToQueryResult_ShouldMapDomainTasksToDtoItems()
    {
        // Arrange
        var domainTasks = new List<UserTask>
        {
            Constants.CreateUserTask(
                Constants.ValidTaskId1,
                Constants.ValidUserId1,
                Constants.ValidTitle1,
                Constants.ValidDescription1
            ),
            Constants.CreateUserTask(
                Constants.ValidTaskId2,
                Constants.ValidUserId2,
                Constants.ValidTitle2,
                Constants.ValidDescription2,
                UserTaskStatus.Completed
            )
        }.OrderBy(x => x.Id);

        // Act
        var result = GetUserTasksUseCase.ToQueryResult(domainTasks);

        // Assert
        result.UserTasks.Should().HaveCount(2);

        var item1 = result.UserTasks.First(x => x.TaskId == Constants.ValidTaskId1);
        
        item1.UserId.Should().Be(Constants.ValidUserId1);
        item1.Title.Should().Be(Constants.ValidTitle1);
        item1.Description.Should().Be(Constants.ValidDescription1);
        item1.Status.Should().Be(nameof(UserTaskStatus.New));
        item1.CreatedAt.Should().Be(Constants.FixedCreatedAt);
        item1.UpdatedAt.Should().Be(Constants.FixedUpdatedAt);

        var item2 = result.UserTasks.First(x => x.TaskId == Constants.ValidTaskId2);
        
        item2.Status.Should().Be(nameof(UserTaskStatus.Completed));
        item2.Title.Should().Be(Constants.ValidTitle2);
        item2.Description.Should().Be(Constants.ValidDescription2);
        item2.Status.Should().Be(nameof(UserTaskStatus.Completed));
        item2.CreatedAt.Should().Be(Constants.FixedCreatedAt);
        item2.UpdatedAt.Should().Be(Constants.FixedUpdatedAt);
    }
}
