# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY OrderFlow.Application ./OrderFlow.Application/
COPY OrderFlow.Domain ./OrderFlow.Domain/
COPY OrderFlow.Infra ./OrderFlow.Infra/
COPY OrderFlow.Api ./OrderFlow.Api/
COPY OrderFlow.slnx .

RUN dotnet restore "OrderFlow.Api/OrderFlow.Api.csproj"
RUN dotnet publish "OrderFlow.Api/OrderFlow.Api.csproj" -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

# Definir porta e ambiente
ENV ASPNETCORE_URLS=http://+:7267
ENV ASPNETCORE_ENVIRONMENT=Development

EXPOSE 7267

ENTRYPOINT ["dotnet", "OrderFlow.Api.dll"]
