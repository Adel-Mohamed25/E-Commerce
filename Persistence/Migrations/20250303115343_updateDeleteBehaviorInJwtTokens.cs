using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updateDeleteBehaviorInJwtTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JwtTokens_Users_UserId",
                table: "JwtTokens");

            migrationBuilder.AddForeignKey(
                name: "FK_JwtTokens_Users_UserId",
                table: "JwtTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JwtTokens_Users_UserId",
                table: "JwtTokens");

            migrationBuilder.AddForeignKey(
                name: "FK_JwtTokens_Users_UserId",
                table: "JwtTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
