using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacaYAY.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeForeignKeyRelationFromRequestToReviewOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VacationRequests_VacationReviews_VacationReviewRefId",
                table: "VacationRequests");

            migrationBuilder.AlterColumn<int>(
                name: "VacationReviewRefId",
                table: "VacationRequests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_VacationRequests_VacationReviews_VacationReviewRefId",
                table: "VacationRequests",
                column: "VacationReviewRefId",
                principalTable: "VacationReviews",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VacationRequests_VacationReviews_VacationReviewRefId",
                table: "VacationRequests");

            migrationBuilder.AlterColumn<int>(
                name: "VacationReviewRefId",
                table: "VacationRequests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VacationRequests_VacationReviews_VacationReviewRefId",
                table: "VacationRequests",
                column: "VacationReviewRefId",
                principalTable: "VacationReviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
