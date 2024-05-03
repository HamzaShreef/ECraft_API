using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ECraft.Migrations
{
    /// <inheritdoc />
    public partial class seed_data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Crafters_Cities_CityId",
                table: "Crafters");

            migrationBuilder.DropForeignKey(
                name: "FK_CraftProjects_AppUser_OwnerId",
                table: "CraftProjects");

            migrationBuilder.DropIndex(
                name: "IX_CraftProjects_OwnerId",
                table: "CraftProjects");

            migrationBuilder.DropIndex(
                name: "IX_Crafters_CityId",
                table: "Crafters");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "CraftProjects");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Crafters");

            migrationBuilder.DropColumn(
                name: "ExtraLocationDetails",
                table: "Crafters");

            migrationBuilder.AddColumn<int>(
                name: "CraftersCount",
                table: "Skill",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Crafts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CrafterId",
                table: "CraftProjects",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CraftersCount",
                table: "Cities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UsersCount",
                table: "Cities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "AppUser",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "AppUser",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ExtraLocationDetails",
                table: "AppUser",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.InsertData(
                table: "AppRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, "f6766f4b-f154-4762-82e6-0c19762202fa", "Admin", "ADMIN" },
                    { 2, "f92d2df3-1ab8-4cf9-9d62-0e4a9b321794", "Crafter", "CRAFTER" }
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "CountryCode", "CountryName" },
                values: new object[,]
                {
                    { 1, "+20", "Egypt" },
                    { 2, "+1", "United States" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUser_CityId",
                table: "AppUser",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUser_Cities_CityId",
                table: "AppUser",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUser_Cities_CityId",
                table: "AppUser");

            migrationBuilder.DropIndex(
                name: "IX_AppUser_CityId",
                table: "AppUser");

            migrationBuilder.DeleteData(
                table: "AppRole",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AppRole",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "CraftersCount",
                table: "Skill");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Crafts");

            migrationBuilder.DropColumn(
                name: "CraftersCount",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "UsersCount",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "AppUser");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "AppUser");

            migrationBuilder.DropColumn(
                name: "ExtraLocationDetails",
                table: "AppUser");

            migrationBuilder.AlterColumn<int>(
                name: "CrafterId",
                table: "CraftProjects",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "CraftProjects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Crafters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ExtraLocationDetails",
                table: "Crafters",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CraftProjects_OwnerId",
                table: "CraftProjects",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Crafters_CityId",
                table: "Crafters",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Crafters_Cities_CityId",
                table: "Crafters",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CraftProjects_AppUser_OwnerId",
                table: "CraftProjects",
                column: "OwnerId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
