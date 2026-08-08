using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AgizDisSagligi.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class OneriSeedVerisi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Oneriler",
                columns: new[] { "Id", "Metin" },
                values: new object[,]
                {
                    { 1, "Dişlerinizi günde en az iki kez, 2 dakika süreyle fırçalayın." },
                    { 2, "Diş ipini her gün kullanmayı unutmayın." },
                    { 3, "Fırçanızı her 3 ayda bir değiştirin." },
                    { 4, "Şekerli ve asitli içeceklerin tüketimini azaltın." },
                    { 5, "Yılda en az bir kez diş hekimi kontrolüne gidin." },
                    { 6, "Fırçalamadan sonra ağız gargarası kullanmayı düşünün." },
                    { 7, "Sert kıllı yerine yumuşak kıllı diş fırçası tercih edin." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Oneriler",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Oneriler",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Oneriler",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Oneriler",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Oneriler",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Oneriler",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Oneriler",
                keyColumn: "Id",
                keyValue: 7);
        }
    }
}
