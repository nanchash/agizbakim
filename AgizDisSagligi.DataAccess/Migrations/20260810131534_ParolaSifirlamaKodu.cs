using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgizDisSagligi.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ParolaSifirlamaKodu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParolaSifirlamaKodu",
                table: "Kullanicilar",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ParolaSifirlamaKoduGecerlilik",
                table: "Kullanicilar",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParolaSifirlamaKodu",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "ParolaSifirlamaKoduGecerlilik",
                table: "Kullanicilar");
        }
    }
}
