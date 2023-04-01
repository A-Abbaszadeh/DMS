using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.Persistence.Migrations
{
    public partial class AuditField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "InsertTime",
                schema: "identity",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                schema: "identity",
                table: "Users",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RemoveTime",
                schema: "identity",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTime",
                schema: "identity",
                table: "Users",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InsertTime",
                schema: "identity",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                schema: "identity",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RemoveTime",
                schema: "identity",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UpdateTime",
                schema: "identity",
                table: "Users");
        }
    }
}
