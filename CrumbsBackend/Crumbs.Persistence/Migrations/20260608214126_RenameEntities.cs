using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crumbs.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StockId",
                table: "Products",
                newName: "Stock");

            migrationBuilder.RenameColumn(
                name: "StripePaymentId",
                table: "Payments",
                newName: "MolliePaymentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Stock",
                table: "Products",
                newName: "StockId");

            migrationBuilder.RenameColumn(
                name: "MolliePaymentId",
                table: "Payments",
                newName: "StripePaymentId");
        }
    }
}
