using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PerformanceManagementSystem.Data.Migrations
{
    public partial class ReportDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExceptionScoreEndDate",
                table: "PerformanceManagementPeriods",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExceptionScoreStartDate",
                table: "PerformanceManagementPeriods",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ReportEndDate",
                table: "PerformanceManagementPeriods",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ReportStartDate",
                table: "PerformanceManagementPeriods",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExceptionScoreEndDate",
                table: "PerformanceManagementPeriods");

            migrationBuilder.DropColumn(
                name: "ExceptionScoreStartDate",
                table: "PerformanceManagementPeriods");

            migrationBuilder.DropColumn(
                name: "ReportEndDate",
                table: "PerformanceManagementPeriods");

            migrationBuilder.DropColumn(
                name: "ReportStartDate",
                table: "PerformanceManagementPeriods");
        }
    }
}
