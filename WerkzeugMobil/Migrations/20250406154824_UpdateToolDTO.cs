using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WerkzeugMobil.Migrations
{
    /// <inheritdoc />
    public partial class UpdateToolDTO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Tools",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Tools");
        }
    }
}
