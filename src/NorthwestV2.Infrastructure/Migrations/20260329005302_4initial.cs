using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NorthwestV2.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _4initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrentStage",
                table: "Items",
                newName: "CurrentStageContribution");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrentStageContribution",
                table: "Items",
                newName: "CurrentStage");
        }
    }
}
