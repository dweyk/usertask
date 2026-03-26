using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UserTaskManagement.DrivenAdapters.DomainModel.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user_tasks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_tasks", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_user_tasks_created_at",
                table: "user_tasks",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_user_tasks_status",
                table: "user_tasks",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_user_tasks_title",
                table: "user_tasks",
                column: "title");

            migrationBuilder.CreateIndex(
                name: "ix_user_tasks_updated_at",
                table: "user_tasks",
                column: "updated_at");

            migrationBuilder.CreateIndex(
                name: "ix_user_tasks_user_id",
                table: "user_tasks",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_tasks");
        }
    }
}
