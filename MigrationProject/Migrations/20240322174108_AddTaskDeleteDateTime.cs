using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MigrationProject.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskDeleteDateTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeleteDateTime",
                table: "Tasks",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleteDateTime",
                table: "Tasks");
        }
    }
}
