using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyLibrary.Server.Migrations
{
    /// <inheritdoc />
    public partial class Updating_SubscriptionTiers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BorrowLimit",
                table: "SubscriptionTiers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CanBorrow",
                table: "SubscriptionTiers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Discount",
                table: "SubscriptionTiers",
                type: "real",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BorrowLimit",
                table: "SubscriptionTiers");

            migrationBuilder.DropColumn(
                name: "CanBorrow",
                table: "SubscriptionTiers");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "SubscriptionTiers");
        }
    }
}
