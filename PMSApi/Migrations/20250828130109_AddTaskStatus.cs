using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSApi.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "ProjectTasks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ProjectTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ProjectTasks");
        }
    }
}
