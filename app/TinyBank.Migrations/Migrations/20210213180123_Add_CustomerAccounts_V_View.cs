using Microsoft.EntityFrameworkCore.Migrations;

namespace TinyBank.Migrations
{
    public partial class Add_CustomerAccounts_V_View : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Create Or Alter View CustomerAccounts_V As " +
                                    "Select  C.CustomerId, C.Name, C.SureName, C.Address, C.CustType, C.CustBankID, C.VatNumber, C.Created As CustCreateDT, C.Active," +
                                    "A.AccountsId, A.AccountNumber, A.AccountDescription, A.State, A.Balance, A.Currency, A.Created AccountCreateDate " +
                                "From Customer C " +
                                "   Left Join Accounts A On A.CustomerId = C.CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW [dbo].[CustomerAccounts_V]");
        }
    }
}
