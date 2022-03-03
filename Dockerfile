#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build

ARG nuget_pat

WORKDIR /app
# Copy csproj and restore as distinct layers
COPY *.sln .

COPY src/Catalog.Api/*.csproj ./src/Catalog.Api/
COPY src/Catalog.ApiContract/*.csproj ./src/Catalog.ApiContract/
COPY src/Catalog.ApplicationService/*.csproj ./src/Catalog.ApplicationService/
COPY src/Catalog.Container/*.csproj ./src/Catalog.Container/
COPY src/Catalog.Domain/*.csproj ./src/Catalog.Domain/
COPY src/Catalog.Repository/*.csproj ./src/Catalog.Repository/
COPY test/Catalog.ApplicationService.Test/*.csproj ./test/Catalog.ApplicationService.Test/
COPY test/Catalog.Domain.Test/*.csproj ./test/Catalog.Domain.Test/
COPY test/Catalog.Repository.Test/*.csproj ./test/Catalog.Repository.Test/

ENV NUGET_CREDENTIALPROVIDER_SESSIONTOKENCACHE_ENABLED true
ENV VSS_NUGET_EXTERNAL_FEED_ENDPOINTS '{"endpointCredentials":[{"endpoint":"https://pkgs.dev.azure.com/topkapi-as/_packaging/PlatformCoreFeed/nuget/v3/index.json","username":"NoRealUserName","password":"'${nuget_pat}'"}]}'
RUN echo "Info Log : VSS_NUGET_EXTERNAL_FEED_ENDPOINTS => $VSS_NUGET_EXTERNAL_FEED_ENDPOINTS"
# Get and install the Artifact Credential provider
RUN wget -qO- https://aka.ms/install-artifacts-credprovider.sh | bash

RUN dotnet restore -s https://pkgs.dev.azure.com/topkapi-as/_packaging/PlatformCoreFeed/nuget/v3/index.json -s https://api.nuget.org/v3/index.json

# Copy everything else and build
COPY src/Catalog.Api/. ./src/Catalog.Api/
COPY src/Catalog.ApiContract/. ./src/Catalog.ApiContract/
COPY src/Catalog.ApplicationService/. ./src/Catalog.ApplicationService/
COPY src/Catalog.Container/. ./src/Catalog.Container/
COPY src/Catalog.Domain/. ./src/Catalog.Domain/
COPY src/Catalog.Repository/. ./src/Catalog.Repository/
COPY test/Catalog.ApplicationService.Test/. ./test/Catalog.ApplicationService.Test/
COPY test/Catalog.Domain.Test/. ./test/atalog.Domain.Test/
COPY test/Catalog.Repository.Test/. ./test/Catalog.Repository.Test/

WORKDIR /app/src/Catalog.Api
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app/src/Catalog.Api/out .
ENTRYPOINT ["dotnet", "Catalog.Api.dll"]