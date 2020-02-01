using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Snail.Permission.Test.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orgs",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Creater = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Updater = table.Column<string>(nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ParentId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orgs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Creater = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Updater = table.Column<string>(nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    RoleId = table.Column<string>(nullable: true),
                    ResourceId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Creater = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Updater = table.Column<string>(nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    ParentId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Creater = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Updater = table.Column<string>(nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserOrgs",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Creater = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Updater = table.Column<string>(nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    OrgId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOrgs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Creater = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Updater = table.Column<string>(nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    RoleId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Creater = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Updater = table.Column<string>(nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Account = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Pwd = table.Column<string>(nullable: true),
                    Gender = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orgs");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Resources");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "UserOrgs");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
