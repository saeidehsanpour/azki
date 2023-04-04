using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PerformanceManagementSystem.Data.Migrations
{
    public partial class UserException : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserExceptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PerformanceManagementPeriodUserMappingId = table.Column<Guid>(type: "uuid", nullable: false),
                    ManagerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Continue = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserExceptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserExceptions_PerformanceManagementPeriodUserMappings_Perf~",
                        column: x => x.PerformanceManagementPeriodUserMappingId,
                        principalTable: "PerformanceManagementPeriodUserMappings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserExceptions_Users_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserExceptionCompetencyMappings",
                columns: table => new
                {
                    CompetencyId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserExceptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserExceptionCompetencyMappings", x => new { x.CompetencyId, x.UserExceptionId });
                    table.ForeignKey(
                        name: "FK_UserExceptionCompetencyMappings_Competencies_CompetencyId",
                        column: x => x.CompetencyId,
                        principalTable: "Competencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserExceptionCompetencyMappings_UserExceptions_UserExceptio~",
                        column: x => x.UserExceptionId,
                        principalTable: "UserExceptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserExceptionCompetencyMappings_UserExceptionId",
                table: "UserExceptionCompetencyMappings",
                column: "UserExceptionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExceptions_ManagerId",
                table: "UserExceptions",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExceptions_PerformanceManagementPeriodUserMappingId",
                table: "UserExceptions",
                column: "PerformanceManagementPeriodUserMappingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserExceptionCompetencyMappings");

            migrationBuilder.DropTable(
                name: "UserExceptions");
        }
    }
}
