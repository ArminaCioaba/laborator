FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /app
EXPOSE 80
FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /src
COPY ["Proiect1.2.csproj", "./"]
RUN dotnet restore "./Proiect1.2.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Proiect1.2.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "Proiect1.2.csproj" -c Release -o /app/publish
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Proiect1.2.dll"]