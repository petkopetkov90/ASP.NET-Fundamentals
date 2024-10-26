using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameZone.Migrations
{
    public partial class GamersGameManyToManyAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GamersGames",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "int", nullable: false),
                    GamerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamersGames", x => new { x.GameId, x.GamerId });
                    table.ForeignKey(
                        name: "FK_GamersGames_AspNetUsers_GamerId",
                        column: x => x.GamerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GamersGames_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GamersGames_GamerId",
                table: "GamersGames",
                column: "GamerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GamersGames");
        }
    }
}
