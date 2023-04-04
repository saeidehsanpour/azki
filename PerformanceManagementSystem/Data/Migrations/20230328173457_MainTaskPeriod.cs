using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PerformanceManagementSystem.Data.Migrations
{
    public partial class MainTaskPeriod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "CompetencyLevelTaskMappings");

            migrationBuilder.AddColumn<Guid>(
                name: "MainTaskOfPeriodId",
                table: "TaskOfPeriods",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskOfPeriods_MainTaskOfPeriodId",
                table: "TaskOfPeriods",
                column: "MainTaskOfPeriodId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskOfPeriods_TaskOfPeriods_MainTaskOfPeriodId",
                table: "TaskOfPeriods",
                column: "MainTaskOfPeriodId",
                principalTable: "TaskOfPeriods",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskOfPeriods_TaskOfPeriods_MainTaskOfPeriodId",
                table: "TaskOfPeriods");

            migrationBuilder.DropIndex(
                name: "IX_TaskOfPeriods_MainTaskOfPeriodId",
                table: "TaskOfPeriods");

            migrationBuilder.DropColumn(
                name: "MainTaskOfPeriodId",
                table: "TaskOfPeriods");

            migrationBuilder.AddColumn<Guid>(
                name: "TaskId",
                table: "CompetencyLevelTaskMappings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
