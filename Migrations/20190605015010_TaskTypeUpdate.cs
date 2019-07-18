using Microsoft.EntityFrameworkCore.Migrations;

namespace TestingFramework.Migrations
{
    public partial class TaskTypeUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Tasks",
                newName: "TaskType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaskType",
                table: "Tasks",
                newName: "Type");
        }
    }
}
