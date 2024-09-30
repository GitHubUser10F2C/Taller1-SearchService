# Usar la imagen base de .NET SDK 8 para construir la aplicación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar el archivo de solución y los archivos del proyecto
COPY *.sln ./
COPY *.csproj ./
RUN dotnet restore

# Copiar el resto del código y compilar la aplicación
COPY . ./
RUN dotnet publish -c Release -o out

# Usar la imagen base de .NET Runtime 8 para ejecutar la aplicación
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Especificar el comando de inicio
ENTRYPOINT ["dotnet", "Taller1-SearchService.dll"]