using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfoTrack.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SearchResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Keywords = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    SearchTarget = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    SearchEngine = table.Column<int>(type: "INTEGER", nullable: false),
                    SearchDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchResults", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SearchResultPositions",
                columns: table => new
                {
                    SearchResultId = table.Column<int>(type: "INTEGER", nullable: false),
                    Position = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchResultPositions", x => new { x.SearchResultId, x.Position });
                    table.ForeignKey(
                        name: "FK_SearchResultPositions_SearchResults_SearchResultId",
                        column: x => x.SearchResultId,
                        principalTable: "SearchResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SearchResultPositions");

            migrationBuilder.DropTable(
                name: "SearchResults");
        }
    }
}
