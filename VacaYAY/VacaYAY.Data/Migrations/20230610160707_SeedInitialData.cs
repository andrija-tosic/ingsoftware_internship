using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VacaYAY.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa", null, "Administrator", "Administrator" },
                    { "dddddddd-dddd-dddd-dddd-dddddddddddd", null, "Default", "Default" }
                });

            migrationBuilder.InsertData(
                table: "Positions",
                columns: new[] { "Id", "Caption", "Description" },
                values: new object[,]
                {
                    { 1, "HR", "Human Resources" },
                    { 2, "iOS Developer", "Apple user" },
                    { 3, "Android Developer", "Android user" },
                    { 4, "MVC Intern", "Lizard" },
                    { 5, "Java Intern", "Also lizard" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "DaysOffNumber", "DeleteDate", "Email", "EmailConfirmed", "EmploymentEndDate", "EmploymentStartDate", "FirstName", "IdNumber", "InsertDate", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PositionId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "11111111-1111-1111-1111-111111111111", 0, "Address", null, 20, null, "andrija@gmail.com", true, new DateTime(2024, 6, 9, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 6, 10, 0, 0, 0, 0, DateTimeKind.Local), "Andrija", "10000", new DateTime(2023, 6, 10, 18, 7, 7, 646, DateTimeKind.Local).AddTicks(2605), "Tošić", true, null, "andrija@gmail.com", "andrija@gmail.com", "AQAAAAIAAYagAAAAEHDpunlJsypwUaV2CQnD749OGkZ36sYLLJpLvXCfKM+bhRiHwuydY7syegptzravlg==", null, false, 4, "", false, "andrija@gmail.com" },
                    { "923316b1-55f2-4839-96c5-679841e02aff", 0, "Address", null, 20, null, "admin@outlook.com", true, new DateTime(2024, 6, 9, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 6, 10, 0, 0, 0, 0, DateTimeKind.Local), "Administrator", "12345", new DateTime(2023, 6, 10, 18, 7, 7, 570, DateTimeKind.Local).AddTicks(3950), "Outlook", true, null, "admin@outlook.com", "admin@outlook.com", "AQAAAAIAAYagAAAAEOfXLvYbjsZzmjan4egorJo39cNegXd6/GLYcVgMVSEWDi6gZXgqIfx+bs1qOkG5kA==", null, false, 1, "", false, "admin@outlook.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "dddddddd-dddd-dddd-dddd-dddddddddddd", "11111111-1111-1111-1111-111111111111" },
                    { "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa", "923316b1-55f2-4839-96c5-679841e02aff" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "dddddddd-dddd-dddd-dddd-dddddddddddd", "11111111-1111-1111-1111-111111111111" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa", "923316b1-55f2-4839-96c5-679841e02aff" });

            migrationBuilder.DeleteData(
                table: "Positions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Positions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Positions",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dddddddd-dddd-dddd-dddd-dddddddddddd");

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "11111111-1111-1111-1111-111111111111");

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "923316b1-55f2-4839-96c5-679841e02aff");

            migrationBuilder.DeleteData(
                table: "Positions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Positions",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
