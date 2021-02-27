using Microsoft.EntityFrameworkCore.Migrations;

namespace BTCPayServer.Stream.Data.Migrations
{
    public partial class InvoiceCurrency1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Invoices");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Invoices",
                type: "text",
                nullable: true);
        }
    }
}
