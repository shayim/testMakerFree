using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TestMakerFreeWebApp.Migrations
{
    public partial class uniqueForeignKey_question : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Quizzes_QuizId1",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_QuizId1",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "QuizId1",
                table: "Questions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuizId1",
                table: "Questions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuizId1",
                table: "Questions",
                column: "QuizId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Quizzes_QuizId1",
                table: "Questions",
                column: "QuizId1",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
