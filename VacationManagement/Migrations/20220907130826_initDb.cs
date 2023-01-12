using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacationManagement.Migrations
{
    public partial class initDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "backgroundColor",
                table: "VacationTypes",
                newName: "BackgroundColor");

            migrationBuilder.RenameColumn(
                name: "VactionBalance",
                table: "Employees",
                newName: "VacationBalance");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Employees",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BackgroundColor",
                table: "VacationTypes",
                newName: "backgroundColor");

            migrationBuilder.RenameColumn(
                name: "VacationBalance",
                table: "Employees",
                newName: "VactionBalance");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);
        }
    }
}
