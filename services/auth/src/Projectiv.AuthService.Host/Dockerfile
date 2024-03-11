FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY "ProjectvilBackend.sln" "ProjectvilBackend.sln"

# Gateways
COPY "gateways/webGateway/src/Projectiv.WebGateway.Host/Projectiv.WebGateway.Host.csproj" "gateways/webGateway/src/Projectiv.WebGateway.Host/Projectiv.WebGateway.Host.csproj"

# Microservices
COPY "services/auth/src/Projectiv.AuthService.Host/Projectiv.AuthService.Host.csproj" "services/auth/src/Projectiv.AuthService.Host/Projectiv.AuthService.Host.csproj"
COPY "services/identity/src/Projectiv.IdentityService.Host/Projectiv.IdentityService.Host.csproj" "services/identity/src/Projectiv.IdentityService.Host/Projectiv.IdentityService.Host.csproj"
COPY "services/petProjects/src/Projectiv.PetprojectsService.Host/Projectiv.PetprojectsService.Host.csproj" "services/petProjects/src/Projectiv.PetprojectsService.Host/Projectiv.PetprojectsService.Host.csproj"

# Shared Projects
COPY "shared/Projectvil.Shared.EntityFramework/Projectvil.Shared.EntityFramework.csproj" "services/petProjects/src/Projectiv.PetprojectsService.Host/Projectiv.PetprojectsService.Host.csproj"
COPY "shared/Projectvil.Shared.Infrastructures/Projectvil.Shared.Infrastructures.csproj" "shared/Projectvil.Shared.Infrastructures/Projectvil.Shared.Infrastructures.csproj"
COPY "shared/Projectvil.Shared.EventBus.Kafka/Projectvil.Shared.EventBus.Kafka.csproj" "shared/Projectvil.Shared.EventBus.Kafka/Projectvil.Shared.EventBus.Kafka.csproj"

RUN dotnet restore "AMSuisse.sln"

COPY . .
WORKDIR "/src/services/auth/src/Projectiv.AuthService.Host"
RUN dotnet publish -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Projectiv.AuthService.Host.dll"]