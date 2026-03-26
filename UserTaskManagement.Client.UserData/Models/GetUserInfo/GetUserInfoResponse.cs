namespace UserTaskManagement.Client.UserData;

public sealed record GetUserInfoResponse(
    long UserId,
    string Name
);
