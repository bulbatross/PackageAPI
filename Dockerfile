FROM mcr.microsoft.com/dotnet/sdk:7.0.400 AS build-env
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0.10
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "PackageAPI.dll"]