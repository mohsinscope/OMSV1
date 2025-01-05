using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OMSV1.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Attachemended : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Lectures_Title",
                table: "Lectures",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_DamagedPassports_PassportNumber",
                table: "DamagedPassports",
                column: "PassportNumber");

            migrationBuilder.CreateIndex(
                name: "IX_DamagedDevices_SerialNumber",
                table: "DamagedDevices",
                column: "SerialNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Lectures_Title",
                table: "Lectures");

            migrationBuilder.DropIndex(
                name: "IX_DamagedPassports_PassportNumber",
                table: "DamagedPassports");

            migrationBuilder.DropIndex(
                name: "IX_DamagedDevices_SerialNumber",
                table: "DamagedDevices");
        }
    }
}
