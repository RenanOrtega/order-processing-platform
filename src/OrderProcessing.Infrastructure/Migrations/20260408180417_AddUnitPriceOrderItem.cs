using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderProcessing.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUnitPriceOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "unit_price",
                table: "OrderItems",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "unit_price",
                table: "OrderItems");
        }
    }
}
