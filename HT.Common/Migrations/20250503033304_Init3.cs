using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HT.Common.Migrations
{
    /// <inheritdoc />
    public partial class Init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Score",
                table: "JournalLogs",
                newName: "MoodScore");

            migrationBuilder.AddColumn<int>(
                name: "EnergyScore",
                table: "JournalLogs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HealthScore",
                table: "JournalLogs",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnergyScore",
                table: "JournalLogs");

            migrationBuilder.DropColumn(
                name: "HealthScore",
                table: "JournalLogs");

            migrationBuilder.RenameColumn(
                name: "MoodScore",
                table: "JournalLogs",
                newName: "Score");
        }
    }
}
