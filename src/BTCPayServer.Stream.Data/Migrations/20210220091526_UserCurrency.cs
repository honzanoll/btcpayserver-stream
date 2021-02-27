using Microsoft.EntityFrameworkCore.Migrations;

namespace BTCPayServer.Stream.Data.Migrations
{
    public partial class UserCurrency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DefaultCurrency",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultCurrency",
                table: "AspNetUsers");
        }
    }
}
