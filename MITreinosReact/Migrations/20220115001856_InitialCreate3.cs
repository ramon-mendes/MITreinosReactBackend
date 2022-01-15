using Microsoft.EntityFrameworkCore.Migrations;

namespace MITreinosReact.Migrations
{
    public partial class InitialCreate3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CoursePageModel",
                keyColumn: "Id",
                keyValue: 1,
                column: "JsName",
                value: "ieorientacoes");

            migrationBuilder.UpdateData(
                table: "CoursePageModel",
                keyColumn: "Id",
                keyValue: 2,
                column: "JsName",
                value: "ieorientacoes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CoursePageModel",
                keyColumn: "Id",
                keyValue: 1,
                column: "JsName",
                value: "orientacoes");

            migrationBuilder.UpdateData(
                table: "CoursePageModel",
                keyColumn: "Id",
                keyValue: 2,
                column: "JsName",
                value: "orientacoes");
        }
    }
}
