using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gestionEmployer.Migrations
{
    /// <inheritdoc />
    public partial class ajouterImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "imageUrl",
                table: "Employers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "imageUrl",
                table: "Employers");
        }
    }
}
