using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_FinalP.Migrations
{
    /// <inheritdoc />
    public partial class identity1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentityNumber",
                table: "Patients");

            migrationBuilder.AddColumn<string>(
                name: "PatientIdentityNumber",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PatientIdentityNumber",
                table: "Patients");

            migrationBuilder.AddColumn<int>(
                name: "IdentityNumber",
                table: "Patients",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
