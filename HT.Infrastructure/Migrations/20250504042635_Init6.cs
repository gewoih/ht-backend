using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HT.Common.Migrations
{
    /// <inheritdoc />
    public partial class Init6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JournalLogs_Users_UserId",
                table: "JournalLogs");

            migrationBuilder.DropIndex(
                name: "IX_JournalLogs_UserId",
                table: "JournalLogs");

            migrationBuilder.DropColumn(
                name: "Polarity",
                table: "Habits");

            migrationBuilder.CreateTable(
                name: "HabitUser",
                columns: table => new
                {
                    HabitsId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HabitUser", x => new { x.HabitsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_HabitUser_Habits_HabitsId",
                        column: x => x.HabitsId,
                        principalTable: "Habits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HabitUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserHabits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    HabitId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserHabits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserHabits_Habits_HabitId",
                        column: x => x.HabitId,
                        principalTable: "Habits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserHabits_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HabitUser_UsersId",
                table: "HabitUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_UserHabits_HabitId",
                table: "UserHabits",
                column: "HabitId");

            migrationBuilder.CreateIndex(
                name: "IX_UserHabits_UserId",
                table: "UserHabits",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HabitUser");

            migrationBuilder.DropTable(
                name: "UserHabits");

            migrationBuilder.AddColumn<int>(
                name: "Polarity",
                table: "Habits",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_JournalLogs_UserId",
                table: "JournalLogs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_JournalLogs_Users_UserId",
                table: "JournalLogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
