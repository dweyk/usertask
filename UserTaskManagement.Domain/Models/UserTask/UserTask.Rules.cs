using UserTaskManagement.Domain.Rules.UserTask;
using UserTaskManagement.Domain.SeedWorks.EntityRule;

namespace UserTaskManagement.Domain.Models.UserTask;

public sealed partial class UserTask
{
    public static class Rules
    {
        public static IEntityRule<UserTask> IsAvailableForChangingStatus(UserTaskStatus newStatus)
        {
            return new IsAvailableForChangingStatusRule(newStatus);
        }
    }
}
