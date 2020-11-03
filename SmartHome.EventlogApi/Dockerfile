FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["SmartHome.EventlogApi/SmartHome.EventlogApi.csproj", "SmartHome.EventlogApi/"]
COPY ["SmartHome.Model/SmartHome.Model.csproj", "SmartHome.Model/"]
COPY ["SmartHome.Net/SmartHome.Net.csproj", "SmartHome.Net/"]
RUN dotnet restore "SmartHome.EventlogApi/SmartHome.EventlogApi.csproj"
COPY . .
WORKDIR "/src/SmartHome.EventlogApi"
RUN dotnet build "SmartHome.EventlogApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartHome.EventlogApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartHome.EventlogApi.dll"]