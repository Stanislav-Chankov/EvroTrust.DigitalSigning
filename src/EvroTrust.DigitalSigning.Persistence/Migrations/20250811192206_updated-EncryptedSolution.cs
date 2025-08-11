using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvroTrust.DigitalSigning.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updatedEncryptedSolution : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EncryptedSolution",
                table: "CodeSolutions",
                type: "text",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "bytea");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "EncryptedSolution",
                table: "CodeSolutions",
                type: "bytea",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
