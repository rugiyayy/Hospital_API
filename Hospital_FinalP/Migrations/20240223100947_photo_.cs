using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_FinalP.Migrations
{
    /// <inheritdoc />
    public partial class photo_ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "DoctorDetails");

            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "Doctors");

            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "DoctorDetails",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
