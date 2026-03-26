using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using UserTaskManagement.Application.DrivenPorts.DomainModel;
using UserTaskManagement.Application.Errors;
using UserTaskManagement.Application.IntegrationEvents;

namespace UserTaskManagement.Application.UseCases.Mediatr.DeleteUserTask;

public static partial class DeleteUserTaskUseCase
{
    public sealed class Handler : IRequestHandler<Command, Result>
    {
        private readonly IUserTaskRepository _userTaskRepository;
        private readonly IOutboxRepository _outboxRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<Handler> _logger;

        public Handler(
            IUserTaskRepository userTaskRepository,
            IUnitOfWork unitOfWork,
            IOutboxRepository outboxRepository,
            ILogger<Handler> logger
        )
        {
            _userTaskRepository = userTaskRepository;
            _unitOfWork = unitOfWork;
            _outboxRepository = outboxRepository;
            _logger = logger;
        }
        
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var userTask = await _userTaskRepository.GetById(request.UserTaskId, cancellationToken);

            if (userTask == null)
            {
                _logger.LogFindUserTaskFailed(
                    request.UserTaskId
                );

                return Result.Fail(new NotFoundError());
            }
            
            _userTaskRepository.Remove(userTask);
            
            var userTaskCreated = UserTaskRemovedEvent.Create(userTask.Id, userTask.UserId);
            
            await _outboxRepository.Add(userTaskCreated, cancellationToken);
            await _unitOfWork.SaveChanges(cancellationToken);

            return Result.Ok();
        }
    }
}

