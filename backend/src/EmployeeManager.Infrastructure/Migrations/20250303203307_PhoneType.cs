using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PhoneType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "PhoneNumbers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "PhoneNumbers");
        }
    }
}
