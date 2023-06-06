using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacaYAY.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReviewerToVacationRequestReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmployeeId",
                table: "VacationRequestsReviews",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReviewerId",
                table: "VacationRequestsReviews",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_VacationRequestsReviews_EmployeeId",
                table: "VacationRequestsReviews",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_VacationRequestsReviews_ReviewerId",
                table: "VacationRequestsReviews",
                column: "ReviewerId");

            migrationBuilder.AddForeignKey(
                name: "FK_VacationRequestsReviews_Employees_EmployeeId",
                table: "VacationRequestsReviews",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VacationRequestsReviews_Employees_ReviewerId",
                table: "VacationRequestsReviews",
                column: "ReviewerId",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VacationRequestsReviews_Employees_EmployeeId",
                table: "VacationRequestsReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_VacationRequestsReviews_Employees_ReviewerId",
                table: "VacationRequestsReviews");

            migrationBuilder.DropIndex(
                name: "IX_VacationRequestsReviews_EmployeeId",
                table: "VacationRequestsReviews");

            migrationBuilder.DropIndex(
                name: "IX_VacationRequestsReviews_ReviewerId",
                table: "VacationRequestsReviews");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "VacationRequestsReviews");

            migrationBuilder.DropColumn(
                name: "ReviewerId",
                table: "VacationRequestsReviews");
        }
    }
}
