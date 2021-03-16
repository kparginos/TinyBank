using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TinyBank.Migrations
{
    public partial class Add_AuditInfo_To_Customer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Updated",
                table: "Customer",
                newName: "AuditInfo_Updated");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Customer",
                newName: "AuditInfo_Created");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "AuditInfo_Created",
                table: "Customer",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AuditInfo_Updated",
                table: "Customer",
                newName: "Updated");

            migrationBuilder.RenameColumn(
                name: "AuditInfo_Created",
                table: "Customer",
                newName: "Created");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Customer",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);
        }
    }
}
