using FluentResults;
using MediatR;
using UserTaskManagement.Application.DrivenPorts.DomainModel;

namespace UserTaskManagement.Application.UseCases.Mediatr.GetUserTasks;

public static partial class GetUserTasksUseCase
{
    public sealed class Handler : IRequestHandler<Query, Result<QueryResult>>
    {
        private readonly IUserTaskRepository _userTaskRepository;

        public Handler(IUserTaskRepository userTaskRepository)
        {
            _userTaskRepository = userTaskRepository;
        }
        
        public async Task<Result<QueryResult>> Handle(Query request, CancellationToken cancellationToken)
        {
            var userTask = await _userTaskRepository.GetAll(cancellationToken);

            if (userTask.Count == 0)
            {
                return new QueryResult([]);
            }

            var orderedUserTask = userTask.OrderBy(x => x.Id);
            
            return ToQueryResult(orderedUserTask);
        }
    }
}

