using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WerkzeugMobil.Migrations
{
    /// <inheritdoc />
    public partial class MigrationNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Lager",
                table: "Werkzeuge",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lager",
                table: "Werkzeuge");
        }
    }
}
