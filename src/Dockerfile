# 1. Estágio de Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o projeto e restaura dependências
COPY ["FCG.PaymentsAPI.csproj", "./"]
RUN dotnet restore "FCG.PaymentsAPI.csproj"

# Copia o restante do código
COPY . .

# Publica aplicação
RUN dotnet publish "FCG.PaymentsAPI.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# 2. Estágio Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "FCG.PaymentsAPI.dll"]