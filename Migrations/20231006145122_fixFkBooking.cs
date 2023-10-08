using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    public partial class fixFkBooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tb_tr_bookings_room_guid",
                table: "tb_tr_bookings");

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_bookings_room_guid",
                table: "tb_tr_bookings",
                column: "room_guid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tb_tr_bookings_room_guid",
                table: "tb_tr_bookings");

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_bookings_room_guid",
                table: "tb_tr_bookings",
                column: "room_guid",
                unique: true);
        }
    }
}
