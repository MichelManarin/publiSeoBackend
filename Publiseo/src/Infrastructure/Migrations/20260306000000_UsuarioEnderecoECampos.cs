using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UsuarioEnderecoECampos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Sobrenome",
                table: "usuarios",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "usuarios",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Endereco",
                table: "usuarios",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cidade",
                table: "usuarios",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "usuarios",
                type: "character varying(2)",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodigoPostal",
                table: "usuarios",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pais",
                table: "usuarios",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "Brasil");

            migrationBuilder.Sql("UPDATE usuarios SET \"Email\" = \"Login\" WHERE \"Email\" IS NULL;");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "usuarios",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "usuarios",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Login",
                table: "usuarios",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_Email",
                table: "usuarios",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_usuarios_Email",
                table: "usuarios");

            migrationBuilder.AlterColumn<string>(
                name: "Login",
                table: "usuarios",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "usuarios",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: false);

            migrationBuilder.DropColumn(
                name: "Sobrenome",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "Endereco",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "Cidade",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "CodigoPostal",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "Pais",
                table: "usuarios");
        }
    }
}
