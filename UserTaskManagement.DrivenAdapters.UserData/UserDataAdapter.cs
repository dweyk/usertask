using FluentResults;
using UserTaskManagement.Application.DrivenPorts.UserData;
using UserTaskManagement.Client.UserData;
using UserProjection = UserTaskManagement.Application.DrivenPorts.UserData.Models.UserData;

namespace UserTaskManagement.DrivenAdapters.UserData;

public sealed class UserDataAdapter : IUserDataPort
{
    private readonly IUserDataClient _userDataClient;

    public UserDataAdapter(IUserDataClient userDataClient)
    {
        _userDataClient = userDataClient;
    }
    
    public async Task<Result<UserProjection>> GetUserData(long userId)
    {
        var request = new GetUserInfoRequest(userId);
        
        var getUserInfoResult = await _userDataClient.GetUserInfo(request);

        if (getUserInfoResult.IsFailed)
        {
            return getUserInfoResult.ToResult();
        }
        
        var userData = getUserInfoResult.Value;
        
        return new UserProjection(
            UserId: userData.UserId,
            Name: userData.Name
        );
    }
}
