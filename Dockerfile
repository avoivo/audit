
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.sln ./
COPY Probanx.TransactionAudit.Consumer/*.csproj ./Probanx.TransactionAudit.Consumer/
COPY Probanx.TransactionAudit.Core/*.csproj ./Probanx.TransactionAudit.Core/
COPY Probanx.TransactionAudit.RabbitMQ/*.csproj ./Probanx.TransactionAudit.RabbitMQ/
COPY Probanx.TransactionAudit.Web/*.csproj ./Probanx.TransactionAudit.Web/

RUN dotnet restore

COPY . ./
RUN dotnet publish Probanx.TransactionAudit.Web -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "Probanx.TransactionAudit.Web.dll"]