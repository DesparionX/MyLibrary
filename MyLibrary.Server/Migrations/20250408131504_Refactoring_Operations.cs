using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyLibrary.Server.Migrations
{
    /// <inheritdoc />
    public partial class Refactoring_Operations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArticleISBN",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "ArticleId",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "ArticleName",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Operations");

            migrationBuilder.RenameColumn(
                name: "StaffName",
                table: "Operations",
                newName: "UserRole");

            migrationBuilder.RenameColumn(
                name: "StaffId",
                table: "Operations",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "ClientName",
                table: "Operations",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Operations",
                newName: "OrderList");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserRole",
                table: "Operations",
                newName: "StaffName");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Operations",
                newName: "StaffId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Operations",
                newName: "ClientName");

            migrationBuilder.RenameColumn(
                name: "OrderList",
                table: "Operations",
                newName: "ClientId");

            migrationBuilder.AddColumn<string>(
                name: "ArticleISBN",
                table: "Operations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArticleId",
                table: "Operations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArticleName",
                table: "Operations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Operations",
                type: "int",
                nullable: true);
        }
    }
}
