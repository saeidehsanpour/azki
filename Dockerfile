#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim AS build
WORKDIR /src
COPY ["PerformanceManagementSystem/PerformanceManagementSystem.csproj", "PerformanceManagementSystem/"]
RUN dotnet restore "PerformanceManagementSystem/PerformanceManagementSystem.csproj"
COPY . .
WORKDIR "/src/PerformanceManagementSystem"
RUN dotnet build "PerformanceManagementSystem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PerformanceManagementSystem.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PerformanceManagementSystem.dll"]