using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_FinalP.Migrations
{
    /// <inheritdoc />
    public partial class max_app_doctor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                table: "WorkingSchedules");

            migrationBuilder.AddColumn<int>(
                name: "MaxAppointments",
                table: "Doctors",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxAppointments",
                table: "Doctors");

            migrationBuilder.AddColumn<int>(
                name: "DayOfWeek",
                table: "WorkingSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
