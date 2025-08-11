using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvroTrust.DigitalSigning.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MoreFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Candidates",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Candidates",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Candidates",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Candidates",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LinkedInProfile",
                table: "Candidates",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ResumeUrl",
                table: "Candidates",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "LinkedInProfile",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "ResumeUrl",
                table: "Candidates");
        }
    }
}
