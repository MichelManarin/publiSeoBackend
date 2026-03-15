using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarArtigoImagemCapa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagemCapaAttribution",
                table: "artigos",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagemCapaUnsplashId",
                table: "artigos",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagemCapaUrl",
                table: "artigos",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagemCapaAttribution",
                table: "artigos");

            migrationBuilder.DropColumn(
                name: "ImagemCapaUnsplashId",
                table: "artigos");

            migrationBuilder.DropColumn(
                name: "ImagemCapaUrl",
                table: "artigos");
        }
    }
}
