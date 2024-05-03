using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECraft.Migrations
{
    /// <inheritdoc />
    public partial class BaseEntityMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CompletionDate",
                table: "CraftProjects",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "RefIdentifier",
                table: "CraftProjects",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "CraftProjects",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "JoinDate",
                table: "Crafters",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "isMaleGender",
                table: "AppUser",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "CommunityTag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagString = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PublicationsCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityTag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skill",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skill_Crafts_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Crafts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTag",
                columns: table => new
                {
                    TagId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTag", x => new { x.ProjectId, x.TagId });
                    table.ForeignKey(
                        name: "FK_ProjectTag_CommunityTag_TagId",
                        column: x => x.TagId,
                        principalTable: "CommunityTag",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjectTag_CraftProjects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "CraftProjects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CrafterSkill",
                columns: table => new
                {
                    SkillId = table.Column<int>(type: "int", nullable: false),
                    CrafterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrafterSkill", x => new { x.CrafterId, x.SkillId });
                    table.ForeignKey(
                        name: "FK_CrafterSkill_Crafters_CrafterId",
                        column: x => x.CrafterId,
                        principalTable: "Crafters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CrafterSkill_Skill_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skill",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CrafterSkill_SkillId",
                table: "CrafterSkill",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTag_TagId",
                table: "ProjectTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Skill_CategoryId",
                table: "Skill",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CrafterSkill");

            migrationBuilder.DropTable(
                name: "ProjectTag");

            migrationBuilder.DropTable(
                name: "Skill");

            migrationBuilder.DropTable(
                name: "CommunityTag");

            migrationBuilder.DropColumn(
                name: "CompletionDate",
                table: "CraftProjects");

            migrationBuilder.DropColumn(
                name: "RefIdentifier",
                table: "CraftProjects");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "CraftProjects");

            migrationBuilder.DropColumn(
                name: "JoinDate",
                table: "Crafters");

            migrationBuilder.DropColumn(
                name: "isMaleGender",
                table: "AppUser");
        }
    }
}
