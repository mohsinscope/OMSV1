using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OMSV1.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LectureDamagedPas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DamagedPassportId",
                table: "AttachmentCUs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LectureId",
                table: "AttachmentCUs",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Lectures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OfficeId = table.Column<int>(type: "integer", nullable: false),
                    GovernorateId = table.Column<int>(type: "integer", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lectures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lectures_Governorates_GovernorateId",
                        column: x => x.GovernorateId,
                        principalTable: "Governorates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lectures_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentCUs_DamagedPassportId",
                table: "AttachmentCUs",
                column: "DamagedPassportId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentCUs_LectureId",
                table: "AttachmentCUs",
                column: "LectureId");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_GovernorateId",
                table: "Lectures",
                column: "GovernorateId");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_OfficeId",
                table: "Lectures",
                column: "OfficeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AttachmentCUs_DamagedPassports_DamagedPassportId",
                table: "AttachmentCUs",
                column: "DamagedPassportId",
                principalTable: "DamagedPassports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AttachmentCUs_Lectures_LectureId",
                table: "AttachmentCUs",
                column: "LectureId",
                principalTable: "Lectures",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DamagedPassport_Attachments",
                table: "AttachmentCUs",
                column: "EntityId",
                principalTable: "DamagedPassports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lecture_Attachments",
                table: "AttachmentCUs",
                column: "EntityId",
                principalTable: "Lectures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttachmentCUs_DamagedPassports_DamagedPassportId",
                table: "AttachmentCUs");

            migrationBuilder.DropForeignKey(
                name: "FK_AttachmentCUs_Lectures_LectureId",
                table: "AttachmentCUs");

            migrationBuilder.DropForeignKey(
                name: "FK_DamagedPassport_Attachments",
                table: "AttachmentCUs");

            migrationBuilder.DropForeignKey(
                name: "FK_Lecture_Attachments",
                table: "AttachmentCUs");

            migrationBuilder.DropTable(
                name: "Lectures");

            migrationBuilder.DropIndex(
                name: "IX_AttachmentCUs_DamagedPassportId",
                table: "AttachmentCUs");

            migrationBuilder.DropIndex(
                name: "IX_AttachmentCUs_LectureId",
                table: "AttachmentCUs");

            migrationBuilder.DropColumn(
                name: "DamagedPassportId",
                table: "AttachmentCUs");

            migrationBuilder.DropColumn(
                name: "LectureId",
                table: "AttachmentCUs");
        }
    }
}
