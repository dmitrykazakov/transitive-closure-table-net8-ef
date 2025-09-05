using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransitiveClosureTable.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExceptionJournals",
                columns: table => new
                {
                    EventId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StackTrace = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: false),
                    QueryParams = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: false),
                    BodyParams = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: false),
                    ExceptionType = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExceptionJournals", x => x.EventId);
                });

            migrationBuilder.CreateTable(
                name: "Trees",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Nodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TreeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nodes_Trees_TreeId",
                        column: x => x.TreeId,
                        principalTable: "Trees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransitiveClosures",
                columns: table => new
                {
                    TreeId = table.Column<int>(type: "int", nullable: false),
                    AncestorId = table.Column<int>(type: "int", nullable: false),
                    DescendantId = table.Column<int>(type: "int", nullable: false),
                    Depth = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransitiveClosures", x => new { x.AncestorId, x.DescendantId });
                    table.ForeignKey(
                        name: "FK_TransitiveClosures_Nodes_AncestorId",
                        column: x => x.AncestorId,
                        principalTable: "Nodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransitiveClosures_Nodes_DescendantId",
                        column: x => x.DescendantId,
                        principalTable: "Nodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransitiveClosures_Trees_TreeId",
                        column: x => x.TreeId,
                        principalTable: "Trees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Nodes_TreeId",
                table: "Nodes",
                column: "TreeId");

            migrationBuilder.CreateIndex(
                name: "IX_TransitiveClosures_AncestorId",
                table: "TransitiveClosures",
                column: "AncestorId");

            migrationBuilder.CreateIndex(
                name: "IX_TransitiveClosures_DescendantId",
                table: "TransitiveClosures",
                column: "DescendantId");

            migrationBuilder.CreateIndex(
                name: "IX_TransitiveClosures_TreeId",
                table: "TransitiveClosures",
                column: "TreeId");

            migrationBuilder.CreateIndex(
                name: "IX_Trees_Name",
                table: "Trees",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExceptionJournals");

            migrationBuilder.DropTable(
                name: "TransitiveClosures");

            migrationBuilder.DropTable(
                name: "Nodes");

            migrationBuilder.DropTable(
                name: "Trees");
        }
    }
}
