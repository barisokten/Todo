# 1. Temel çalışma zamanı imajı
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# 2. SDK imajı ile build işlemi
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Proje dosyasını kopyala (proje adını kendi dosyana göre düzelt!)
COPY TodoApi.csproj ./
RUN dotnet restore

# Tüm dosyaları kopyala ve build et
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# 3. Çalışan final imaj
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TodoApi.dll"]  
# Proje adıyla aynı olmalı! 
 
