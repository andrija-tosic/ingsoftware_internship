using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacaYAY.Data.Migrations
{
    /// <inheritdoc />
    public partial class MoveLeaveTypeFromVacationRequestReviewToVacationRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VacationRequestsReviews_LeaveTypes_LeaveTypeId",
                table: "VacationReviews");

            migrationBuilder.DropIndex(
                name: "IX_VacationRequestsReviews_LeaveTypeId",
                table: "VacationReviews");

            migrationBuilder.DropColumn(
                name: "LeaveTypeId",
                table: "VacationReviews");

            migrationBuilder.AddColumn<int>(
                name: "LeaveTypeId",
                table: "VacationRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_VacationRequests_LeaveTypeId",
                table: "VacationRequests",
                column: "LeaveTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_VacationRequests_LeaveTypes_LeaveTypeId",
                table: "VacationRequests",
                column: "LeaveTypeId",
                principalTable: "LeaveTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VacationRequests_LeaveTypes_LeaveTypeId",
                table: "VacationRequests");

            migrationBuilder.DropIndex(
                name: "IX_VacationRequests_LeaveTypeId",
                table: "VacationRequests");

            migrationBuilder.DropColumn(
                name: "LeaveTypeId",
                table: "VacationRequests");

            migrationBuilder.AddColumn<int>(
                name: "LeaveTypeId",
                table: "VacationReviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_VacationRequestsReviews_LeaveTypeId",
                table: "VacationReviews",
                column: "LeaveTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_VacationRequestsReviews_LeaveTypes_LeaveTypeId",
                table: "VacationReviews",
                column: "LeaveTypeId",
                principalTable: "LeaveTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
