FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

COPY services/PaymentsApi/ca-oebm.crt.cer /usr/local/share/ca-certificates/CA-OEBM.crt

RUN update-ca-certificates

WORKDIR /src

COPY services ./services

WORKDIR /src/services/StockApi

RUN dotnet restore StockApi.csproj
RUN dotnet publish StockApi.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "StockApi.dll"]