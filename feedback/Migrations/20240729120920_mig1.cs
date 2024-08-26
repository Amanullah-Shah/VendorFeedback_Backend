using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace feedback.Migrations
{
    /// <inheritdoc />
    public partial class mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackDetails_Questions_QuestionId",
                table: "FeedbackDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackDetails_Service_answer_AnswerID",
                table: "FeedbackDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackDetails_suggestion_answer_suggestion_answerID",
                table: "FeedbackDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackDetails_suggestion_question_suggestionID",
                table: "FeedbackDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackMasters_Branches_BranchId",
                table: "FeedbackMasters");

            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackMasters_Regions_RegionId",
                table: "FeedbackMasters");

            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackMasters_Users_UserId",
                table: "FeedbackMasters");

            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackMasters_VendorMasters_VendorMasterId",
                table: "FeedbackMasters");

            //migrationBuilder.DropIndex(
            //    name: "IX_FeedbackMasters_BranchId",
            //    table: "FeedbackMasters");

            //migrationBuilder.DropIndex(
            //    name: "IX_FeedbackMasters_RegionId",
            //    table: "FeedbackMasters");

            //migrationBuilder.DropIndex(
            //    name: "IX_FeedbackMasters_UserId",
            //    table: "FeedbackMasters");

            //migrationBuilder.DropIndex(
            //    name: "IX_FeedbackMasters_VendorMasterId",
            //    table: "FeedbackMasters");

            //migrationBuilder.DropIndex(
            //    name: "IX_FeedbackDetails_AnswerID",
            //    table: "FeedbackDetails");

            //migrationBuilder.DropIndex(
            //    name: "IX_FeedbackDetails_QuestionId",
            //    table: "FeedbackDetails");

            //migrationBuilder.DropIndex(
            //    name: "IX_FeedbackDetails_suggestion_answerID",
            //    table: "FeedbackDetails");

            //migrationBuilder.DropIndex(
            //    name: "IX_FeedbackDetails_suggestionID",
            //    table: "FeedbackDetails");

            migrationBuilder.AddColumn<byte[]>(
                name: "Sla",
                table: "VendorMasters",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sla",
                table: "VendorMasters");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackMasters_BranchId",
                table: "FeedbackMasters",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackMasters_RegionId",
                table: "FeedbackMasters",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackMasters_UserId",
                table: "FeedbackMasters",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackMasters_VendorMasterId",
                table: "FeedbackMasters",
                column: "VendorMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackDetails_AnswerID",
                table: "FeedbackDetails",
                column: "AnswerID");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackDetails_QuestionId",
                table: "FeedbackDetails",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackDetails_suggestion_answerID",
                table: "FeedbackDetails",
                column: "suggestion_answerID");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackDetails_suggestionID",
                table: "FeedbackDetails",
                column: "suggestionID");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackDetails_Questions_QuestionId",
                table: "FeedbackDetails",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackDetails_Service_answer_AnswerID",
                table: "FeedbackDetails",
                column: "AnswerID",
                principalTable: "Service_answer",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackDetails_suggestion_answer_suggestion_answerID",
                table: "FeedbackDetails",
                column: "suggestion_answerID",
                principalTable: "suggestion_answer",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackDetails_suggestion_question_suggestionID",
                table: "FeedbackDetails",
                column: "suggestionID",
                principalTable: "suggestion_question",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackMasters_Branches_BranchId",
                table: "FeedbackMasters",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackMasters_Regions_RegionId",
                table: "FeedbackMasters",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackMasters_Users_UserId",
                table: "FeedbackMasters",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackMasters_VendorMasters_VendorMasterId",
                table: "FeedbackMasters",
                column: "VendorMasterId",
                principalTable: "VendorMasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
