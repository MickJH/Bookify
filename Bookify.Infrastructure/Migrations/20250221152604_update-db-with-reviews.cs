using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatedbwithreviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_reviews_appartments_apartment_id",
                table: "reviews");

            migrationBuilder.RenameColumn(
                name: "apartment_id",
                table: "reviews",
                newName: "appartment_id");

            migrationBuilder.RenameIndex(
                name: "ix_reviews_apartment_id",
                table: "reviews",
                newName: "ix_reviews_appartment_id");

            migrationBuilder.AddForeignKey(
                name: "fk_reviews_appartments_appartment_id",
                table: "reviews",
                column: "appartment_id",
                principalTable: "appartments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_reviews_appartments_appartment_id",
                table: "reviews");

            migrationBuilder.RenameColumn(
                name: "appartment_id",
                table: "reviews",
                newName: "apartment_id");

            migrationBuilder.RenameIndex(
                name: "ix_reviews_appartment_id",
                table: "reviews",
                newName: "ix_reviews_apartment_id");

            migrationBuilder.AddForeignKey(
                name: "fk_reviews_appartments_apartment_id",
                table: "reviews",
                column: "apartment_id",
                principalTable: "appartments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
