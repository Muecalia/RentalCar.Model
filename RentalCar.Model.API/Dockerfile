FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RentalCar.Model.API/RentalCar.Model.API.csproj", "RentalCar.Model.API/"]
RUN dotnet restore "RentalCar.Model.API/RentalCar.Model.API.csproj"
COPY . .
WORKDIR "/src/RentalCar.Model.API"
RUN dotnet build "RentalCar.Model.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "RentalCar.Model.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RentalCar.Model.API.dll"]
