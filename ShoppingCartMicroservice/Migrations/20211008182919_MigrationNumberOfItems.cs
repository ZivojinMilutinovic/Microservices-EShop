using Microsoft.EntityFrameworkCore.Migrations;

namespace ShoppingCartMicroservice.Migrations
{
    public partial class MigrationNumberOfItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfItems",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfItems",
                table: "Product");
        }
    }
}
