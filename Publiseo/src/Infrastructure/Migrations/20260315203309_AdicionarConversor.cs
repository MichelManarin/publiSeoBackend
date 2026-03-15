using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarConversor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "conversores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BlogId = table.Column<Guid>(type: "uuid", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    TextoBotaoInicial = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    TipoFinalizacao = table.Column<int>(type: "integer", nullable: false),
                    MensagemFinalizacao = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    WhatsAppNumero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    WhatsAppTextoPreDefinido = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_conversores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_conversores_blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "blogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "conversor_leads",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ConversorId = table.Column<Guid>(type: "uuid", nullable: false),
                    NomeCompleto = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Telefone = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    RespostasJson = table.Column<string>(type: "jsonb", nullable: false),
                    ArtigoId = table.Column<Guid>(type: "uuid", nullable: true),
                    Ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_conversor_leads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_conversor_leads_conversores_ConversorId",
                        column: x => x.ConversorId,
                        principalTable: "conversores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "conversor_perguntas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ConversorId = table.Column<Guid>(type: "uuid", nullable: false),
                    Ordem = table.Column<int>(type: "integer", nullable: false),
                    TextoPergunta = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    TipoCampo = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_conversor_perguntas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_conversor_perguntas_conversores_ConversorId",
                        column: x => x.ConversorId,
                        principalTable: "conversores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_conversor_leads_ConversorId",
                table: "conversor_leads",
                column: "ConversorId");

            migrationBuilder.CreateIndex(
                name: "IX_conversor_perguntas_ConversorId",
                table: "conversor_perguntas",
                column: "ConversorId");

            migrationBuilder.CreateIndex(
                name: "IX_conversores_BlogId",
                table: "conversores",
                column: "BlogId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "conversor_leads");

            migrationBuilder.DropTable(
                name: "conversor_perguntas");

            migrationBuilder.DropTable(
                name: "conversores");
        }
    }
}
