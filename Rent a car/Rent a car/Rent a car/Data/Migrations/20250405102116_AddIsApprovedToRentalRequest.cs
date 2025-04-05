using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent_a_car.Data.Migrations
{
    public partial class AddIsApprovedToRentalRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "RentalRequest",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "RentalRequest");
        }
    }
}
