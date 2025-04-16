using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyLibrary.Server.Migrations
{
    /// <inheritdoc />
    public partial class Subscriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameColumn(
            //    name: "BookLimit",
            //    table: "Users",
            //    newName: "BorrowLimit");

            //migrationBuilder.AddColumn<bool>(
            //    name: "CanBorrow",
            //    table: "Users",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<float>(
            //    name: "Discount",
            //    table: "Users",
            //    type: "real",
            //    nullable: false,
            //    defaultValue: 0f);

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false, defaultValueSql: "NEWID()"),
                    SubscriptionId = table.Column<int>(type: "int", nullable: true),
                    SubscriptionTier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Months = table.Column<int>(type: "int", nullable: true),
                    ExpireDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionTiers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Months = table.Column<int>(type: "int", nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionTiers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "SubscriptionTiers");

            //migrationBuilder.DropColumn(
            //    name: "CanBorrow",
            //    table: "Users");

            //migrationBuilder.DropColumn(
            //    name: "Discount",
            //    table: "Users");

            //migrationBuilder.RenameColumn(
            //    name: "BorrowLimit",
            //    table: "Users",
            //    newName: "BookLimit");
        }
    }
}
