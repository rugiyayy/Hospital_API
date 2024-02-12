using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_FinalP.Migrations
{
    /// <inheritdoc />
    public partial class workdays : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkingDay_WorkingSchedules_WorkingScheduleId",
                table: "WorkingDay");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkingDay",
                table: "WorkingDay");

            migrationBuilder.RenameTable(
                name: "WorkingDay",
                newName: "WorkingDays");

            migrationBuilder.RenameIndex(
                name: "IX_WorkingDay_WorkingScheduleId",
                table: "WorkingDays",
                newName: "IX_WorkingDays_WorkingScheduleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkingDays",
                table: "WorkingDays",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingDays_WorkingSchedules_WorkingScheduleId",
                table: "WorkingDays",
                column: "WorkingScheduleId",
                principalTable: "WorkingSchedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkingDays_WorkingSchedules_WorkingScheduleId",
                table: "WorkingDays");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkingDays",
                table: "WorkingDays");

            migrationBuilder.RenameTable(
                name: "WorkingDays",
                newName: "WorkingDay");

            migrationBuilder.RenameIndex(
                name: "IX_WorkingDays_WorkingScheduleId",
                table: "WorkingDay",
                newName: "IX_WorkingDay_WorkingScheduleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkingDay",
                table: "WorkingDay",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingDay_WorkingSchedules_WorkingScheduleId",
                table: "WorkingDay",
                column: "WorkingScheduleId",
                principalTable: "WorkingSchedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
