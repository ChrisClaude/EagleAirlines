using Microsoft.EntityFrameworkCore.Migrations;

namespace BookingApi.Migrations
{
    public partial class BookingModelModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Flight_FlightId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Passenger_PassengerID",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Seat_Flight_FlightId",
                table: "Seat");

            migrationBuilder.DropColumn(
                name: "PassengeId",
                table: "Booking");

            migrationBuilder.RenameColumn(
                name: "FlightId",
                table: "Seat",
                newName: "FlightID");

            migrationBuilder.RenameIndex(
                name: "IX_Seat_FlightId",
                table: "Seat",
                newName: "IX_Seat_FlightID");

            migrationBuilder.RenameColumn(
                name: "FlightId",
                table: "Booking",
                newName: "FlightID");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_FlightId",
                table: "Booking",
                newName: "IX_Booking_FlightID");

            migrationBuilder.AlterColumn<int>(
                name: "PassengerID",
                table: "Booking",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Flight_FlightID",
                table: "Booking",
                column: "FlightID",
                principalTable: "Flight",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Passenger_PassengerID",
                table: "Booking",
                column: "PassengerID",
                principalTable: "Passenger",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Seat_Flight_FlightID",
                table: "Seat",
                column: "FlightID",
                principalTable: "Flight",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Flight_FlightID",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Passenger_PassengerID",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Seat_Flight_FlightID",
                table: "Seat");

            migrationBuilder.RenameColumn(
                name: "FlightID",
                table: "Seat",
                newName: "FlightId");

            migrationBuilder.RenameIndex(
                name: "IX_Seat_FlightID",
                table: "Seat",
                newName: "IX_Seat_FlightId");

            migrationBuilder.RenameColumn(
                name: "FlightID",
                table: "Booking",
                newName: "FlightId");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_FlightID",
                table: "Booking",
                newName: "IX_Booking_FlightId");

            migrationBuilder.AlterColumn<int>(
                name: "PassengerID",
                table: "Booking",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "PassengeId",
                table: "Booking",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Flight_FlightId",
                table: "Booking",
                column: "FlightId",
                principalTable: "Flight",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Passenger_PassengerID",
                table: "Booking",
                column: "PassengerID",
                principalTable: "Passenger",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Seat_Flight_FlightId",
                table: "Seat",
                column: "FlightId",
                principalTable: "Flight",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
