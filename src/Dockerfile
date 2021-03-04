FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /src

COPY ./BTCPayServer.Stream.Business ./BTCPayServer.Stream.Business
COPY ./BTCPayServer.Stream.Common ./BTCPayServer.Stream.Common
COPY ./BTCPayServer.Stream.Data ./BTCPayServer.Stream.Data
COPY ./BTCPayServer.Stream.Data.Npgsql ./BTCPayServer.Stream.Data.Npgsql
COPY ./BTCPayServer.Stream.Data.Sqlite ./BTCPayServer.Stream.Data.Sqlite
COPY ./BTCPayServer.Stream.HttpClients ./BTCPayServer.Stream.HttpClients
COPY ./BTCPayServer.Stream.Portal ./BTCPayServer.Stream.Portal
COPY ./BTCPayServer.Stream.Repository ./BTCPayServer.Stream.Repository
COPY ./BTCPayServer.Stream.sln .
COPY ./nuget.config .

RUN dotnet restore

RUN dotnet publish ./BTCPayServer.Stream.Portal -c Release -o /app

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app .

RUN mkdir /Storage
RUN mkdir /Database

ENV ASPNETCORE_URLS http://+:5000
EXPOSE 5000

ENTRYPOINT ["dotnet", "BTCPayServer.Stream.Portal.dll"]