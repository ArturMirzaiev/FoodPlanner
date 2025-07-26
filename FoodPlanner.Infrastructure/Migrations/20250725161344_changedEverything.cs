using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FoodPlanner.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changedEverything : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("3f9f79aa-d1e3-4c4c-a4ab-2797d22708a7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5dc26533-c98d-4f6e-9258-d0b9b6bf701e", "AQAAAAIAAYagAAAAEBIiesxTzh2g4UVMqyWv0gJ7o1qq0HoDiWPChi8lhQ/3em7pA1nX7DofU5JhUWBb3g==", "168d9b3e-8f18-4dd6-af6d-1f5053f49910" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Persons_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("3f9f79aa-d1e3-4c4c-a4ab-2797d22708a7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fcd8dfe9-a53c-419b-9aee-32fb6731db21", "AQAAAAIAAYagAAAAECfMxjUgtGgrpOkNLHfLBSI11p3Manv+Cfd5HtP49ghxhv8MEXHsZlq8w3yPBzrn7w==", "3f918aaa-e046-4087-9fa2-93c84be0d1e8" });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Name", "UserId" },
                values: new object[,]
                {
                    { new Guid("1fd1498b-9d00-4887-a294-8df82675eb3a"), "Катя", new Guid("3f9f79aa-d1e3-4c4c-a4ab-2797d22708a7") },
                    { new Guid("c7fd7bc5-32a7-46e6-bd96-541e02c3cc54"), "Артур", new Guid("3f9f79aa-d1e3-4c4c-a4ab-2797d22708a7") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Persons_UserId",
                table: "Persons",
                column: "UserId");
        }
    }
}
