# Use the official .NET image as base for development
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Use the SDK image for building and publishing
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy and restore dependencies
COPY ["Machine_Setup_Worksheet.csproj", "."]
RUN dotnet restore "Machine_Setup_Worksheet.csproj"

# Copy the entire project and build
COPY . .
RUN dotnet build "Machine_Setup_Worksheet.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "Machine_Setup_Worksheet.csproj" -c Release -o /app/publish

# Final stage/image for execution
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Machine_Setup_Worksheet.dll"]
