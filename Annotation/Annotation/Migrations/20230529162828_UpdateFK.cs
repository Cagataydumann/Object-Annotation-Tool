using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Annotation.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoundingBoxes_Classes_ClassId",
                table: "BoundingBoxes");

            migrationBuilder.AddForeignKey(
                name: "FK_BoundingBoxes_Classes_ClassId",
                table: "BoundingBoxes",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoundingBoxes_Classes_ClassId",
                table: "BoundingBoxes");

            migrationBuilder.AddForeignKey(
                name: "FK_BoundingBoxes_Classes_ClassId",
                table: "BoundingBoxes",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
