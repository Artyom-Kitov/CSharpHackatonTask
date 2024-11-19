FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5000 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Task5Http/Task5Http.csproj", "Task5Http/"]
COPY ["HackatonTaskLib/HackatonTaskLib.csproj", "HackatonTaskLib/"]
COPY ["Nsu.HackathonProblem.Contracts/Nsu.HackathonProblem.Contracts.csproj", "Nsu.HackathonProblem.Contracts/"]
RUN dotnet restore "./Task5Http/Task5Http.csproj"
COPY . .
WORKDIR "/src/Task5Http"
RUN dotnet build "./Task5Http.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Task5Http.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN mkdir /app/data
ENTRYPOINT ["dotnet", "Task5Http.dll"]