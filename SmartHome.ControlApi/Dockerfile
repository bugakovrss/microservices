#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["SmartHome.ControlApi/SmartHome.ControlApi.csproj", "SmartHome.ControlApi/"]
COPY ["SmartHome.Model/SmartHome.Model.csproj", "SmartHome.Model/"]
RUN dotnet restore "SmartHome.ControlApi/SmartHome.ControlApi.csproj"
COPY . .
WORKDIR "/src/SmartHome.ControlApi"
RUN dotnet build "SmartHome.ControlApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartHome.ControlApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartHome.ControlApi.dll"]