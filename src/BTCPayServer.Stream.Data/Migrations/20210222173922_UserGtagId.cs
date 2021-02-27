using Microsoft.EntityFrameworkCore.Migrations;

namespace BTCPayServer.Stream.Data.Migrations
{
    public partial class UserGtagId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GtagId",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GtagId",
                table: "AspNetUsers");
        }
    }
}
