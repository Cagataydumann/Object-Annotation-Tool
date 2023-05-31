using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Annotation.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFK3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassName",
                table: "BoundingBoxes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClassName",
                table: "BoundingBoxes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
