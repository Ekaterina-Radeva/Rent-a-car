using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rent_a_car.Data.Migrations
{
    public partial class CreateRentalRequestsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RentalRequest",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    CarId = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RentalRequest_Car_CarId",
                        column: x => x.CarId,
                        principalTable: "Car",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RentalRequest_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RentalRequest_CarId",
                table: "RentalRequest",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalRequest_UserId",
                table: "RentalRequest",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RentalRequest");
        }
    }
}
