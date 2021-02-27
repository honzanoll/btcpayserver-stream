using Microsoft.EntityFrameworkCore.Migrations;

namespace BTCPayServer.Stream.Data.Migrations
{
    public partial class UserStylesheet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StylesheetFile",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StylesheetFile",
                table: "AspNetUsers");
        }
    }
}
