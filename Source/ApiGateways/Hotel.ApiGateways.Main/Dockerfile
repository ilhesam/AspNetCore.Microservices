#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Source/ApiGateways/Hotel.ApiGateways.Main/Hotel.ApiGateways.Main.csproj", "Source/ApiGateways/Hotel.ApiGateways.Main/"]
RUN dotnet restore "Source/ApiGateways/Hotel.ApiGateways.Main/Hotel.ApiGateways.Main.csproj"
COPY . .
WORKDIR "/src/Source/ApiGateways/Hotel.ApiGateways.Main"
RUN dotnet build "Hotel.ApiGateways.Main.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hotel.ApiGateways.Main.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hotel.ApiGateways.Main.dll"]