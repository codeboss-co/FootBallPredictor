
## Migrations

> dotnet tool install --global dotnet-ef --version 3.1.1

> Install-Package Microsoft.EntityFrameworkCore.Design into FootballPredictor.Api

> dotnet ef migrations add InitialCreate -s .\src\hosts\FootballPredictor.Api\ -p .\src\libraries\Data\FootballPredictor.Data.EFCore.PostgreSQL\

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