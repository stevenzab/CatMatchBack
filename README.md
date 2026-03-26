# CatMatchBack

Backend API ASP.NET Core pour un projet de vote de chats.

## 🌐 Démo en ligne

L'API est déployée et accessible en production:

**URL**: [https://catmatch-dwgtdxf8dffwepa6.francecentral-01.azurewebsites.net/](https://catmatch-dwgtdxf8dffwepa6.francecentral-01.azurewebsites.net/)

**Swagger UI**: [https://catmatch-dwgtdxf8dffwepa6.francecentral-01.azurewebsites.net/swagger](https://catmatch-dwgtdxf8dffwepa6.francecentral-01.azurewebsites.net/swagger)

## Sommaire

1. Presentation
2. Stack technique
3. Architecture
4. Structure du repository
5. Prerequis
6. Installation et lancement en local
7. Configuration
8. Seeding de donnees
9. Documentation API
10. Lancer les tests
11. CORS et front-end
12. Ameliorations possibles

## Presentation

Le projet expose une API REST qui permet de:

- Recuperer la liste des chats, tries par score de vote.
- Recuperer un chat par identifiant.
- Voter pour un chat (increment/decrement du score).

## Stack technique

- .NET 8 (ASP.NET Core Web API)
- MongoDB (MongoDB.Driver)
- Swagger/OpenAPI (Swashbuckle)
- MSTest + Moq (tests unitaires)

## Architecture

Le projet suit une separation par couches:

- CatMatch.API: points d'entree HTTP, configuration middleware, DI racine.
- CatMatch.Application: logique metier (services), orchestration, interfaces.
- CatMatch.Domain: modeles metier, DTO, mapping, settings.
- CatMatch.Infrastructure: acces donnees MongoDB, repository, seeding.
- tests: projets de tests unitaires par couche.

## Structure du repository

```
CatMatchBack/
|- CatMatch.API.slnx
|- CatMatch.API/
|  |- CatMatch.API/                # Projet Web API
|  |- CatMatch.Application/        # Services metier
|  |- CatMatch.Domain/             # Modeles/DTO/settings
|  |- CatMatch.Infrastructure/     # Acces Mongo + seeding
|- tests/
|  |- CatMatch.Api.UnitTests/
|  |- CatMatch.Application.UnitTests/
|  |- CatMatch.Domain.UnitTests/
|  |- CatMatch.Infrastructure.UnitTests/
```

## Prerequis

- SDK .NET 8+
- MongoDB (local ou distant)

## Installation et lancement en local

Depuis la racine du repository:

```bash
dotnet restore CatMatch.API.slnx
dotnet build CatMatch.API.slnx
dotnet run --project CatMatch.API/CatMatch.API/CatMatch.API.csproj
```

En environnement Development, Swagger est disponible sur:

- http://localhost:5081/swagger
- https://localhost:7269/swagger

## Configuration

Fichiers de configuration principaux:

- `CatMatch.API/CatMatch.API/appsettings.json`
- `CatMatch.API/CatMatch.API/appsettings.Development.json`
- `CatMatch.API/CatMatch.API/appsettings.Production.json`

Section attendue pour MongoDB:

```json
"MongoDB": {
	"ConnectionString": "mongodb://localhost:27017",
	"DatabaseName": "Dev"
}
```

Variables utiles:

- `ASPNETCORE_ENVIRONMENT=Development` pour activer Swagger + seeding auto.

## Seeding de donnees

Au demarrage en Development:

- L'application verifie si la collection `Cat` contient deja des donnees.
- Si vide, elle charge `response.json` et insere les chats.

Le fichier `response.json` est copie dans le dossier de sortie au build.

## Documentation API

Base route controller:

- `api/CatMatch`

### 1) Recuperer tous les chats

- Methode: `GET`
- Route: `/api/CatMatch/GetAllCat`
- Reponse: `200 OK` + `CatDto[]`

Exemple:

```json
[
	{
		"id": "67e3fe8b9f8f5638b03e8f2a",
		"url": "http://24.media.tumblr.com/tumblr_m82woaL5AD1rro1o5o1_1280.jpg",
		"vote": 12
	}
]
```

### 2) Recuperer un chat par ID

- Methode: `GET`
- Route: `/api/CatMatch/GetCatById/{id}`
- Reponse: `200 OK` + `CatDto`

Si l'id est introuvable, la couche metier leve une exception.

### 3) Voter pour un chat

- Methode: `POST`
- Route: `/api/CatMatch/VoteCat`
- Body: `CatDto`
- Reponse: `201 Created`

Exemple de body:

```json
{
	"id": "67e3fe8b9f8f5638b03e8f2a",
	"url": "http://24.media.tumblr.com/tumblr_m82woaL5AD1rro1o5o1_1280.jpg",
	"vote": 1
}
```

Notes importantes:

- Le champ `vote` est utilise comme increment (ex: `1`, `-1`, `2`).
- Le score est mis a jour en base via un `$inc` MongoDB.

## Lancer les tests

Depuis la racine:

```bash
dotnet test CatMatch.API.slnx
```

Ou par projet:

```bash
dotnet test tests/CatMatch.Api.UnitTests/CatMatch.Api.UnitTests.csproj
dotnet test tests/CatMatch.Application.UnitTests/CatMatch.Application.UnitTests.csproj
dotnet test tests/CatMatch.Domain.UnitTests/CatMatch.Domain.UnitTests.csproj
dotnet test tests/CatMatch.Infrastructure.UnitTests/CatMatch.Infrastructure.UnitTests.csproj
```

## CORS et front-end

La policy `AllowFrontend` autorise:

- https://cat-match-front.vercel.app
- http://localhost:5173
- http://localhost:3000