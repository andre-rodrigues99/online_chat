FROM mcr.microsoft.com/dotnet/sdk:latest AS build

WORKDIR /app

COPY *.csproj ./

RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:latest AS base

WORKDIR /app

COPY --from=build /app/publish .

ENV ORACLE_DB_URL=$ORACLE_DB_URL  
ENV ORACLE_DB_PORT=$ORACLE_DB_PORT  
ENV ORACLE_DB_USER=$ORACLE_DB_USER  
ENV ORACLE_DB_PASSW=$ORACLE_DB_PASSW  
ENV ORACLE_DB_SERV_SID=$ORACLE_DB_SERV_SID 

EXPOSE 80

ENTRYPOINT ["dotnet", "backend.dll"]
