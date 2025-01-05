using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OMSV1.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Lecturdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "Lectures",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "LectureTypeId",
                table: "Lectures",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LectureType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LectureType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LectureType_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_CompanyId",
                table: "Lectures",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_LectureTypeId",
                table: "Lectures",
                column: "LectureTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LectureType_CompanyId",
                table: "LectureType",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lectures_Company_CompanyId",
                table: "Lectures",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lectures_LectureType_LectureTypeId",
                table: "Lectures",
                column: "LectureTypeId",
                principalTable: "LectureType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lectures_Company_CompanyId",
                table: "Lectures");

            migrationBuilder.DropForeignKey(
                name: "FK_Lectures_LectureType_LectureTypeId",
                table: "Lectures");

            migrationBuilder.DropTable(
                name: "LectureType");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropIndex(
                name: "IX_Lectures_CompanyId",
                table: "Lectures");

            migrationBuilder.DropIndex(
                name: "IX_Lectures_LectureTypeId",
                table: "Lectures");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Lectures");

            migrationBuilder.DropColumn(
                name: "LectureTypeId",
                table: "Lectures");
        }
    }
}
