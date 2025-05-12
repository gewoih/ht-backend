using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HT.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Score_Calmness",
                table: "JournalLogs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Score_Satisfaction",
                table: "JournalLogs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Score_Sleep",
                table: "JournalLogs",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score_Calmness",
                table: "JournalLogs");

            migrationBuilder.DropColumn(
                name: "Score_Satisfaction",
                table: "JournalLogs");

            migrationBuilder.DropColumn(
                name: "Score_Sleep",
                table: "JournalLogs");
        }
    }
}
