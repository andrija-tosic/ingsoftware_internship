using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VacaYAY.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialSeedData : Migration
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
                table: "LeaveTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Days off" },
                    { 2, "Sick leave" },
                    { 3, "Paid leave" },
                    { 4, "Unpaid leave" }
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
                    { "11111111-1111-1111-1111-111111111111", 0, "Svetog Patrijarlimpija 12", null, 20, null, "andrija@gmail.com", true, new DateTime(2024, 6, 11, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 6, 12, 0, 0, 0, 0, DateTimeKind.Local), "Andrija", "10000", new DateTime(2023, 6, 12, 15, 0, 47, 161, DateTimeKind.Local).AddTicks(3667), "Tošić", true, null, "andrija@gmail.com", "andrija@gmail.com", "AQAAAAIAAYagAAAAECKg1DkTYN20sh/jwGE7wwnhrJ6bv2TUeUNuQauPtfT6pKNNtw0+6RT+VpKZ+FBVDA==", null, false, 4, "", false, "andrija@gmail.com" },
                    { "22222222-2222-2222-2222-222222222222", 0, "Dino Mustafić 8", null, 20, null, "papak.potočar@gmail.com", true, new DateTime(2024, 6, 11, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 6, 12, 0, 0, 0, 0, DateTimeKind.Local), "Papak", "10001", new DateTime(2023, 6, 12, 15, 0, 47, 272, DateTimeKind.Local).AddTicks(7358), "Potočar", true, null, "papak.potočar@gmail.com", "papak.potočar@gmail.com", "AQAAAAIAAYagAAAAEM/PFOQpSR3LaX90mhMhKUZrCmPETnAxdJnzW66KfyOObx7df4trDyMkq9F3cPONLQ==", null, false, 5, "", false, "papak.potočar@gmail.com" },
                    { "33333333-3333-3333-3333-333333333333", 0, "S.T.R. Gugleta", null, 20, null, "katrafilov.f@gmail.com", true, new DateTime(2024, 6, 11, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 6, 12, 0, 0, 0, 0, DateTimeKind.Local), "Katrafilov", "10002", new DateTime(2023, 6, 12, 15, 0, 47, 497, DateTimeKind.Local).AddTicks(3271), "F", true, null, "katrafilov.f@gmail.com", "katrafilov.f@gmail.com", "AQAAAAIAAYagAAAAEBxOYlLI4Yt2YOoXHCaJ5pifPDOXtx73980sRYPSFYnpAQZrmWP3xjB6PUMW9JcwLQ==", null, false, 3, "", false, "katrafilov.f@gmail.com" },
                    { "44444444-4444-4444-4444-444444444444", 0, "Željka Radeljića Škoda Roomster", null, 20, null, "jagan.drankovic@gmail.com", true, new DateTime(2024, 6, 11, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 6, 12, 0, 0, 0, 0, DateTimeKind.Local), "Jagan", "10003", new DateTime(2023, 6, 12, 15, 0, 47, 740, DateTimeKind.Local).AddTicks(4635), "Dranković", true, null, "jagan.drankovic@gmail.com", "jagan.drankovic@gmail.com", "AQAAAAIAAYagAAAAEJ5/KiOG7lA0czk9gL5/zFRefP1ofid+CmVrSKuBjwQK6Aq8iEyF2xLZf0PWk0FUuA==", null, false, 2, "", false, "jagan.drankovic@gmail.com" },
                    { "55555555-5555-5555-5555-555555555555", 0, "Derek Kentford Ave", null, 20, null, "menza.projic@gmail.com", true, new DateTime(2024, 6, 11, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 6, 12, 0, 0, 0, 0, DateTimeKind.Local), "Menza", "10004", new DateTime(2023, 6, 12, 15, 0, 47, 945, DateTimeKind.Local).AddTicks(9236), "Projić", true, null, "menza.projic@gmail.com", "menza.projic@gmail.com", "AQAAAAIAAYagAAAAELUVPANdzbdzrqQHjlVy2ciOXVgwpK70Iz8eNtlwADUIb9pt9Df+63Wxf/veWNSnGw==", null, false, 5, "", false, "menza.projic@gmail.com" },
                    { "66666666-6666-6666-6666-666666666666", 0, "Dylan McKenzie St.", null, 20, null, "goran.los.andjeles@gmail.com", true, new DateTime(2024, 6, 11, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 6, 12, 0, 0, 0, 0, DateTimeKind.Local), "Goran", "10005", new DateTime(2023, 6, 12, 15, 0, 48, 180, DateTimeKind.Local).AddTicks(974), "Los Anđeles", true, null, "goran.los.andjeles@gmail.com", "goran.los.andjeles@gmail.com", "AQAAAAIAAYagAAAAEKItUcrnoDDDZKUUMzsWZkqBTig3j0CDQMNhrvZ/VUPKgCv9cXwA7s9NJEkKoRh8Uw==", null, false, 4, "", false, "goran.los.andjeles@gmail.com" },
                    { "77777777-7777-7777-7777-777777777777", 0, "Sokače \"Sv. Trifutin\"", null, 20, null, "milka.ladovinka@gmail.com", true, new DateTime(2024, 6, 11, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 6, 12, 0, 0, 0, 0, DateTimeKind.Local), "Milka", "10006", new DateTime(2023, 6, 12, 15, 0, 48, 408, DateTimeKind.Local).AddTicks(7451), "Ladovinka", true, null, "milka.ladovinka@gmail.com", "milka.ladovinka@gmail.com", "AQAAAAIAAYagAAAAEM5blQVUae1DbLKDkNaAP67eVqzRAcUu4IOW3J78VxLx1o/OxfUN7irnrTekTywMkg==", null, false, 2, "", false, "milka.ladovinka@gmail.com" },
                    { "88888888-8888-8888-8888-888888888888", 0, "Ispod mosta, Zenica", null, 20, null, "mustafa.hrustic@gmail.com", true, new DateTime(2024, 6, 11, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 6, 12, 0, 0, 0, 0, DateTimeKind.Local), "Mustafa", "10007", new DateTime(2023, 6, 12, 15, 0, 48, 612, DateTimeKind.Local).AddTicks(9747), "Hrustić", true, null, "mustafa.hrustic@gmail.com", "mustafa.hrustic@gmail.com", "AQAAAAIAAYagAAAAEK04Heey7LCuzg8WR8vT7JourNerFO4DmW/3rZbltRNqBScmncw+Eq0rdd5126HBNQ==", null, false, 3, "", false, "mustafa.hrustic@gmail.com" },
                    { "99999999-9999-9999-9999-999999999999", 0, "Bogoljuba Bradostanojevića", null, 20, null, "boban.gasev@gmail.com", true, new DateTime(2024, 6, 11, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 6, 12, 0, 0, 0, 0, DateTimeKind.Local), "Boban", "10008", new DateTime(2023, 6, 12, 15, 0, 48, 795, DateTimeKind.Local).AddTicks(8985), "Gasev", true, null, "boban.gasev@gmail.com", "boban.gasev@gmail.com", "AQAAAAIAAYagAAAAEKhygagmokkNhZNPEjPUW5TagcR0HZEFP6RRtX58EMOojt01Q0cZFALoQ+F1qwZsjg==", null, false, 4, "", false, "boban.gasev@gmail.com" },
                    { "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa", 0, "Vodo Elektro 13", null, 20, null, "admin@outlook.com", true, new DateTime(2024, 6, 11, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 6, 12, 0, 0, 0, 0, DateTimeKind.Local), "Administrator", "12345", new DateTime(2023, 6, 12, 15, 0, 47, 44, DateTimeKind.Local).AddTicks(5176), "Outlook", true, null, "admin@outlook.com", "admin@outlook.com", "AQAAAAIAAYagAAAAEMlBYZzgG0Yz9w4UFKd9h3aUXxoJ9DfGp0KCEXzAZlEF1dGfQiKd5iECzmDWFETyFw==", null, false, 1, "", false, "admin@outlook.com" },
                    { "eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee", 0, "Patrijarha Veropojlija", null, 20, null, "erl.znojsulja@gmail.com", true, new DateTime(2024, 6, 11, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 6, 12, 0, 0, 0, 0, DateTimeKind.Local), "Erl", "10009", new DateTime(2023, 6, 12, 15, 0, 48, 999, DateTimeKind.Local).AddTicks(5663), "Znojšulja", true, null, "erl.znojsulja@gmail.com", "erl.znojsulja@gmail.com", "AQAAAAIAAYagAAAAEKQCpUDJxl+sN6qiTGW8l7JgYfzvjVrS+IFIwv8zNuu2ROxGQlJAVSOKn3tGkxd10A==", null, false, 4, "", false, "erl.znojsulja@gmail.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "dddddddd-dddd-dddd-dddd-dddddddddddd", "11111111-1111-1111-1111-111111111111" },
                    { "dddddddd-dddd-dddd-dddd-dddddddddddd", "22222222-2222-2222-2222-222222222222" },
                    { "dddddddd-dddd-dddd-dddd-dddddddddddd", "33333333-3333-3333-3333-333333333333" },
                    { "dddddddd-dddd-dddd-dddd-dddddddddddd", "44444444-4444-4444-4444-444444444444" },
                    { "dddddddd-dddd-dddd-dddd-dddddddddddd", "55555555-5555-5555-5555-555555555555" },
                    { "dddddddd-dddd-dddd-dddd-dddddddddddd", "66666666-6666-6666-6666-666666666666" },
                    { "dddddddd-dddd-dddd-dddd-dddddddddddd", "77777777-7777-7777-7777-777777777777" },
                    { "dddddddd-dddd-dddd-dddd-dddddddddddd", "88888888-8888-8888-8888-888888888888" },
                    { "dddddddd-dddd-dddd-dddd-dddddddddddd", "99999999-9999-9999-9999-999999999999" },
                    { "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa", "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa" },
                    { "dddddddd-dddd-dddd-dddd-dddddddddddd", "eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee" }
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
                keyValues: new object[] { "dddddddd-dddd-dddd-dddd-dddddddddddd", "22222222-2222-2222-2222-222222222222" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "dddddddd-dddd-dddd-dddd-dddddddddddd", "33333333-3333-3333-3333-333333333333" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "dddddddd-dddd-dddd-dddd-dddddddddddd", "44444444-4444-4444-4444-444444444444" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "dddddddd-dddd-dddd-dddd-dddddddddddd", "55555555-5555-5555-5555-555555555555" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "dddddddd-dddd-dddd-dddd-dddddddddddd", "66666666-6666-6666-6666-666666666666" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "dddddddd-dddd-dddd-dddd-dddddddddddd", "77777777-7777-7777-7777-777777777777" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "dddddddd-dddd-dddd-dddd-dddddddddddd", "88888888-8888-8888-8888-888888888888" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "dddddddd-dddd-dddd-dddd-dddddddddddd", "99999999-9999-9999-9999-999999999999" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa", "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "dddddddd-dddd-dddd-dddd-dddddddddddd", "eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee" });

            migrationBuilder.DeleteData(
                table: "LeaveTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "LeaveTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "LeaveTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "LeaveTypes",
                keyColumn: "Id",
                keyValue: 4);

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
                keyValue: "22222222-2222-2222-2222-222222222222");

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "33333333-3333-3333-3333-333333333333");

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "44444444-4444-4444-4444-444444444444");

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "55555555-5555-5555-5555-555555555555");

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "66666666-6666-6666-6666-666666666666");

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "77777777-7777-7777-7777-777777777777");

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "88888888-8888-8888-8888-888888888888");

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "99999999-9999-9999-9999-999999999999");

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: "eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee");

            migrationBuilder.DeleteData(
                table: "Positions",
                keyColumn: "Id",
                keyValue: 1);

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
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Positions",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
