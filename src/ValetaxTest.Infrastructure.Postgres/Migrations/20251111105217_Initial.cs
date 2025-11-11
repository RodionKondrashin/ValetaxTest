using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ValetaxTest.Infrastructure.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "trees",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_trees", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "nodes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    TreeId = table.Column<long>(type: "bigint", nullable: false),
                    ParentId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_nodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_nodes_nodes_ParentId",
                        column: x => x.ParentId,
                        principalTable: "nodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_nodes_trees_TreeId",
                        column: x => x.TreeId,
                        principalTable: "trees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_nodes_name",
                table: "nodes",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_nodes_ParentId",
                table: "nodes",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_nodes_TreeId",
                table: "nodes",
                column: "TreeId");

            migrationBuilder.CreateIndex(
                name: "IX_trees_name",
                table: "trees",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "nodes");

            migrationBuilder.DropTable(
                name: "trees");
        }
    }
}
