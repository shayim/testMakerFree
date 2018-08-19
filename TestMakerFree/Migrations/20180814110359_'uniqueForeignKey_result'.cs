using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TestMakerFreeWebApp.Migrations
{
    public partial class uniqueForeignKey_result : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Results_Quizzes_QuizId1",
                table: "Results");

            migrationBuilder.DropIndex(
                name: "IX_Results_QuizId1",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "QuizId1",
                table: "Results");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuizId1",
                table: "Results",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Results_QuizId1",
                table: "Results",
                column: "QuizId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Results_Quizzes_QuizId1",
                table: "Results",
                column: "QuizId1",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
