using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BlogAutorPadraoNome : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutorPadraoUsuarioId",
                table: "blogs");

            migrationBuilder.AddColumn<string>(
                name: "AutorPadraoNome",
                table: "blogs",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutorPadraoNome",
                table: "blogs");

            migrationBuilder.AddColumn<Guid>(
                name: "AutorPadraoUsuarioId",
                table: "blogs",
                type: "uuid",
                nullable: true);
        }
    }
}
