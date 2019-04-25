using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestingFramework.Migrations
{
    public partial class ResultStartedTimestamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "StartedTimestamp",
                table: "ScorecardResults",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartedTimestamp",
                table: "ScorecardResults");
        }
    }
}
