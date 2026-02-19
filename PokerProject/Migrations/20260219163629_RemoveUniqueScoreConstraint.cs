using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokerProject.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUniqueScoreConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Scores_UserId_GameId",
                table: "Scores");

            migrationBuilder.CreateIndex(
                name: "IX_Scores_UserId_GameId",
                table: "Scores",
                columns: new[] { "UserId", "GameId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Scores_UserId_GameId",
                table: "Scores");

            migrationBuilder.CreateIndex(
                name: "IX_Scores_UserId_GameId",
                table: "Scores",
                columns: new[] { "UserId", "GameId" },
                unique: true);
        }
    }
}
