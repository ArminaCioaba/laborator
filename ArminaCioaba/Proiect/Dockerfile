FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /app
EXPOSE 80
FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /src
COPY ["Proiect.csproj", "./"]
RUN dotnet restore "./Proiect.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Proiect.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "Proiect.csproj" -c Release -o /app/publish
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Proiect.dll"]