using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodPlanner.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removeUnitFromIngredientToDishIngredient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Unit",
                table: "Ingredient");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("3f9f79aa-d1e3-4c4c-a4ab-2797d22708a7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cd7b7a7e-730a-418e-aa35-cf500761f405", "AQAAAAIAAYagAAAAEE0hJpO3hQekSSMwhmgnf830eGf33VcZfFNp/+2n3TeD5OWnD+E+2ZGEufRmysaJDg==", "6dd0b133-c263-469a-919a-9da73981d695" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Unit",
                table: "Ingredient",
                type: "int",
                maxLength: 10,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("3f9f79aa-d1e3-4c4c-a4ab-2797d22708a7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b455bdb2-6cc7-498f-af3b-7e7279289601", "AQAAAAIAAYagAAAAEFWYT9RACOvYfvr5mFhQwO9g20poK1vdqiDNYg9N+iVaycL5FUj0YSi6nmo43B8crw==", "99f4c8be-7c60-478c-84fa-ccdb90994d80" });

            migrationBuilder.UpdateData(
                table: "Ingredient",
                keyColumn: "Id",
                keyValue: new Guid("7e36a395-6da1-4189-b072-2f702cc2eeed"),
                column: "Unit",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Ingredient",
                keyColumn: "Id",
                keyValue: new Guid("acc23aee-8bb6-417b-a24e-92f471cb531d"),
                column: "Unit",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Ingredient",
                keyColumn: "Id",
                keyValue: new Guid("e0b781b9-1d53-41ac-a645-997ace341929"),
                column: "Unit",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Ingredient",
                keyColumn: "Id",
                keyValue: new Guid("e9f880ad-9920-47eb-97ac-69f4969bbd5e"),
                column: "Unit",
                value: 0);
        }
    }
}
