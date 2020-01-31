
## Migrations

> dotnet tool install --global dotnet-ef --version 3.1.1

> Install-Package Microsoft.EntityFrameworkCore.Design into FootballPredictor.Api

> dotnet ef migrations add InitialCreate -s .\src\hosts\FootballPredictor.Api\ -p .\src\libraries\Data\FootballPredictor.Data.EFCore.PostgreSQL\

> dotnet ef migrations remove -s .\src\hosts\FootballPredictor.Api\ -p .\src\libraries\Data\FootballPredictor.Data.EFCore.PostgreSQL\

> dotnet ef database update -s .\src\hosts\FootballPredictor.Api\ -p .\src\libraries\Data\FootballPredictor.Data.EFCore.PostgreSQL\

## Postman

```
POST /match HTTP/1.1
Host: localhost:5000
Content-Type: application/json

{
	"Competition" : "PL",
	"Matchday" : 1
}
```

## Database ML Query

```
SELECT 
CAST("public"."Matches"."SeasonId" as REAL) as "SeasonId", 
"public"."Matches"."HomeTeam",
CAST("public"."Matches"."HomeTeamId" as REAL) as "HomeTeamId", 
"public"."Matches"."AwayTeam",
CAST("public"."Matches"."AwayTeamId" as REAL) as "AwayTeamId", 
"public"."Matches"."Winner",
CAST("public"."Matches"."WinnerId" as REAL) as "WinnerId", 
CAST("public"."Matches"."HomeTeamGoals" as REAL) as "HomeTeamGoals", 
CAST("public"."Matches"."AwayTeamGoals" as REAL) as "AwayTeamGoals"
FROM "public"."Matches"
```

## Logging

> docker run -e ACCEPT_EULA=Y   -v /path/to/seq/data:/data   -p 80:80   -p 5341:5341   datalust/seq:latest