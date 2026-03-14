using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarSearchConsoleOAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "search_console_metricas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BlogDominioId = table.Column<Guid>(type: "uuid", nullable: false),
                    Data = table.Column<DateOnly>(type: "date", nullable: false),
                    TipoBusca = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Impressoes = table.Column<long>(type: "bigint", nullable: false),
                    Cliques = table.Column<long>(type: "bigint", nullable: false),
                    Ctr = table.Column<double>(type: "double precision", nullable: false),
                    PosicaoMedia = table.Column<double>(type: "double precision", nullable: false),
                    DataSincronizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_search_console_metricas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_search_console_metricas_blog_dominios_BlogDominioId",
                        column: x => x.BlogDominioId,
                        principalTable: "blog_dominios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "search_console_oauth",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    RefreshToken = table.Column<string>(type: "text", nullable: false),
                    EmailGoogle = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    DataVinculo = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_search_console_oauth", x => x.Id);
                    table.ForeignKey(
                        name: "FK_search_console_oauth_usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_search_console_metricas_BlogDominioId_Data_TipoBusca",
                table: "search_console_metricas",
                columns: new[] { "BlogDominioId", "Data", "TipoBusca" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_search_console_oauth_UsuarioId",
                table: "search_console_oauth",
                column: "UsuarioId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "search_console_metricas");

            migrationBuilder.DropTable(
                name: "search_console_oauth");
        }
    }
}
