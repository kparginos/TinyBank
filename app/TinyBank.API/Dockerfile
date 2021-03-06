#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["app/TinyBank.Api/TinyBank.Api.csproj", "app/TinyBank.Api/"]
COPY ["src/TinyBank.Core/TinyBank.Core.csproj", "src/TinyBank.Core/"]
RUN dotnet restore "app/TinyBank.Api/TinyBank.Api.csproj"
COPY . .
WORKDIR "/src/app/TinyBank.Api"
RUN dotnet build "TinyBank.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TinyBank.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TinyBank.Api.dll"]