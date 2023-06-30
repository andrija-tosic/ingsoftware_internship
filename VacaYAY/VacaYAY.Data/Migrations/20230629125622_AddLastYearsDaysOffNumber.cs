using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacaYAY.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLastYearsDaysOffNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LastYearsDaysTakenOffNumber",
                table: "VacationReviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LastYearsDaysOffNumber",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "000f4869-1f3a-4d63-a92d-6fa8753aa353",
                column: "LastYearsDaysOffNumber",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "069fe119-c139-4d6f-a4d5-0bc90540339f",
                column: "LastYearsDaysOffNumber",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "19eadb6f-7ed7-4acd-9bf4-26825f5619a7",
                column: "LastYearsDaysOffNumber",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "24105a5d-752a-4f0d-b992-787388f159bf",
                column: "LastYearsDaysOffNumber",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "2f409517-b274-44ea-9380-c57fff02871d",
                column: "LastYearsDaysOffNumber",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "335ef315-05f4-4d87-a678-05ec02de608f",
                column: "LastYearsDaysOffNumber",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "38f4bcd5-eab1-45a7-9929-2fb8550dbe57",
                column: "LastYearsDaysOffNumber",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "3e21a209-9d4c-44e3-8e97-f7ed042c9c56",
                column: "LastYearsDaysOffNumber",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "aac218f6-96c4-47c0-b231-310b7f7f6a85",
                column: "LastYearsDaysOffNumber",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "ca0a5ad3-689c-4915-8261-01a67889e664",
                column: "LastYearsDaysOffNumber",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "cc89ca61-a1bf-42a0-b4b6-a2c548ffb6a6",
                column: "LastYearsDaysOffNumber",
                value: 0);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Employees_LastYearsDaysOffNumber_Range",
                table: "Employees",
                sql: "[LastYearsDaysOffNumber] >= 0 AND [LastYearsDaysOffNumber] <= 2147483647");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_Number",
                table: "Contracts",
                column: "Number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Employees_LastYearsDaysOffNumber_Range",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_Number",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "LastYearsDaysTakenOffNumber",
                table: "VacationReviews");

            migrationBuilder.DropColumn(
                name: "LastYearsDaysOffNumber",
                table: "Employees");
        }
    }
}
