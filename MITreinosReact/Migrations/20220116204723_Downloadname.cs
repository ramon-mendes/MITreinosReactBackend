using Microsoft.EntityFrameworkCore.Migrations;

namespace MITreinosReact.Migrations
{
    public partial class Downloadname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "CourseLessonDownloads",
                newName: "Size");

            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "CourseLessonDownloads",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Filename",
                table: "CourseLessonDownloads",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Extension",
                table: "CourseLessonDownloads");

            migrationBuilder.DropColumn(
                name: "Filename",
                table: "CourseLessonDownloads");

            migrationBuilder.RenameColumn(
                name: "Size",
                table: "CourseLessonDownloads",
                newName: "Title");
        }
    }
}
