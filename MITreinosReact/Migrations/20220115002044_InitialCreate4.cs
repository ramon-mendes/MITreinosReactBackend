using Microsoft.EntityFrameworkCore.Migrations;

namespace MITreinosReact.Migrations
{
    public partial class InitialCreate4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JsName",
                table: "CoursePageModel");

            migrationBuilder.UpdateData(
                table: "CoursePageModel",
                keyColumn: "Id",
                keyValue: 1,
                column: "Slug",
                value: "ieorientacoes");

            migrationBuilder.UpdateData(
                table: "CoursePageModel",
                keyColumn: "Id",
                keyValue: 2,
                column: "Slug",
                value: "ieorientacoes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JsName",
                table: "CoursePageModel",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CoursePageModel",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "JsName", "Slug" },
                values: new object[] { "ieorientacoes", "orientacoes" });

            migrationBuilder.UpdateData(
                table: "CoursePageModel",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "JsName", "Slug" },
                values: new object[] { "ieorientacoes", "orientacoes" });
        }
    }
}
