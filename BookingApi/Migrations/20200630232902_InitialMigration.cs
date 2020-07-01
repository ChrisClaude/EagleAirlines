using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookingApi.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Airport",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 150, nullable: false),
                    City = table.Column<string>(maxLength: 70, nullable: false),
                    Country = table.Column<string>(maxLength: 100, nullable: false),
                    Iata = table.Column<string>(maxLength: 100, nullable: false),
                    Iciao = table.Column<string>(maxLength: 100, nullable: false),
                    Latitude = table.Column<string>(maxLength: 100, nullable: false),
                    Longitude = table.Column<string>(maxLength: 100, nullable: false),
                    Altitude = table.Column<string>(maxLength: 100, nullable: false),
                    Timezone = table.Column<string>(maxLength: 100, nullable: false),
                    Dst = table.Column<string>(maxLength: 100, nullable: false),
                    Tz = table.Column<string>(maxLength: 100, nullable: false),
                    StationType = table.Column<string>(maxLength: 100, nullable: false),
                    Source = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airport", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Flight",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flight", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Passenger",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(maxLength: 70, nullable: false),
                    Surname = table.Column<string>(maxLength: 70, nullable: false),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(maxLength: 70, nullable: false),
                    PassportNumber = table.Column<string>(maxLength: 70, nullable: false),
                    Citizenship = table.Column<string>(maxLength: 70, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passenger", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Departure",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    FlightID = table.Column<int>(nullable: false),
                    AirportID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departure", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Departure_Airport_AirportID",
                        column: x => x.AirportID,
                        principalTable: "Airport",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Departure_Flight_FlightID",
                        column: x => x.FlightID,
                        principalTable: "Flight",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Destination",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    FlightID = table.Column<int>(nullable: false),
                    AirportID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destination", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Destination_Airport_AirportID",
                        column: x => x.AirportID,
                        principalTable: "Airport",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Destination_Flight_FlightID",
                        column: x => x.FlightID,
                        principalTable: "Flight",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Seat",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeatNum = table.Column<string>(nullable: true),
                    Cabin = table.Column<int>(nullable: false),
                    FlightID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seat", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Seat_Flight_FlightID",
                        column: x => x.FlightID,
                        principalTable: "Flight",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Status = table.Column<string>(nullable: true),
                    FlightID = table.Column<int>(nullable: false),
                    PassengerID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Booking_Flight_FlightID",
                        column: x => x.FlightID,
                        principalTable: "Flight",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Booking_Passenger_PassengerID",
                        column: x => x.PassengerID,
                        principalTable: "Passenger",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Booking_FlightID",
                table: "Booking",
                column: "FlightID");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_PassengerID",
                table: "Booking",
                column: "PassengerID");

            migrationBuilder.CreateIndex(
                name: "IX_Departure_AirportID",
                table: "Departure",
                column: "AirportID");

            migrationBuilder.CreateIndex(
                name: "IX_Departure_FlightID",
                table: "Departure",
                column: "FlightID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Destination_AirportID",
                table: "Destination",
                column: "AirportID");

            migrationBuilder.CreateIndex(
                name: "IX_Destination_FlightID",
                table: "Destination",
                column: "FlightID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Seat_FlightID",
                table: "Seat",
                column: "FlightID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "Departure");

            migrationBuilder.DropTable(
                name: "Destination");

            migrationBuilder.DropTable(
                name: "Seat");

            migrationBuilder.DropTable(
                name: "Passenger");

            migrationBuilder.DropTable(
                name: "Airport");

            migrationBuilder.DropTable(
                name: "Flight");
        }
    }
}
