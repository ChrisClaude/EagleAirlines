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
                    Timezone = table.Column<string>(nullable: true),
                    Dst = table.Column<string>(nullable: true),
                    Tz = table.Column<string>(maxLength: 100, nullable: false),
                    StationType = table.Column<string>(nullable: true),
                    Source = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airport", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Airport");
        }
    }
}
