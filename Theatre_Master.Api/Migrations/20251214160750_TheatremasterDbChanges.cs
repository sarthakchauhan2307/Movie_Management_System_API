using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheatreMaster.Api.Migrations
{
    /// <inheritdoc />
    public partial class TheatremasterDbChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Shows_ScreenId",
                table: "Shows",
                column: "ScreenId");

            migrationBuilder.AddForeignKey(
                name: "FK_Screens_Theatres_TheatreId",
                table: "Screens",
                column: "TheatreId",
                principalTable: "Theatres",
                principalColumn: "TheatreId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shows_Screens_ScreenId",
                table: "Shows",
                column: "ScreenId",
                principalTable: "Screens",
                principalColumn: "ScreenId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Screens_Theatres_TheatreId",
                table: "Screens");

            migrationBuilder.DropForeignKey(
                name: "FK_Shows_Screens_ScreenId",
                table: "Shows");

            migrationBuilder.DropIndex(
                name: "IX_Shows_ScreenId",
                table: "Shows");
        }
    }
}
