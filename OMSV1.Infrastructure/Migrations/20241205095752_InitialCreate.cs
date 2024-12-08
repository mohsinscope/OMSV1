using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OMSV1.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DamagedDeviceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DamagedDeviceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DamagedTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DamagedTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Governorates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Governorates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Offices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Code = table.Column<int>(type: "integer", nullable: false),
                    ReceivingStaff = table.Column<int>(type: "integer", nullable: false),
                    AccountStaff = table.Column<int>(type: "integer", nullable: false),
                    PrintingStaff = table.Column<int>(type: "integer", nullable: false),
                    QualityStaff = table.Column<int>(type: "integer", nullable: false),
                    DeliveryStaff = table.Column<int>(type: "integer", nullable: false),
                    GovernorateId = table.Column<int>(type: "integer", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offices_Governorates_GovernorateId",
                        column: x => x.GovernorateId,
                        principalTable: "Governorates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ReceivingStaff = table.Column<int>(type: "integer", nullable: false),
                    AccountStaff = table.Column<int>(type: "integer", nullable: false),
                    PrintingStaff = table.Column<int>(type: "integer", nullable: false),
                    QualityStaff = table.Column<int>(type: "integer", nullable: false),
                    DeliveryStaff = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Note = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    WorkingHours = table.Column<int>(type: "integer", nullable: false),
                    OfficeId = table.Column<int>(type: "integer", nullable: false),
                    GovernorateId = table.Column<int>(type: "integer", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attendances_Governorates_GovernorateId",
                        column: x => x.GovernorateId,
                        principalTable: "Governorates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Attendances_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DamagedDevices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SerialNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DamagedDeviceTypeId = table.Column<int>(type: "integer", nullable: false),
                    DeviceTypeId = table.Column<int>(type: "integer", nullable: false),
                    OfficeId = table.Column<int>(type: "integer", nullable: false),
                    GovernorateId = table.Column<int>(type: "integer", nullable: false),
                    ProfileId = table.Column<int>(type: "integer", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DamagedDevices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DamagedDevices_DamagedDeviceTypes_DamagedDeviceTypeId",
                        column: x => x.DamagedDeviceTypeId,
                        principalTable: "DamagedDeviceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DamagedDevices_DeviceTypes_DeviceTypeId",
                        column: x => x.DeviceTypeId,
                        principalTable: "DeviceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DamagedDevices_Governorates_GovernorateId",
                        column: x => x.GovernorateId,
                        principalTable: "Governorates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DamagedDevices_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DamagedPassports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PassportNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DamagedTypeId = table.Column<int>(type: "integer", nullable: false),
                    OfficeId = table.Column<int>(type: "integer", nullable: false),
                    GovernorateId = table.Column<int>(type: "integer", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DamagedPassports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DamagedPassports_DamagedTypes_DamagedTypeId",
                        column: x => x.DamagedTypeId,
                        principalTable: "DamagedTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DamagedPassports_Governorates_GovernorateId",
                        column: x => x.GovernorateId,
                        principalTable: "Governorates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DamagedPassports_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AttachmentCUs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FilePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    EntityType = table.Column<string>(type: "text", nullable: false),
                    EntityId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DamagedDeviceId = table.Column<int>(type: "integer", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentCUs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttachmentCUs_DamagedDevices_DamagedDeviceId",
                        column: x => x.DamagedDeviceId,
                        principalTable: "DamagedDevices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DamagedDevice_Attachments",
                        column: x => x.EntityId,
                        principalTable: "DamagedDevices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentCUs_DamagedDeviceId",
                table: "AttachmentCUs",
                column: "DamagedDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentCUs_EntityId",
                table: "AttachmentCUs",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_GovernorateId",
                table: "Attendances",
                column: "GovernorateId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_OfficeId",
                table: "Attendances",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_DamagedDevices_DamagedDeviceTypeId",
                table: "DamagedDevices",
                column: "DamagedDeviceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DamagedDevices_DeviceTypeId",
                table: "DamagedDevices",
                column: "DeviceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DamagedDevices_GovernorateId",
                table: "DamagedDevices",
                column: "GovernorateId");

            migrationBuilder.CreateIndex(
                name: "IX_DamagedDevices_OfficeId",
                table: "DamagedDevices",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_DamagedPassports_DamagedTypeId",
                table: "DamagedPassports",
                column: "DamagedTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DamagedPassports_GovernorateId",
                table: "DamagedPassports",
                column: "GovernorateId");

            migrationBuilder.CreateIndex(
                name: "IX_DamagedPassports_OfficeId",
                table: "DamagedPassports",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_Offices_GovernorateId",
                table: "Offices",
                column: "GovernorateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttachmentCUs");

            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "DamagedPassports");

            migrationBuilder.DropTable(
                name: "DamagedDevices");

            migrationBuilder.DropTable(
                name: "DamagedTypes");

            migrationBuilder.DropTable(
                name: "DamagedDeviceTypes");

            migrationBuilder.DropTable(
                name: "DeviceTypes");

            migrationBuilder.DropTable(
                name: "Offices");

            migrationBuilder.DropTable(
                name: "Governorates");
        }
    }
}
