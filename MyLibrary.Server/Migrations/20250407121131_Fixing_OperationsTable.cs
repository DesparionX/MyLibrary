using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyLibrary.Server.Migrations
{
    /// <inheritdoc />
    public partial class Fixing_OperationsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "Operations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientName",
                table: "Operations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OperationDate",
                table: "Operations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OperationName",
                table: "Operations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Operations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StaffId",
                table: "Operations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StaffName",
                table: "Operations",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "ClientId",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "ClientName",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "OperationDate",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "OperationName",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "StaffId",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "StaffName",
                table: "Operations");
        }
    }
}
