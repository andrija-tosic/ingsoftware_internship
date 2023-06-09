using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacaYAY.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameVacationRequestReviewToVacationReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VacationReviews_VacationRequests_VacationRequestRefId",
                table: "VacationReviews");

            migrationBuilder.CreateIndex(
                name: "IX_VacationRequests_VacationReviewRefId",
                table: "VacationRequests",
                column: "VacationReviewRefId");

            migrationBuilder.AddForeignKey(
                name: "FK_VacationRequests_VacationReviews_VacationReviewRefId",
                table: "VacationRequests",
                column: "VacationReviewRefId",
                principalTable: "VacationReviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VacationReviews_VacationRequests_VacationRequestRefId",
                table: "VacationReviews",
                column: "VacationRequestRefId",
                principalTable: "VacationRequests",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VacationRequests_VacationReviews_VacationReviewRefId",
                table: "VacationRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_VacationReviews_VacationRequests_VacationRequestRefId",
                table: "VacationReviews");

            migrationBuilder.DropIndex(
                name: "IX_VacationRequests_VacationReviewRefId",
                table: "VacationRequests");

            migrationBuilder.AddForeignKey(
                name: "FK_VacationReviews_VacationRequests_VacationRequestRefId",
                table: "VacationReviews",
                column: "VacationRequestRefId",
                principalTable: "VacationRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
