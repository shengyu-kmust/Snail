using Microsoft.EntityFrameworkCore.Migrations;

namespace Snail.Permission.Test.Migrations
{
    public partial class _201909212 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Gender",
                table: "User",
                newName: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "int",
                table: "User",
                newName: "Gender");
        }
    }
}
