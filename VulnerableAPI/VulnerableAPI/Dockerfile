# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
EXPOSE 5133
WORKDIR /src
COPY VulnerableAPI/*.csproj VulnerableAPI/

WORKDIR /src/VulnerableAPI
RUN dotnet restore

COPY VulnerableAPI/. /src/VulnerableAPI/.
RUN dotnet publish -o /app --no-self-contained --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "VulnerableAPI.dll"]
