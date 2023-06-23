using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VacaYAY.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddContractsAndContractTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContractTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    DocumentUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.Id);
                    table.CheckConstraint("CK_Contracts_Number_MinLength", "LEN([Number]) >= 6");
                    table.ForeignKey(
                        name: "FK_Contracts_ContractTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ContractTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contracts_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ContractTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Open-Ended" },
                    { 2, "Full-Time" },
                    { 3, "Part-Time" },
                    { 4, "Fixed-Term" },
                    { 5, "Consultant" },
                    { 6, "Internship" }
                });

            migrationBuilder.InsertData(
                table: "Contracts",
                columns: new[] { "Id", "DocumentUrl", "EmployeeId", "EndDate", "Number", "StartDate", "TypeId" },
                values: new object[,]
                {
                    { 1, "placeholder", "000f4869-1f3a-4d63-a92d-6fa8753aa353", new DateTime(2023, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "100000", new DateTime(2023, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, "placeholder", "000f4869-1f3a-4d63-a92d-6fa8753aa353", new DateTime(2023, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "100001", new DateTime(2023, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_EmployeeId",
                table: "Contracts",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_TypeId",
                table: "Contracts",
                column: "TypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "ContractTypes");
        }
    }
}
