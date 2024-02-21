using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Notes.Migrations
{
    /// <inheritdoc />
    public partial class NoteFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NoteFileID",
                table: "tb_notes",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tb_files",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PathFile = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_files", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_notes_NoteFileID",
                table: "tb_notes",
                column: "NoteFileID");

            migrationBuilder.CreateIndex(
                name: "IX_tb_files_PathFile",
                table: "tb_files",
                column: "PathFile",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_notes_tb_files_NoteFileID",
                table: "tb_notes",
                column: "NoteFileID",
                principalTable: "tb_files",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_notes_tb_files_NoteFileID",
                table: "tb_notes");

            migrationBuilder.DropTable(
                name: "tb_files");

            migrationBuilder.DropIndex(
                name: "IX_tb_notes_NoteFileID",
                table: "tb_notes");

            migrationBuilder.DropColumn(
                name: "NoteFileID",
                table: "tb_notes");
        }
    }
}
