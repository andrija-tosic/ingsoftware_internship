using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacaYAY.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMistakenDoubleForeignKeysForOneToOneInVacationRequestAndVacationReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "VacationReviewRefId",
                table: "VacationRequests");

            migrationBuilder.AddForeignKey(
                name: "FK_VacationReviews_VacationRequests_VacationRequestRefId",
                table: "VacationReviews",
                column: "VacationRequestRefId",
                principalTable: "VacationRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VacationReviews_VacationRequests_VacationRequestRefId",
                table: "VacationReviews");

            migrationBuilder.AddColumn<int>(
                name: "VacationReviewRefId",
                table: "VacationRequests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VacationRequests_VacationReviewRefId",
                table: "VacationRequests",
                column: "VacationReviewRefId");

            migrationBuilder.AddForeignKey(
                name: "FK_VacationRequests_VacationReviews_VacationReviewRefId",
                table: "VacationRequests",
                column: "VacationReviewRefId",
                principalTable: "VacationReviews",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VacationReviews_VacationRequests_VacationRequestRefId",
                table: "VacationReviews",
                column: "VacationRequestRefId",
                principalTable: "VacationRequests",
                principalColumn: "Id");
        }
    }
}
