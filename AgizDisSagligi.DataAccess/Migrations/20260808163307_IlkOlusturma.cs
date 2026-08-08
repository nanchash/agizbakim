using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgizDisSagligi.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class IlkOlusturma : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kullanicilar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Mail = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ParolaSifreli = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdSoyad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DogumTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KayitTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanicilar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Oneriler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Metin = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Oneriler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hedefler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullaniciId = table.Column<int>(type: "int", nullable: false),
                    Baslik = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PeriyotZaman = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PeriyotSiklik = table.Column<int>(type: "int", nullable: false),
                    OnemDerecesi = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hedefler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hedefler_Kullanicilar_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullaniciId = table.Column<int>(type: "int", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GorselYolu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EklenmeTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notlar_Kullanicilar_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DurumKayitlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HedefId = table.Column<int>(type: "int", nullable: false),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Saat = table.Column<TimeSpan>(type: "time", nullable: false),
                    Sure = table.Column<int>(type: "int", nullable: false),
                    Uygulandi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DurumKayitlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DurumKayitlari_Hedefler_HedefId",
                        column: x => x.HedefId,
                        principalTable: "Hedefler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DurumKayitlari_HedefId",
                table: "DurumKayitlari",
                column: "HedefId");

            migrationBuilder.CreateIndex(
                name: "IX_Hedefler_KullaniciId",
                table: "Hedefler",
                column: "KullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_Kullanicilar_Mail",
                table: "Kullanicilar",
                column: "Mail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notlar_KullaniciId",
                table: "Notlar",
                column: "KullaniciId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DurumKayitlari");

            migrationBuilder.DropTable(
                name: "Notlar");

            migrationBuilder.DropTable(
                name: "Oneriler");

            migrationBuilder.DropTable(
                name: "Hedefler");

            migrationBuilder.DropTable(
                name: "Kullanicilar");
        }
    }
}
