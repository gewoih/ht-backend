using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HT.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MoodScore",
                table: "JournalLogs",
                newName: "Score_Mood");

            migrationBuilder.RenameColumn(
                name: "HealthScore",
                table: "JournalLogs",
                newName: "Score_Health");

            migrationBuilder.RenameColumn(
                name: "EnergyScore",
                table: "JournalLogs",
                newName: "Score_Energy");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Date",
                table: "JournalLogs",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Score_Mood",
                table: "JournalLogs",
                newName: "MoodScore");

            migrationBuilder.RenameColumn(
                name: "Score_Health",
                table: "JournalLogs",
                newName: "HealthScore");

            migrationBuilder.RenameColumn(
                name: "Score_Energy",
                table: "JournalLogs",
                newName: "EnergyScore");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "JournalLogs",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }
    }
}
