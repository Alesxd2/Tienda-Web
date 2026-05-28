using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiendaWeb.Migrations
{
    /// <inheritdoc />
    public partial class CervezaUrlImagen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UrlImagen",
                table: "Cervezas",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlImagen",
                table: "Cervezas");
        }
    }
}
