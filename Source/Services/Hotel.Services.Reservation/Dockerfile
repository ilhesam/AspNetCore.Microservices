#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Source/Services/Hotel.Services.Reservation/Hotel.Services.Reservation.csproj", "Source/Services/Hotel.Services.Reservation/"]
RUN dotnet restore "Source/Services/Hotel.Services.Reservation/Hotel.Services.Reservation.csproj"
COPY . .
WORKDIR "/src/Source/Services/Hotel.Services.Reservation"
RUN dotnet build "Hotel.Services.Reservation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hotel.Services.Reservation.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hotel.Services.Reservation.dll"]