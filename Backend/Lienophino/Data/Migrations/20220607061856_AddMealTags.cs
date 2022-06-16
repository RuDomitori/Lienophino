using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lienophino.Migrations
{
    public partial class AddMealTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MealTag",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealTag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Meal2MealTag",
                columns: table => new
                {
                    MealId = table.Column<Guid>(type: "uuid", nullable: false),
                    MealTagId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meal2MealTag", x => new { x.MealId, x.MealTagId });
                    table.ForeignKey(
                        name: "FK_Meal2MealTag_Meal_MealId",
                        column: x => x.MealId,
                        principalTable: "Meal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Meal2MealTag_MealTag_MealTagId",
                        column: x => x.MealTagId,
                        principalTable: "MealTag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Meal2MealTag_MealTagId",
                table: "Meal2MealTag",
                column: "MealTagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Meal2MealTag");

            migrationBuilder.DropTable(
                name: "MealTag");
        }
    }
}
