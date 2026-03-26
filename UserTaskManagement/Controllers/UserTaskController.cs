using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UserTaskManagement.Application.Errors;
using UserTaskManagement.Application.Extensions;
using UserTaskManagement.Application.UseCases.Mediatr.CreateUserTask;
using UserTaskManagement.Application.UseCases.Mediatr.DeleteUserTask;
using UserTaskManagement.Application.UseCases.Mediatr.GetUserTasks;
using UserTaskManagement.Application.UseCases.Mediatr.UpdateUserTask;
using UserTaskManagement.Endpoints;

namespace UserTaskManagement.Controllers;

[Produces("application/json")]
[SwaggerTag("Пользовательские задачи")]
[ApiController]
public class UserTaskController : ControllerBase
{
    private readonly ISender _sender;

    public UserTaskController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateUserTaskUseCase.CommandResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    [Route("v1/api/usertask/create")]
    [SwaggerOperation("Создать пользовательскую задачу")]
    public async Task<ActionResult<CreateUserTaskUseCase.CommandResult>> CreateUserTask(
        CreateUserTaskUseCase.Command request
    )
    {
        var result = await _sender.Send(request);

        if (result.IsFailed)
        {
            if (result.HasError<NotFoundError>())
            {
                return StatusCode(StatusCodes.Status404NotFound, new ErrorModel(result.ToErrorString()));
            }
            
            return BadRequest(new ErrorModel(result.ToErrorString()));
        }
        
        return Ok(result.Value);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    [Route("v1/api/usertask/update")]
    [SwaggerOperation("Обновить пользовательскую задачу")]
    public async Task<ActionResult> UpdateUserTask(UpdateUserTaskUseCase.Command request)
    {
        var result = await _sender.Send(request);

        if (result.IsFailed)
        {
            if (result.HasError<NotFoundError>())
            {
                return StatusCode(StatusCodes.Status404NotFound, new ErrorModel(result.ToErrorString()));
            }
            
            return BadRequest(new ErrorModel(result.ToErrorString()));
        }
        
        return Ok();
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    [Route("v1/api/usertask/delete")]
    [SwaggerOperation("Удалить пользовательскую задачу")]
    public async Task<ActionResult> DeleteUserTask(DeleteUserTaskUseCase.Command request)
    {
        var result = await _sender.Send(request);

        if (result.IsFailed)
        {
            if (result.HasError<NotFoundError>())
            {
                return StatusCode(StatusCodes.Status404NotFound, new ErrorModel(result.ToErrorString()));
            }
            
            return BadRequest(new ErrorModel(result.ToErrorString()));
        }
        
        return Ok();
    }
    
    // в задачке не было указано про фильтры, реализую без них
    [HttpGet]
    [ProducesResponseType(typeof(GetUserTasksUseCase.QueryResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [Route("v1/api/usertasks")]
    [SwaggerOperation("Получить пользовательские задачи")]
    public async Task<ActionResult<GetUserTasksUseCase.QueryResult>> GetUserTasks()
    {
        var result = await _sender.Send(new GetUserTasksUseCase.Query());

        if (result.IsFailed)
        {
            return BadRequest(new ErrorModel(result.ToErrorString()));
        }
        
        return Ok(result.Value);
    }
}
