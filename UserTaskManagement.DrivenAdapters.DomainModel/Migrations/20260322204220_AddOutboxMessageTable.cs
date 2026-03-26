using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UserTaskManagement.DrivenAdapters.DomainModel.Migrations
{
    /// <inheritdoc />
    public partial class AddOutboxMessageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "outbox_message",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    data = table.Column<string>(type: "jsonb", nullable: false),
                    occurred_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    processed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    retry_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_outbox_message", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_outbox_message_type",
                table: "outbox_message",
                column: "type");

            migrationBuilder.CreateIndex(
                name: "ix_outbox_message_unprocessed",
                table: "outbox_message",
                columns: new[] { "processed_at", "occurred_on" },
                filter: "processed_at IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "outbox_message");
        }
    }
}
