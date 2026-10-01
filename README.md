# SupportWebApp

## Projektets formål

Formålet med projektet er at udvikle en Blazor Web App, som kan bruges til at oprette og vise supporthenvendelser.

Applikationen er koblet sammen med Azure Cosmos DB, hvor supporthenvendelserne bliver gemt. Brugeren kan derfor oprette en ny supporthenvendelse og efterfølgende se de eksisterende henvendelser i applikationen.

Løsningen består blandt andet af en model til supporthenvendelser, en Cosmos DB-service, en side til oprettelse af henvendelser og en side til visning af henvendelser.

## Oprettelse af Cosmos DB

Cosmos DB kan oprettes med Azure CLI. I denne løsning anvendes resource groupen `IBasSupportRG`, regionen `swedencentral`, databasen `IBasSupportDB` og containeren `ibassupport`.

Først oprettes resource groupen:

```bash
az group create \
  --name IBasSupportRG \
  --location swedencentral

Herefter oprettes Cosmos DB-accounten:
az cosmosdb create \
  --name $DBACCOUNT \
  --resource-group $RESGRP \
  --locations regionName=swedencentral \
  --enable-free-tier true
  
Databasen oprettes derefter:
az cosmosdb sql database create \
  --account-name $DBACCOUNT \
  --resource-group $RESGRP \
  --name $DATABASE

Til sidst oprettes containeren med /category som partition key:
az cosmosdb sql container create \
  --account-name $DBACCOUNT \
  --resource-group $RESGRP \
  --database-name $DATABASE \
  --name $CONTAINER \
  --partition-key-path "/category"

De anvendte variabler er:
export RESGRP="IBasSupportRG"
export CONTAINER="ibassupport"
export DATABASE="IBasSupportDB"

Jeg har fuldfør løsningen efter en hård kamp og en masse forvirring.
Jeg har brug Generativ ai til de steder jeg slet ikke kunne finde ud af eller komme videre fra.
Løsningen er testet ved at oprette en supporthenvendelse gennem webapplikationen og efterfølgende vise den på SupportList-siden.