using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projectiv.PetprojectsService.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class CreateProjectAndDocuments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PetProject.Projects",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(700)", maxLength: 700, nullable: false),
                    DomainId = table.Column<Guid>(type: "uuid", nullable: false),
                    StatusId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectUrl = table.Column<string>(type: "text", nullable: true),
                    GitUrl = table.Column<string>(type: "text", nullable: true),
                    ImageId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateBy = table.Column<string>(type: "text", nullable: true),
                    CreateOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetProject.Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PetProject.Projects_PetProject.Domains_DomainId",
                        column: x => x.DomainId,
                        principalSchema: "public",
                        principalTable: "PetProject.Domains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PetProject.Projects_PetProject.Statuses_StatusId",
                        column: x => x.StatusId,
                        principalSchema: "public",
                        principalTable: "PetProject.Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PetProject.ProjectDocuments",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateBy = table.Column<string>(type: "text", nullable: true),
                    CreateOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetProject.ProjectDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PetProject.ProjectDocuments_Blobs_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Blobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PetProject.ProjectDocuments_PetProject.Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "public",
                        principalTable: "PetProject.Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PetProject.ProjectDocuments_DocumentId",
                schema: "public",
                table: "PetProject.ProjectDocuments",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_PetProject.ProjectDocuments_ProjectId",
                schema: "public",
                table: "PetProject.ProjectDocuments",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PetProject.Projects_DomainId",
                schema: "public",
                table: "PetProject.Projects",
                column: "DomainId");

            migrationBuilder.CreateIndex(
                name: "IX_PetProject.Projects_StatusId",
                schema: "public",
                table: "PetProject.Projects",
                column: "StatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PetProject.ProjectDocuments",
                schema: "public");

            migrationBuilder.DropTable(
                name: "PetProject.Projects",
                schema: "public");
        }
    }
}
