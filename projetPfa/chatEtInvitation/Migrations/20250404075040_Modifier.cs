using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace chatEtInvitation.Migrations
{
    /// <inheritdoc />
    public partial class Modifier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTeam",
                table: "MessageStatuts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTeam",
                table: "MessageStatuts");
        }
    }
}
