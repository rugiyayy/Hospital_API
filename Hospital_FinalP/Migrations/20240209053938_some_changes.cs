using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_FinalP.Migrations
{
    /// <inheritdoc />
    public partial class some_changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkingSchedule_Doctors_DoctorId",
                table: "WorkingSchedule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkingSchedule",
                table: "WorkingSchedule");

            migrationBuilder.RenameTable(
                name: "WorkingSchedule",
                newName: "WorkingSchedules");

            migrationBuilder.RenameIndex(
                name: "IX_WorkingSchedule_DoctorId",
                table: "WorkingSchedules",
                newName: "IX_WorkingSchedules_DoctorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkingSchedules",
                table: "WorkingSchedules",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingSchedules_Doctors_DoctorId",
                table: "WorkingSchedules",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkingSchedules_Doctors_DoctorId",
                table: "WorkingSchedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkingSchedules",
                table: "WorkingSchedules");

            migrationBuilder.RenameTable(
                name: "WorkingSchedules",
                newName: "WorkingSchedule");

            migrationBuilder.RenameIndex(
                name: "IX_WorkingSchedules_DoctorId",
                table: "WorkingSchedule",
                newName: "IX_WorkingSchedule_DoctorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkingSchedule",
                table: "WorkingSchedule",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingSchedule_Doctors_DoctorId",
                table: "WorkingSchedule",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
