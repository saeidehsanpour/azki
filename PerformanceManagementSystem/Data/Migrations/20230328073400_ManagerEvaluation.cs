using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PerformanceManagementSystem.Data.Migrations
{
    public partial class ManagerEvaluation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ManagerEvaluationQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    Question = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagerEvaluationQuestions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ManagerEvaluations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PerformanceManagementPeriodUserMappingId = table.Column<Guid>(type: "uuid", nullable: false),
                    ManagerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Continue = table.Column<string>(type: "text", nullable: false),
                    ShouldImprove = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagerEvaluations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ManagerEvaluations_PerformanceManagementPeriodUserMappings_~",
                        column: x => x.PerformanceManagementPeriodUserMappingId,
                        principalTable: "PerformanceManagementPeriodUserMappings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ManagerEvaluations_Users_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ManagerEvaluationAnswers",
                columns: table => new
                {
                    ManagerEvaluationId = table.Column<Guid>(type: "uuid", nullable: false),
                    ManagerEvaluationQuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Answer = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagerEvaluationAnswers", x => new { x.ManagerEvaluationId, x.ManagerEvaluationQuestionId });
                    table.ForeignKey(
                        name: "FK_ManagerEvaluationAnswers_ManagerEvaluationQuestions_Manager~",
                        column: x => x.ManagerEvaluationQuestionId,
                        principalTable: "ManagerEvaluationQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ManagerEvaluationAnswers_ManagerEvaluations_ManagerEvaluati~",
                        column: x => x.ManagerEvaluationId,
                        principalTable: "ManagerEvaluations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ManagerEvaluationAnswers_ManagerEvaluationQuestionId",
                table: "ManagerEvaluationAnswers",
                column: "ManagerEvaluationQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_ManagerEvaluations_ManagerId",
                table: "ManagerEvaluations",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_ManagerEvaluations_PerformanceManagementPeriodUserMappingId",
                table: "ManagerEvaluations",
                column: "PerformanceManagementPeriodUserMappingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ManagerEvaluationAnswers");

            migrationBuilder.DropTable(
                name: "ManagerEvaluationQuestions");

            migrationBuilder.DropTable(
                name: "ManagerEvaluations");
        }
    }
}
