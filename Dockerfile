FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

COPY ["Directory.Build.props", "./"]
COPY ["Directory.Packages.props", "./"]

COPY ["UserTaskManagement.sln", "./"]

COPY ["UserTaskManagement/", "UserTaskManagement/"]
COPY ["UserTaskManagement.Domain/", "UserTaskManagement.Domain/"]
COPY ["UserTaskManagement.Application/", "UserTaskManagement.Application/"]
COPY ["UserTaskManagement.Application.UseCases/", "UserTaskManagement.Application.UseCases/"]
COPY ["UserTaskManagement.Application.DrivenPorts/", "UserTaskManagement.Application.DrivenPorts/"]
COPY ["UserTaskManagement.Client.UserData/", "UserTaskManagement.Client.UserData/"]
COPY ["UserTaskManagement.DrivenAdapters.DomainModel/", "UserTaskManagement.DrivenAdapters.DomainModel/"]
COPY ["UserTaskManagement.DrivenAdapters.UserData/", "UserTaskManagement.DrivenAdapters.UserData/"]
COPY ["UserTaskManagement.DrivenAdapters.MessageBroker/", "UserTaskManagement.DrivenAdapters.MessageBroker/"]
COPY ["UserTaskManagement.DrivenAdapters.OutboxProcessor/", "UserTaskManagement.DrivenAdapters.OutboxProcessor/"]
COPY ["UserTaskManagement.DrivenAdapters.OutboxProcessor/", "UserTaskManagement.DrivenAdapters.OutboxProcessor/"]
COPY ["UserTaskManagement.Tests/", "UserTaskManagement.Tests/"]

RUN dotnet restore "UserTaskManagement.sln"

COPY . .

WORKDIR "/src/UserTaskManagement"
RUN dotnet publish "UserTaskManagement.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime
WORKDIR /app

RUN addgroup -g 1001 appgroup && \
    adduser -u 1001 -G appgroup -D appuser && \
    chown -R appuser:appgroup /app

COPY --from=build --chown=appuser:appgroup /app/publish .

USER appuser

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

ENTRYPOINT ["dotnet", "UserTaskManagement.dll"]