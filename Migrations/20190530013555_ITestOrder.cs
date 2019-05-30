using Microsoft.EntityFrameworkCore.Migrations;

namespace TestingFramework.Migrations
{
    public partial class ITestOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "ScorecardTests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "CategoryTests",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "ScorecardTests");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "CategoryTests");
        }
    }
}
