using Microsoft.EntityFrameworkCore.Migrations;

namespace TinyBank.Migrations
{
    public partial class Add_TransactionStateType_Column : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Transaction",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "Transaction");
        }
    }
}
