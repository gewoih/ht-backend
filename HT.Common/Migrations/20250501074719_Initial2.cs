using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HT.Common.Migrations
{
    /// <inheritdoc />
    public partial class Initial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HabitLogs_DayScores_DayScoreId",
                table: "HabitLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Habits_Users_UserId",
                table: "Habits");

            migrationBuilder.DropTable(
                name: "DayScores");

            migrationBuilder.DropIndex(
                name: "IX_Habits_UserId",
                table: "Habits");

            migrationBuilder.DropIndex(
                name: "IX_HabitLogs_DayScoreId",
                table: "HabitLogs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Habits");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "HabitLogs");

            migrationBuilder.DropColumn(
                name: "DayScoreId",
                table: "HabitLogs");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "HabitLogs",
                newName: "JournalLogId");

            migrationBuilder.CreateTable(
                name: "JournalLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JournalLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HabitLogs_JournalLogId",
                table: "HabitLogs",
                column: "JournalLogId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalLogs_UserId",
                table: "JournalLogs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_HabitLogs_JournalLogs_JournalLogId",
                table: "HabitLogs",
                column: "JournalLogId",
                principalTable: "JournalLogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HabitLogs_JournalLogs_JournalLogId",
                table: "HabitLogs");

            migrationBuilder.DropTable(
                name: "JournalLogs");

            migrationBuilder.DropIndex(
                name: "IX_HabitLogs_JournalLogId",
                table: "HabitLogs");

            migrationBuilder.RenameColumn(
                name: "JournalLogId",
                table: "HabitLogs",
                newName: "UserId");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Habits",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "HabitLogs",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "DayScoreId",
                table: "HabitLogs",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DayScores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Score = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DayScores_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Habits_UserId",
                table: "Habits",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HabitLogs_DayScoreId",
                table: "HabitLogs",
                column: "DayScoreId");

            migrationBuilder.CreateIndex(
                name: "IX_DayScores_UserId",
                table: "DayScores",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_HabitLogs_DayScores_DayScoreId",
                table: "HabitLogs",
                column: "DayScoreId",
                principalTable: "DayScores",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Habits_Users_UserId",
                table: "Habits",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
