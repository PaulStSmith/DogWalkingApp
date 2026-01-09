using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogWalkingApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsDeletedFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Remove IsDeleted columns from all tables
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Walks");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Dogs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Clients");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Add IsDeleted columns back if rolling back
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Clients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Dogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Walks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}