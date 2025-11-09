# Use official .NET 8 SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything and build
COPY . .
RUN dotnet publish -c Release -o /out

# Use official ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published files
COPY --from=build /out .

# Render provides a PORT env var; use it
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}
ENV PORT=10000
EXPOSE 10000

# Start the application
ENTRYPOINT ["dotnet", "ContainVirus.dll"]
