using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Api.Migrations
{
    /// <inheritdoc />
    public partial class changeTaskModelInsertFilenameProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                schema: "TaskManager",
                table: "TasksTable",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                schema: "TaskManager",
                table: "TasksTable");
        }
    }
}
