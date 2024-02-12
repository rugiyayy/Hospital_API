using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_FinalP.Migrations
{
    /// <inheritdoc />
    public partial class work_sc_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkingDay",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    WorkingScheduleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingDay", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkingDay_WorkingSchedules_WorkingScheduleId",
                        column: x => x.WorkingScheduleId,
                        principalTable: "WorkingSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkingDay_WorkingScheduleId",
                table: "WorkingDay",
                column: "WorkingScheduleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkingDay");
        }
    }
}
