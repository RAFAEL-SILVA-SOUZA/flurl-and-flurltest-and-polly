
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_ENVIRONMENT=Development 

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ClientFlurl.Api/ClientFlurl.Api.csproj", "ClientFlurl.Api/"]
COPY ["ClientFlurl.Domain/ClientFlurl.Domain.csproj", "ClientFlurl.Domain/"]
RUN dotnet restore "ClientFlurl.Api/ClientFlurl.Api.csproj"
COPY . .
WORKDIR "/src/ClientFlurl.Api"
RUN dotnet build "ClientFlurl.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ClientFlurl.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ClientFlurl.Api.dll"]