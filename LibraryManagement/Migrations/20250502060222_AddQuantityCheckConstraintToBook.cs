using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddQuantityCheckConstraintToBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Book_Quantity_Range",
                table: "Books");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Books_Quantity_Range",
                table: "Books",
                sql: "[Quantity] >= 0 AND [Quantity] <= 100");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Books_Quantity_Range",
                table: "Books");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Book_Quantity_Range",
                table: "Books",
                sql: "[Quantity] >= 0 AND [Quantity] <= 100");
        }
    }
}
