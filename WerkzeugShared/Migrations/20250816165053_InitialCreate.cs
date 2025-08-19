using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WerkzeugShared.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Benutzer",
                columns: table => new
                {
                    Benutzername = table.Column<string>(type: "TEXT", nullable: false),
                    Passwort = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Benutzer", x => x.Benutzername);
                });

            migrationBuilder.CreateTable(
                name: "Projekte",
                columns: table => new
                {
                    ProjektAddresse = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projekte", x => x.ProjektAddresse);
                });

            migrationBuilder.CreateTable(
                name: "Tools",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ToolTypeCounts = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tools", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Werkzeuge",
                columns: table => new
                {
                    WerkzeugId = table.Column<string>(type: "TEXT", nullable: false),
                    Marke = table.Column<string>(type: "TEXT", nullable: true),
                    Art = table.Column<string>(type: "TEXT", nullable: false),
                    Beschreibung = table.Column<string>(type: "TEXT", nullable: true),
                    ProjektAdresse = table.Column<string>(type: "TEXT", nullable: true),
                    History = table.Column<string>(type: "TEXT", nullable: true),
                    Lager = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Werkzeuge", x => x.WerkzeugId);
                    table.ForeignKey(
                        name: "FK_Werkzeuge_Projekte_ProjektAdresse",
                        column: x => x.ProjektAdresse,
                        principalTable: "Projekte",
                        principalColumn: "ProjektAddresse");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Werkzeuge_ProjektAdresse",
                table: "Werkzeuge",
                column: "ProjektAdresse");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Benutzer");

            migrationBuilder.DropTable(
                name: "Tools");

            migrationBuilder.DropTable(
                name: "Werkzeuge");

            migrationBuilder.DropTable(
                name: "Projekte");
        }
    }
}
