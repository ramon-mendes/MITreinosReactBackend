using Microsoft.EntityFrameworkCore.Migrations;

namespace MITreinosReact.Migrations
{
    public partial class InitialCreate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CoursePageModel",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "JsName", "Slug", "Title" },
                values: new object[] { "orientacoes", "orientacoes", "Orientações" });

            migrationBuilder.UpdateData(
                table: "CoursePageModel",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "JsName", "Slug", "Title" },
                values: new object[] { "orientacoes", "orientacoes", "Orientações" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CoursePageModel",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "JsName", "Slug", "Title" },
                values: new object[] { "filmes", "filmes", "Filmes" });

            migrationBuilder.UpdateData(
                table: "CoursePageModel",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "JsName", "Slug", "Title" },
                values: new object[] { "filmes", "filmes", "Filmes" });
        }
    }
}
