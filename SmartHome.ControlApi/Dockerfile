FROM mcr.microsoft.com/dotnet/core/sdk:3.1.301-bionic AS build 
WORKDIR /src
COPY ["SmartHome.ControlApi/SmartHome.ControlApi.csproj", "SmartHome.ControlApi/"]
COPY ["SmartHome.Model/SmartHome.Model.csproj", "SmartHome.Model/"]
COPY ["SmartHome.Net/SmartHome.Net.csproj", "SmartHome.Net/"]
RUN dotnet restore "SmartHome.ControlApi/SmartHome.ControlApi.csproj"
COPY . .
WORKDIR "/src/SmartHome.ControlApi"
RUN dotnet build "SmartHome.ControlApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartHome.ControlApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.5-bionic AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartHome.ControlApi.dll"]