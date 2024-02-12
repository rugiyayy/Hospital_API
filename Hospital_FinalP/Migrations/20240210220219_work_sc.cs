using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_FinalP.Migrations
{
    /// <inheritdoc />
    public partial class work_sc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                table: "WorkingSchedules");

            migrationBuilder.AddColumn<string>(
                name: "WorkingDaysString",
                table: "WorkingSchedules",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkingDaysString",
                table: "WorkingSchedules");

            migrationBuilder.AddColumn<int>(
                name: "DayOfWeek",
                table: "WorkingSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
