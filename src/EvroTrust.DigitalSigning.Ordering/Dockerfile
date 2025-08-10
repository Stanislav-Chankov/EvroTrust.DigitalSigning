# Use the official .NET 8 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copy project files and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy all source code and build the application
COPY . ./
RUN dotnet publish -c Release -o /out

# Use the .NET 8 runtime image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app
COPY --from=build /out ./

# Run the application
ENTRYPOINT ["dotnet", "EvroTrust.DigitalSigning.Ordering.dll"]