using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BlogObjetivoEProdutoVinculado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DescricaoProdutoVinculado",
                table: "blogs",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObjetivoFinal",
                table: "blogs",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PossuiProdutoVinculado",
                table: "blogs",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescricaoProdutoVinculado",
                table: "blogs");

            migrationBuilder.DropColumn(
                name: "ObjetivoFinal",
                table: "blogs");

            migrationBuilder.DropColumn(
                name: "PossuiProdutoVinculado",
                table: "blogs");
        }
    }
}
