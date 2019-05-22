using Microsoft.EntityFrameworkCore.Migrations;

namespace TestingFramework.Migrations
{
    public partial class UserInfoName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "UserInfos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "UserInfos");
        }
    }
}
