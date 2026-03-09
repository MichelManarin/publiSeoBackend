using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DominioCamposCompraExpiracao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataExpiracao",
                table: "dominios",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.Sql(
                "UPDATE dominios SET \"DataExpiracao\" = \"DataCompra\" + INTERVAL '1 year' WHERE \"DataExpiracao\" IS NULL;");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataExpiracao",
                table: "dominios",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "dominios",
                type: "numeric(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Moeda",
                table: "dominios",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PeriodoAnos",
                table: "dominios",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<bool>(
                name: "Privacy",
                table: "dominios",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RenewAuto",
                table: "dominios",
                type: "boolean",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "DataExpiracao", table: "dominios");
            migrationBuilder.DropColumn(name: "Total", table: "dominios");
            migrationBuilder.DropColumn(name: "Moeda", table: "dominios");
            migrationBuilder.DropColumn(name: "PeriodoAnos", table: "dominios");
            migrationBuilder.DropColumn(name: "Privacy", table: "dominios");
            migrationBuilder.DropColumn(name: "RenewAuto", table: "dominios");
        }
    }
}
