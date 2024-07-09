# Use the official .NET image as base for development
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8001
EXPOSE 8002


# Use the SDK image for building and publishing
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Machine_Setup_Worksheet.csproj", "."]
RUN dotnet restore "./Machine_Setup_Worksheet.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "Machine_Setup_Worksheet.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "Machine_Setup_Worksheet.csproj" -c Release -o /app/publish

# Final stage/image for execution
FROM base AS final
ENV ASPNETCORE_HTTP_PORTS=8001
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Machine_Setup_Worksheet.dll"]
