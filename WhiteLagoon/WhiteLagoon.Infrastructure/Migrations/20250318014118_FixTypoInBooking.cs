using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhiteLagoon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixTypoInBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StripePaymenIntenttId",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StripePaymenIntenttId",
                table: "Bookings");
        }
    }
}
