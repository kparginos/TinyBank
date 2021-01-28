using Microsoft.EntityFrameworkCore.Migrations;

namespace TinyBank.Migrations
{
    public partial class Add_AccountLink_To_Customer_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Accounts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_CustomerId",
                table: "Accounts",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Customer_CustomerId",
                table: "Accounts",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Customer_CustomerId",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_CustomerId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Accounts");
        }
    }
}
