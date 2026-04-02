using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MekkysCakes.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class WishlistFeatureUpdateV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserEmail",
                table: "Wishlists",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdded",
                table: "WishlistItem",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Wishlist_UserEmail",
                table: "Wishlists",
                column: "UserEmail");

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItem_WishlistId_ProductId",
                table: "WishlistItem",
                columns: new[] { "WishlistId", "ProductId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Wishlist_UserEmail",
                table: "Wishlists");

            migrationBuilder.DropIndex(
                name: "IX_WishlistItem_WishlistId_ProductId",
                table: "WishlistItem");

            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "WishlistItem");

            migrationBuilder.AlterColumn<string>(
                name: "UserEmail",
                table: "Wishlists",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
