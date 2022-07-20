using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lienophino.Migrations
{
    public partial class AddImagesToMealAndIngredient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "Meal",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "Ingredient",
                type: "uuid",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Meal");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Ingredient");
        }
    }
}
