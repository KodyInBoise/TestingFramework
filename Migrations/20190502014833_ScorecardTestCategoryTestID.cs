using Microsoft.EntityFrameworkCore.Migrations;

namespace TestingFramework.Migrations
{
    public partial class ScorecardTestCategoryTestID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TestID",
                table: "ScorecardTests",
                newName: "CategoryTestID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CategoryTestID",
                table: "ScorecardTests",
                newName: "TestID");
        }
    }
}
