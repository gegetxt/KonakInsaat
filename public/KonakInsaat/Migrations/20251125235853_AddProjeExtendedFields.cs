using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KonakInsaat.Migrations
{
    /// <inheritdoc />
    public partial class AddProjeExtendedFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Adres",
                table: "Projeler",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BuildYear",
                table: "Projeler",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DaireTipleriABlok",
                table: "Projeler",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DaireTipleriBBlok",
                table: "Projeler",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GoogleMapsUrl",
                table: "Projeler",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrtakAlanlar",
                table: "Projeler",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProjeBaslangicTarihi",
                table: "Projeler",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProjeTamamlanmaTarihi",
                table: "Projeler",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telefon1",
                table: "Projeler",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telefon1Diller",
                table: "Projeler",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telefon2",
                table: "Projeler",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telefon2Diller",
                table: "Projeler",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Adres",
                table: "Projeler");

            migrationBuilder.DropColumn(
                name: "BuildYear",
                table: "Projeler");

            migrationBuilder.DropColumn(
                name: "DaireTipleriABlok",
                table: "Projeler");

            migrationBuilder.DropColumn(
                name: "DaireTipleriBBlok",
                table: "Projeler");

            migrationBuilder.DropColumn(
                name: "GoogleMapsUrl",
                table: "Projeler");

            migrationBuilder.DropColumn(
                name: "OrtakAlanlar",
                table: "Projeler");

            migrationBuilder.DropColumn(
                name: "ProjeBaslangicTarihi",
                table: "Projeler");

            migrationBuilder.DropColumn(
                name: "ProjeTamamlanmaTarihi",
                table: "Projeler");

            migrationBuilder.DropColumn(
                name: "Telefon1",
                table: "Projeler");

            migrationBuilder.DropColumn(
                name: "Telefon1Diller",
                table: "Projeler");

            migrationBuilder.DropColumn(
                name: "Telefon2",
                table: "Projeler");

            migrationBuilder.DropColumn(
                name: "Telefon2Diller",
                table: "Projeler");
        }
    }
}
