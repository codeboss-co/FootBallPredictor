using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace FootballPredictor.Data.EFCore.PostgreSQL.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MatchId = table.Column<long>(nullable: false),
                    SeasonId = table.Column<long>(nullable: false),
                    Matchday = table.Column<long>(nullable: false),
                    HomeTeam = table.Column<string>(nullable: true),
                    HomeTeamId = table.Column<long>(nullable: false),
                    AwayTeam = table.Column<string>(nullable: true),
                    AwayTeamId = table.Column<long>(nullable: false),
                    Winner = table.Column<string>(nullable: true),
                    WinnerId = table.Column<long>(nullable: true),
                    HomeTeamGoals = table.Column<int>(nullable: false),
                    AwayTeamGoals = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Matches");
        }
    }
}
