# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["PMSApi/PMSApi.csproj", "PMSApi/"]
RUN dotnet restore "PMSApi/PMSApi.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/PMSApi"
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PMSApi.dll"]
