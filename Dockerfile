FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build-env
 
WORKDIR /app


COPY ./ClientFlurl.Api/ClientFlurl.Api.csproj ClientFlurl.Api/
COPY ./ClientFlurl.Domain/ClientFlurl.Domain.csproj ClientFlurl.Domain/

RUN dotnet restore "ClientFlurl.Api/ClientFlurl.Api.csproj"
COPY . ./


RUN dotnet publish -c Release -o out
 
FROM mcr.microsoft.com/dotnet/aspnet:5.0
 
WORKDIR /app
 
COPY --from=build-env /app/out .
 
EXPOSE 80
 
ENTRYPOINT [ "dotnet", "ClientFlurl.Api.dll" ]