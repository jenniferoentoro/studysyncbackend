using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace source_service.Migrations
{
    /// <inheritdoc />
    public partial class removeFileType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileType",
                table: "Sources",
                newName: "FileName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Sources",
                newName: "FileType");
        }
    }
}
