using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KonakInsaat.Migrations
{
    /// <inheritdoc />
    public partial class AddBaslikToIletisim : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Baslik",
                table: "Iletisim",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Baslik",
                table: "Iletisim");
        }
    }
}
