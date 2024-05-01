using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShop24.Migrations
{
    /// <inheritdoc />
    public partial class AddPickUpToOrderModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PickUpPointId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PickUpPointId",
                table: "Orders");
        }
    }
}
