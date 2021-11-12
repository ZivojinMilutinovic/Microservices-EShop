using Microsoft.EntityFrameworkCore.Migrations;

namespace ShoppingCartMicroservice.Migrations
{
    public partial class RenameModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OriginalProductId",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalProductId",
                table: "Product");
        }
    }
}
