FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

COPY services/PaymentsApi/ca-oebm.crt.cer /usr/local/share/ca-certificates/CA-OEBM.crt

RUN update-ca-certificates

WORKDIR /src

COPY services ./services

WORKDIR /src/services/UserApi

RUN dotnet restore UserApi.csproj
RUN dotnet publish UserApi.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "UserApi.dll"]