using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_FinalP.Migrations
{
    /// <inheritdoc />
    public partial class updated_props_to_user_entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResetTokenExpires",
                table: "AspNetUsers",
                newName: "ResetCodeExpires");

            migrationBuilder.RenameColumn(
                name: "PasswordResetToken",
                table: "AspNetUsers",
                newName: "ResetCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResetCodeExpires",
                table: "AspNetUsers",
                newName: "ResetTokenExpires");

            migrationBuilder.RenameColumn(
                name: "ResetCode",
                table: "AspNetUsers",
                newName: "PasswordResetToken");
        }
    }
}
