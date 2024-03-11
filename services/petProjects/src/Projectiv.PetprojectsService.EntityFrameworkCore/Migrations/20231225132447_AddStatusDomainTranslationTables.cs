using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projectiv.PetprojectsService.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusDomainTranslationTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.CreateTable(
                name: "PetProject.Domains",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NameTranslationId = table.Column<Guid>(type: "uuid", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetProject.Domains", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PetProject.Statuses",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NameTranslationId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetProject.Statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PetProject.Translations",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Language = table.Column<int>(type: "integer", nullable: false),
                    Translate = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetProject.Translations", x => new { x.Id, x.Language });
                });
            
            migrationBuilder.RunScript("20231225125443_ADD_DOMAINS.sql");
            migrationBuilder.RunScript("20231225125443_ADD_STATUSES.sql");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PetProject.Domains",
                schema: "public");

            migrationBuilder.DropTable(
                name: "PetProject.Statuses",
                schema: "public");

            migrationBuilder.DropTable(
                name: "PetProject.Translations",
                schema: "public");

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateBy = table.Column<string>(type: "text", nullable: true),
                    CreateOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.Id);
                });
        }
    }
}
