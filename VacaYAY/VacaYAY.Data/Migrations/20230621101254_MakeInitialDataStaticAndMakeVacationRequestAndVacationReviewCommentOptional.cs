using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacaYAY.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeInitialDataStaticAndMakeVacationRequestAndVacationReviewCommentOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "VacationReviews",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(512)",
                oldMaxLength: 512);

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "VacationRequests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "000f4869-1f3a-4d63-a92d-6fa8753aa353",
                columns: new[] { "EmploymentEndDate", "EmploymentStartDate", "InsertDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "AQAAAAIAAYagAAAAELBhGKQIekty8VzCyPnzBK8uFvx5mqUY+L0LNJ0fi616XYT/nVK9DfbIFFIotkyoHQ==" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "069fe119-c139-4d6f-a4d5-0bc90540339f",
                columns: new[] { "EmploymentEndDate", "EmploymentStartDate", "InsertDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "AQAAAAIAAYagAAAAELBhGKQIekty8VzCyPnzBK8uFvx5mqUY+L0LNJ0fi616XYT/nVK9DfbIFFIotkyoHQ==" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "19eadb6f-7ed7-4acd-9bf4-26825f5619a7",
                columns: new[] { "Email", "EmploymentEndDate", "EmploymentStartDate", "InsertDate", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "UserName" },
                values: new object[] { "andrija.tosic@ingsoftware.com", new DateTime(2024, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "andrija.tosic@ingsoftware.com", "andrija.tosic@ingsoftware.com", "AQAAAAIAAYagAAAAELBhGKQIekty8VzCyPnzBK8uFvx5mqUY+L0LNJ0fi616XYT/nVK9DfbIFFIotkyoHQ==", "andrija.tosic@ingsoftware.com" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "24105a5d-752a-4f0d-b992-787388f159bf",
                columns: new[] { "EmploymentEndDate", "EmploymentStartDate", "InsertDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "AQAAAAIAAYagAAAAELBhGKQIekty8VzCyPnzBK8uFvx5mqUY+L0LNJ0fi616XYT/nVK9DfbIFFIotkyoHQ==" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "2f409517-b274-44ea-9380-c57fff02871d",
                columns: new[] { "EmploymentEndDate", "EmploymentStartDate", "InsertDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "AQAAAAIAAYagAAAAELBhGKQIekty8VzCyPnzBK8uFvx5mqUY+L0LNJ0fi616XYT/nVK9DfbIFFIotkyoHQ==" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "335ef315-05f4-4d87-a678-05ec02de608f",
                columns: new[] { "EmploymentEndDate", "EmploymentStartDate", "InsertDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "AQAAAAIAAYagAAAAELBhGKQIekty8VzCyPnzBK8uFvx5mqUY+L0LNJ0fi616XYT/nVK9DfbIFFIotkyoHQ==" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "38f4bcd5-eab1-45a7-9929-2fb8550dbe57",
                columns: new[] { "EmploymentEndDate", "EmploymentStartDate", "InsertDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "AQAAAAIAAYagAAAAELBhGKQIekty8VzCyPnzBK8uFvx5mqUY+L0LNJ0fi616XYT/nVK9DfbIFFIotkyoHQ==" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "3e21a209-9d4c-44e3-8e97-f7ed042c9c56",
                columns: new[] { "EmploymentEndDate", "EmploymentStartDate", "InsertDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "AQAAAAIAAYagAAAAELBhGKQIekty8VzCyPnzBK8uFvx5mqUY+L0LNJ0fi616XYT/nVK9DfbIFFIotkyoHQ==" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "aac218f6-96c4-47c0-b231-310b7f7f6a85",
                columns: new[] { "EmploymentEndDate", "EmploymentStartDate", "InsertDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "AQAAAAIAAYagAAAAELBhGKQIekty8VzCyPnzBK8uFvx5mqUY+L0LNJ0fi616XYT/nVK9DfbIFFIotkyoHQ==" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "ca0a5ad3-689c-4915-8261-01a67889e664",
                columns: new[] { "EmploymentEndDate", "EmploymentStartDate", "InsertDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "AQAAAAIAAYagAAAAELBhGKQIekty8VzCyPnzBK8uFvx5mqUY+L0LNJ0fi616XYT/nVK9DfbIFFIotkyoHQ==" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "cc89ca61-a1bf-42a0-b4b6-a2c548ffb6a6",
                columns: new[] { "EmploymentEndDate", "EmploymentStartDate", "InsertDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "AQAAAAIAAYagAAAAELBhGKQIekty8VzCyPnzBK8uFvx5mqUY+L0LNJ0fi616XYT/nVK9DfbIFFIotkyoHQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "VacationReviews",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "VacationRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "000f4869-1f3a-4d63-a92d-6fa8753aa353",
                columns: new[] { "EmploymentEndDate", "EmploymentStartDate", "InsertDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 6, 14, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 6, 14, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 6, 14, 12, 45, 30, 449, DateTimeKind.Local).AddTicks(9822), "AQAAAAIAAYagAAAAEDi8iOw17uJuGxh7HuFj22ewPruiKcO2n5NPRMeCxNY4Ui4ZfAzbaXFB4irHxRxGZg==" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "069fe119-c139-4d6f-a4d5-0bc90540339f",
                columns: new[] { "EmploymentEndDate", "EmploymentStartDate", "InsertDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 6, 14, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 6, 14, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 6, 14, 12, 45, 30, 833, DateTimeKind.Local).AddTicks(1348), "AQAAAAIAAYagAAAAEN+dp2XYB8LfthZSWG6Cn3LhfGwDzANNYruyOL2282PJv0RnIgB6/ceL9d1bD6zaAw==" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "19eadb6f-7ed7-4acd-9bf4-26825f5619a7",
                columns: new[] { "Email", "EmploymentEndDate", "EmploymentStartDate", "InsertDate", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "UserName" },
                values: new object[] { "andrija@gmail.com", new DateTime(2024, 6, 14, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 6, 14, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 6, 14, 12, 45, 30, 544, DateTimeKind.Local).AddTicks(3294), "andrija@gmail.com", "andrija@gmail.com", "AQAAAAIAAYagAAAAEJ6hGK3sREbO29Ag+q8znTUDdk5Qij3xUF5ce/nhguCP2af+PtSpksWV7oJnKWQy7Q==", "andrija@gmail.com" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "24105a5d-752a-4f0d-b992-787388f159bf",
                columns: new[] { "EmploymentEndDate", "EmploymentStartDate", "InsertDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 6, 14, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 6, 14, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 6, 14, 12, 45, 30, 915, DateTimeKind.Local).AddTicks(4345), "AQAAAAIAAYagAAAAELM7vtTSEst/y3BJfyflN8wafTkGco8gCiV2QPwoJwmMhtjCPlrjzEQhYQqrJEWioA==" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "2f409517-b274-44ea-9380-c57fff02871d",
                columns: new[] { "EmploymentEndDate", "EmploymentStartDate", "InsertDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 6, 14, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 6, 14, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 6, 14, 12, 45, 31, 465, DateTimeKind.Local).AddTicks(8625), "AQAAAAIAAYagAAAAEEVj4VqzDW37gBZ2czciWg1Ylb3n3kEQvVk8+AuWyG6ofCpwVMNNRtSnoErCTw6uDw==" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "335ef315-05f4-4d87-a678-05ec02de608f",
                columns: new[] { "EmploymentEndDate", "EmploymentStartDate", "InsertDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 6, 14, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 6, 14, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 6, 14, 12, 45, 31, 52, DateTimeKind.Local).AddTicks(718), "AQAAAAIAAYagAAAAEAPf5HAZchz3PzIwlQVVzWPuQnyMjs2weOI0ieO8qgyVq7+w4LQlVXNEKROUJQx0Gg==" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "38f4bcd5-eab1-45a7-9929-2fb8550dbe57",
                columns: new[] { "EmploymentEndDate", "EmploymentStartDate", "InsertDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 6, 14, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 6, 14, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 6, 14, 12, 45, 30, 737, DateTimeKind.Local).AddTicks(2682), "AQAAAAIAAYagAAAAEPNEOnIqOjZ70/+h1UiUCHO+TFade5CvsexVEiwsAwSNgfgVJL4pH7onOaVd9PupNw==" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "3e21a209-9d4c-44e3-8e97-f7ed042c9c56",
                columns: new[] { "EmploymentEndDate", "EmploymentStartDate", "InsertDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 6, 14, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 6, 14, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 6, 14, 12, 45, 31, 343, DateTimeKind.Local).AddTicks(7286), "AQAAAAIAAYagAAAAEMKCP2vvELZ2NbRvRh+simaps6fj2c4XCKi2E+zk4v16BH+QRRqwM7Huv0XslPbptA==" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "aac218f6-96c4-47c0-b231-310b7f7f6a85",
                columns: new[] { "EmploymentEndDate", "EmploymentStartDate", "InsertDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 6, 14, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 6, 14, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 6, 14, 12, 45, 30, 645, DateTimeKind.Local).AddTicks(3994), "AQAAAAIAAYagAAAAEMUkAi800mYK+an/5p6+VsolLQqjtliA/prtE1AHzMMZu3iBcdqFVlPBcQdrm4yCYA==" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "ca0a5ad3-689c-4915-8261-01a67889e664",
                columns: new[] { "EmploymentEndDate", "EmploymentStartDate", "InsertDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 6, 14, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 6, 14, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 6, 14, 12, 45, 31, 236, DateTimeKind.Local).AddTicks(5750), "AQAAAAIAAYagAAAAEN0uLXeYPEABjJGdk3h0H0+91So62yzrc7gS5OHBXxAPC+rqWGL4eoieZlwzVerWsg==" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "cc89ca61-a1bf-42a0-b4b6-a2c548ffb6a6",
                columns: new[] { "EmploymentEndDate", "EmploymentStartDate", "InsertDate", "PasswordHash" },
                values: new object[] { new DateTime(2024, 6, 14, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 6, 14, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 6, 14, 12, 45, 31, 147, DateTimeKind.Local).AddTicks(5669), "AQAAAAIAAYagAAAAEEHKHvaqb7znGgvJv3UjxfJ0xIlUdnDrK5jgxArpmzbaaQfFM80eEL6iZ8cKOODSPA==" });
        }
    }
}
