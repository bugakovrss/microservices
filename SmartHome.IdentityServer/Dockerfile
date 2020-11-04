FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["SmartHome.IdentityServer/SmartHome.IdentityServer.csproj", "SmartHome.IdentityServer/"]
RUN dotnet restore "SmartHome.IdentityServer/SmartHome.IdentityServer.csproj"
COPY . .
WORKDIR "/src/SmartHome.IdentityServer"
RUN dotnet build "SmartHome.IdentityServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartHome.IdentityServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartHome.IdentityServer.dll"]