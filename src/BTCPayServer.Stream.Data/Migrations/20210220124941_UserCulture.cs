using Microsoft.EntityFrameworkCore.Migrations;

namespace BTCPayServer.Stream.Data.Migrations
{
    public partial class UserCulture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DefaultCulture",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultCulture",
                table: "AspNetUsers");
        }
    }
}
