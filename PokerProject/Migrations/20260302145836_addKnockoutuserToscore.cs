using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokerProject.Migrations
{
    /// <inheritdoc />
    public partial class addKnockoutuserToscore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KnockedOutUserId",
                table: "Scores",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Scores_KnockedOutUserId",
                table: "Scores",
                column: "KnockedOutUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_Users_KnockedOutUserId",
                table: "Scores",
                column: "KnockedOutUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scores_Users_KnockedOutUserId",
                table: "Scores");

            migrationBuilder.DropIndex(
                name: "IX_Scores_KnockedOutUserId",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "KnockedOutUserId",
                table: "Scores");
        }
    }
}
