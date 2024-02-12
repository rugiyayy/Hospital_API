using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_FinalP.Migrations
{
    /// <inheritdoc />
    public partial class work_sc_changed11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkingDay",
                table: "WorkingSchedules");

            migrationBuilder.AddColumn<string>(
                name: "WorkingDays",
                table: "WorkingSchedules",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkingDays",
                table: "WorkingSchedules");

            migrationBuilder.AddColumn<int>(
                name: "WorkingDay",
                table: "WorkingSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
