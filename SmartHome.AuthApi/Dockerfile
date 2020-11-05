FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["SmartHome.AuthApi/SmartHome.AuthApi.csproj", "SmartHome.AuthApi/"]
COPY ["SmartHome.Model/SmartHome.Model.csproj", "SmartHome.Model/"]
RUN dotnet restore "SmartHome.AuthApi/SmartHome.AuthApi.csproj"
COPY . .
WORKDIR "/src/SmartHome.AuthApi"
RUN dotnet build "SmartHome.AuthApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartHome.AuthApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartHome.AuthApi.dll"]