using Microsoft.EntityFrameworkCore.Migrations;

namespace BTCPayServer.Stream.Data.Sqlite.Migrations
{
    public partial class MinTipSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MinTips",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinTips",
                table: "AspNetUsers");
        }
    }
}
