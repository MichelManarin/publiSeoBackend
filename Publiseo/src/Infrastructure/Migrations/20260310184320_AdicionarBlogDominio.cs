using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarBlogDominio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "blog_dominios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BlogId = table.Column<Guid>(type: "uuid", nullable: false),
                    NomeDominio = table.Column<string>(type: "character varying(253)", maxLength: 253, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blog_dominios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_blog_dominios_blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "blogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_blog_dominios_BlogId_NomeDominio",
                table: "blog_dominios",
                columns: new[] { "BlogId", "NomeDominio" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "blog_dominios");
        }
    }
}
