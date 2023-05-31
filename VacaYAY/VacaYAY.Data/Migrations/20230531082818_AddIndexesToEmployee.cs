using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacaYAY.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexesToEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Employees_FirstName_LastName_EmploymentStartDate_EmploymentEndDate",
                table: "Employees",
                columns: new[] { "FirstName", "LastName", "EmploymentStartDate", "EmploymentEndDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Employees_FirstName_LastName_EmploymentStartDate_EmploymentEndDate",
                table: "Employees");
        }
    }
}
