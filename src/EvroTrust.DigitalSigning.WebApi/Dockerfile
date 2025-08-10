FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

## This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

COPY ./src ./src

RUN dotnet restore "src/EvroTrust.DigitalSigning.WebApi/EvroTrust.DigitalSigning.WebApi.csproj"
COPY . .
WORKDIR "/src/EvroTrust.DigitalSigning.WebApi"
RUN dotnet build "EvroTrust.DigitalSigning.WebApi.csproj" -c Release -o /app/build

## This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
RUN dotnet publish "EvroTrust.DigitalSigning.WebApi.csproj" -c Release -o /app/publish

## This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EvroTrust.DigitalSigning.WebApi.dll"]