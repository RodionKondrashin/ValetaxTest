using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ValetaxTest.Infrastructure.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AddExceptionJournal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_nodes_nodes_ParentId",
                table: "nodes");

            migrationBuilder.CreateTable(
                name: "exception_journal",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    event_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    text = table.Column<string>(type: "text", nullable: false),
                    query_parameters = table.Column<string>(type: "text", nullable: true),
                    body_parameters = table.Column<string>(type: "text", nullable: true),
                    stack_trace = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_exception_journal", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_exception_journal_created_at",
                table: "exception_journal",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_exception_journal_event_id",
                table: "exception_journal",
                column: "event_id");

            migrationBuilder.AddForeignKey(
                name: "FK_nodes_nodes_ParentId",
                table: "nodes",
                column: "ParentId",
                principalTable: "nodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_nodes_nodes_ParentId",
                table: "nodes");

            migrationBuilder.DropTable(
                name: "exception_journal");

            migrationBuilder.AddForeignKey(
                name: "FK_nodes_nodes_ParentId",
                table: "nodes",
                column: "ParentId",
                principalTable: "nodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
