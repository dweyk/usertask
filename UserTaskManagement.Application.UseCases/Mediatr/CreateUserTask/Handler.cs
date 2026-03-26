using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using UserTaskManagement.Application.DrivenPorts.DomainModel;
using UserTaskManagement.Application.DrivenPorts.UserData;
using UserTaskManagement.Application.Errors;
using UserTaskManagement.Application.Extensions;
using UserTaskManagement.Application.IntegrationEvents;
using UserTaskManagement.Domain.Factories;

namespace UserTaskManagement.Application.UseCases.Mediatr.CreateUserTask;

public static partial class CreateUserTaskUseCase
{
    public sealed class Handler : IRequestHandler<Command, Result<CommandResult>>
    {
        private readonly IUserTaskFactory _factory;
        private readonly IUserDataPort _userDataPort;
        private readonly IUserTaskRepository _userTaskRepository;
        private readonly IOutboxRepository _outboxRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<Handler> _logger;

        public Handler(
            IUserTaskFactory factory,
            IUserDataPort userDataPort,
            IUserTaskRepository userTaskRepository,
            IUnitOfWork unitOfWork,
            IOutboxRepository outboxRepository,
            ILogger<Handler> logger
        )
        {
            _factory = factory;
            _userDataPort = userDataPort;
            _userTaskRepository = userTaskRepository;
            _unitOfWork = unitOfWork;
            _outboxRepository = outboxRepository;
            _logger = logger;
        }
        
        public async Task<Result<CommandResult>> Handle(Command request, CancellationToken cancellationToken)
        {
            // проверяем, что юзер, на которого назначаем задачу действительно существует
            var userInfoResult = await _userDataPort.GetUserData(request.UserId);

            if (userInfoResult.IsFailed)
            {
                _logger.LogFindUserInfoFailed(
                    request.UserId,
                    userInfoResult.ToErrorString()
                );

                if (userInfoResult.HasError<NotFoundError>())
                {
                    return new NotFoundError();
                }

                return Result.Fail("Не удалось создать задачу");
            }
            
            var createUserTaskResult = _factory.CreateUserTask(
                request.UserId, 
                request.Title,
                request.Description
            );

            if (createUserTaskResult.IsFailed)
            {
                _logger.LogCreatingUserTaskFailed(createUserTaskResult.ToErrorString());
                
                return Result.Fail("Не удалось создать задачу");
            }
            
            var userTask = createUserTaskResult.Value;

            using var transactionScope = _unitOfWork.BeginTransaction();
            
            await _userTaskRepository.Add(createUserTaskResult.Value);
            // для получения id таски, но в рамках транзакции для атомарности
            await _unitOfWork.SaveChanges(cancellationToken); 
            
            var userTaskCreated = UserTaskCreatedEvent.Create(
                userTask.Id,
                userTask.UserId,
                userTask.Title,
                userTask.Description
            );
            
            await _outboxRepository.Add(userTaskCreated, cancellationToken);
            await _unitOfWork.Commit(transactionScope, cancellationToken);
            
            return new CommandResult(userTask.Id);
        }
    }
}

