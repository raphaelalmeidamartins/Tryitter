using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace Tryitter.Migrations
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public partial class RemoveImageFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Images_ProfilePictureId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ProfilePictureId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProfilePictureId",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProfilePictureId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProfilePictureId",
                table: "Users",
                column: "ProfilePictureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Images_ProfilePictureId",
                table: "Users",
                column: "ProfilePictureId",
                principalTable: "Images",
                principalColumn: "ImageId");
        }
    }
}
