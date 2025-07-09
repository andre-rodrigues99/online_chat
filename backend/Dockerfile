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

RUN apt-get update && apt-get install -y netcat && rm -rf /var/lib/apt/lists/*

#ENTRYPOINT ["dotnet", "backend.dll"]
ENTRYPOINT /bin/sh -c "\
  echo 'Testando conexão com Oracle em $ORACLE_DB_URL:$ORACLE_DB_PORT...'; \
  nc -z -v -w 5 $ORACLE_DB_URL $ORACLE_DB_PORT || (echo 'Falha na conexão TCP'; exit 1); \
  echo 'Conexão OK! Iniciando backend...'; \
  dotnet backend.dll"