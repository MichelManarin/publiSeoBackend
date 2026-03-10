using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarBlogExternalId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ExternalId",
                table: "blogs",
                type: "uuid",
                nullable: true);

            migrationBuilder.Sql("UPDATE blogs SET \"ExternalId\" = gen_random_uuid() WHERE \"ExternalId\" IS NULL");

            migrationBuilder.AlterColumn<Guid>(
                name: "ExternalId",
                table: "blogs",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_blogs_ExternalId",
                table: "blogs",
                column: "ExternalId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_blogs_ExternalId",
                table: "blogs");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "blogs");
        }
    }
}
