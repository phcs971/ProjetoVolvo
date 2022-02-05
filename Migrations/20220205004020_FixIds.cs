using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoVolvo.Migrations
{
    public partial class FixIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "Sales");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Sales",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SellerId",
                table: "Sales",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
