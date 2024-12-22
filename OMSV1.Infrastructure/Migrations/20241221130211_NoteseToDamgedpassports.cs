using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OMSV1.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NoteseToDamgedpassports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "DamagedPassports",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "DamagedPassports");
        }
    }
}
