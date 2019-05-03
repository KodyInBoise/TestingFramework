using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestingFramework.Migrations
{
    public partial class TaskOwnerNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "Owner",
                table: "Tasks",
                nullable: true,
                oldClrType: typeof(Guid));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "Owner",
                table: "Tasks",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);
        }
    }
}
