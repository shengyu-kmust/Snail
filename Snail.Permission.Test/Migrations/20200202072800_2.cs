using Microsoft.EntityFrameworkCore.Migrations;

namespace Snail.Permission.Test.Migrations
{
    public partial class _2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions");

            migrationBuilder.RenameTable(
                name: "Permissions",
                newName: "RoleResources");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleResources",
                table: "RoleResources",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleResources",
                table: "RoleResources");

            migrationBuilder.RenameTable(
                name: "RoleResources",
                newName: "Permissions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions",
                column: "Id");
        }
    }
}
