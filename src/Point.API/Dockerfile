FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Point.API/Point.API.csproj", "Point.API/"]
COPY ["Point.Shared/Point.Shared.csproj", "Point.Shared/"]
COPY ["Point.Application/Point.Application.csproj", "Point.Application/"]
COPY ["Point.Domain/Point.Domain.csproj", "Point.Domain/"]
COPY ["Point.Infrastructure/Point.Infrastructure.csproj", "Point.Infrastructure/"]
RUN dotnet restore "Point.API/Point.API.csproj"
COPY . .
WORKDIR "/src/Point.API"
RUN dotnet build "Point.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Point.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Point.API.dll"]