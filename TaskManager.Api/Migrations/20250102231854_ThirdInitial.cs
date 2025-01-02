using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Api.Migrations
{
    /// <inheritdoc />
    public partial class ThirdInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "TaskManager");

            migrationBuilder.CreateTable(
                name: "DesksTable",
                schema: "TaskManager",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPrivate = table.Column<bool>(type: "boolean", nullable: false),
                    Columns = table.Column<string[]>(type: "text[]", nullable: true),
                    AdminId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskIds = table.Column<List<Guid>>(type: "uuid[]", nullable: true),
                    Color = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Image = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DesksTable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectsTable",
                schema: "TaskManager",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AdminId = table.Column<Guid>(type: "uuid", nullable: true),
                    ProjectStatus = table.Column<int>(type: "integer", nullable: false),
                    UsersIds = table.Column<List<Guid>>(type: "uuid[]", nullable: true),
                    DesksIds = table.Column<List<Guid>>(type: "uuid[]", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Image = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectsTable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersTable",
                schema: "TaskManager",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    LastLoginDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserStatus = table.Column<int>(type: "integer", nullable: false),
                    ProjectsIds = table.Column<List<Guid>>(type: "uuid[]", nullable: true),
                    DesksIds = table.Column<List<Guid>>(type: "uuid[]", nullable: true),
                    TasksIds = table.Column<List<Guid>>(type: "uuid[]", nullable: true),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Image = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersTable_ProjectsTable_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "TaskManager",
                        principalTable: "ProjectsTable",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectUser",
                schema: "TaskManager",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectUser_ProjectsTable_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "TaskManager",
                        principalTable: "ProjectsTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectUser_UsersTable_UserId",
                        column: x => x.UserId,
                        principalSchema: "TaskManager",
                        principalTable: "UsersTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TasksTable",
                schema: "TaskManager",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    File = table.Column<byte[]>(type: "bytea", nullable: true),
                    Column = table.Column<string>(type: "text", nullable: true),
                    DeskId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExecutorId = table.Column<Guid>(type: "uuid", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Color = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Image = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TasksTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TasksTable_DesksTable_DeskId",
                        column: x => x.DeskId,
                        principalSchema: "TaskManager",
                        principalTable: "DesksTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TasksTable_UsersTable_CreatorId",
                        column: x => x.CreatorId,
                        principalSchema: "TaskManager",
                        principalTable: "UsersTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TasksTable_UsersTable_ExecutorId",
                        column: x => x.ExecutorId,
                        principalSchema: "TaskManager",
                        principalTable: "UsersTable",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DesksTable_AdminId",
                schema: "TaskManager",
                table: "DesksTable",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_DesksTable_ProjectId",
                schema: "TaskManager",
                table: "DesksTable",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectsTable_AdminId",
                schema: "TaskManager",
                table: "ProjectsTable",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUser_ProjectId",
                schema: "TaskManager",
                table: "ProjectUser",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUser_UserId",
                schema: "TaskManager",
                table: "ProjectUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TasksTable_CreatorId",
                schema: "TaskManager",
                table: "TasksTable",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_TasksTable_DeskId",
                schema: "TaskManager",
                table: "TasksTable",
                column: "DeskId");

            migrationBuilder.CreateIndex(
                name: "IX_TasksTable_ExecutorId",
                schema: "TaskManager",
                table: "TasksTable",
                column: "ExecutorId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersTable_ProjectId",
                schema: "TaskManager",
                table: "UsersTable",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_DesksTable_ProjectsTable_ProjectId",
                schema: "TaskManager",
                table: "DesksTable",
                column: "ProjectId",
                principalSchema: "TaskManager",
                principalTable: "ProjectsTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DesksTable_UsersTable_AdminId",
                schema: "TaskManager",
                table: "DesksTable",
                column: "AdminId",
                principalSchema: "TaskManager",
                principalTable: "UsersTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectsTable_UsersTable_AdminId",
                schema: "TaskManager",
                table: "ProjectsTable",
                column: "AdminId",
                principalSchema: "TaskManager",
                principalTable: "UsersTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersTable_ProjectsTable_ProjectId",
                schema: "TaskManager",
                table: "UsersTable");

            migrationBuilder.DropTable(
                name: "ProjectUser",
                schema: "TaskManager");

            migrationBuilder.DropTable(
                name: "TasksTable",
                schema: "TaskManager");

            migrationBuilder.DropTable(
                name: "DesksTable",
                schema: "TaskManager");

            migrationBuilder.DropTable(
                name: "ProjectsTable",
                schema: "TaskManager");

            migrationBuilder.DropTable(
                name: "UsersTable",
                schema: "TaskManager");
        }
    }
}
