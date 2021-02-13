using Microsoft.EntityFrameworkCore.Migrations;

namespace TinyBank.Migrations
{
    public partial class Rename_Account_Column_Active_to_State : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Accounts");

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Accounts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "Accounts");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Accounts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
