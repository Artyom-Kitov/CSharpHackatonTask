FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Task6RabbitMq/Task6RabbitMq.csproj", "Task6RabbitMq/"]
COPY ["HackatonTaskLib/HackatonTaskLib.csproj", "HackatonTaskLib/"]
COPY ["Nsu.HackathonProblem.Contracts/Nsu.HackathonProblem.Contracts.csproj", "Nsu.HackathonProblem.Contracts/"]
RUN dotnet restore "./Task6RabbitMq/Task6RabbitMq.csproj"
COPY . .
WORKDIR "/src/Task6RabbitMq"
RUN dotnet build "./Task6RabbitMq.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Task6RabbitMq.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN mkdir /app/data
ENTRYPOINT ["dotnet", "Task6RabbitMq.dll"]