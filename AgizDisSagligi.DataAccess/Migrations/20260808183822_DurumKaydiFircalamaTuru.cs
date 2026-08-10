using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgizDisSagligi.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class DurumKaydiFircalamaTuru : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FircalamaTuru",
                table: "DurumKayitlari",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FircalamaTuru",
                table: "DurumKayitlari");
        }
    }
}
