using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ArtigoStatusGeracaoETentativas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusGeracao",
                table: "artigos",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TentativasGeracao",
                table: "artigos",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusGeracao",
                table: "artigos");

            migrationBuilder.DropColumn(
                name: "TentativasGeracao",
                table: "artigos");
        }
    }
}
