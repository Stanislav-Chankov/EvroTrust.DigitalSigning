using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvroTrust.DigitalSigning.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MoreProsAndReferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignedBy",
                table: "CodingTasks",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "CandidateId",
                table: "CodingTasks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "CandidateEmail",
                table: "CodeSolutions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CandidateFullName",
                table: "CodeSolutions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "CodeSolutions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileType",
                table: "CodeSolutions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReviewNotes",
                table: "CodeSolutions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReviewedAt",
                table: "CodeSolutions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reviewer",
                table: "CodeSolutions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "CodeSolutions",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CodingTasks_CandidateId",
                table: "CodingTasks",
                column: "CandidateId");

            migrationBuilder.AddForeignKey(
                name: "FK_CodingTasks_Candidates_CandidateId",
                table: "CodingTasks",
                column: "CandidateId",
                principalTable: "Candidates",
                principalColumn: "CandidateId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CodingTasks_Candidates_CandidateId",
                table: "CodingTasks");

            migrationBuilder.DropIndex(
                name: "IX_CodingTasks_CandidateId",
                table: "CodingTasks");

            migrationBuilder.DropColumn(
                name: "AssignedBy",
                table: "CodingTasks");

            migrationBuilder.DropColumn(
                name: "CandidateId",
                table: "CodingTasks");

            migrationBuilder.DropColumn(
                name: "CandidateEmail",
                table: "CodeSolutions");

            migrationBuilder.DropColumn(
                name: "CandidateFullName",
                table: "CodeSolutions");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "CodeSolutions");

            migrationBuilder.DropColumn(
                name: "FileType",
                table: "CodeSolutions");

            migrationBuilder.DropColumn(
                name: "ReviewNotes",
                table: "CodeSolutions");

            migrationBuilder.DropColumn(
                name: "ReviewedAt",
                table: "CodeSolutions");

            migrationBuilder.DropColumn(
                name: "Reviewer",
                table: "CodeSolutions");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "CodeSolutions");
        }
    }
}
