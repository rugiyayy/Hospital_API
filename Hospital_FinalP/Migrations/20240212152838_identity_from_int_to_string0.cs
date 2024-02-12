using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_FinalP.Migrations
{
    /// <inheritdoc />
    public partial class identity_from_int_to_string0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MyId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "MyProp",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MyProp",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "MyId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }
    }
}
