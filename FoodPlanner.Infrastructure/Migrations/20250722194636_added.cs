using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FoodPlanner.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("57454260-b650-4114-864f-9625ced0b947"), null, "Admin", "ADMIN" },
                    { new Guid("5c15241a-44e5-4ece-9eca-f5d832af5dad"), null, "User", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("3f9f79aa-d1e3-4c4c-a4ab-2797d22708a7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b455bdb2-6cc7-498f-af3b-7e7279289601", "AQAAAAIAAYagAAAAEFWYT9RACOvYfvr5mFhQwO9g20poK1vdqiDNYg9N+iVaycL5FUj0YSi6nmo43B8crw==", "99f4c8be-7c60-478c-84fa-ccdb90994d80" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("5c15241a-44e5-4ece-9eca-f5d832af5dad"), new Guid("3f9f79aa-d1e3-4c4c-a4ab-2797d22708a7") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("57454260-b650-4114-864f-9625ced0b947"));

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("5c15241a-44e5-4ece-9eca-f5d832af5dad"), new Guid("3f9f79aa-d1e3-4c4c-a4ab-2797d22708a7") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5c15241a-44e5-4ece-9eca-f5d832af5dad"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("3f9f79aa-d1e3-4c4c-a4ab-2797d22708a7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5a495663-6157-4e3c-a227-4d80d6d0641d", "AQAAAAIAAYagAAAAEFOqEUoqT2GC0dvS/W0OwxrPwX0hTPUPwv2zn6/Zdvctc5zPATM6HqcO8mfGpZwyVQ==", "9f2e738d-8f08-456f-adf2-1657df9459e2" });
        }
    }
}
