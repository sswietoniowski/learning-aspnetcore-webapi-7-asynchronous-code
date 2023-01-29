using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Books.Api.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "FirstName", "LastName" },
                values: new object[,]
                {
                    { new Guid("c0a80121-48aa-48b0-8c0c-927b1f8eac2e"), "George", "Martin" },
                    { new Guid("c0a80121-48aa-48b0-8c0c-927b1f8eac2f"), "Stephen", "King" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "Description", "Title" },
                values: new object[,]
                {
                    { new Guid("c0a80121-48aa-48b0-8c0c-927b1f8eac2c"), new Guid("c0a80121-48aa-48b0-8c0c-927b1f8eac2e"), "The first book in the A Song of Ice and Fire series.", "A Game of Thrones" },
                    { new Guid("c0a80121-48aa-48b0-8c0c-927b1f8eac2d"), new Guid("c0a80121-48aa-48b0-8c0c-927b1f8eac2f"), "A horror novel about an evil hotel.", "The Shining" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("c0a80121-48aa-48b0-8c0c-927b1f8eac2c"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("c0a80121-48aa-48b0-8c0c-927b1f8eac2d"));

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("c0a80121-48aa-48b0-8c0c-927b1f8eac2e"));

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("c0a80121-48aa-48b0-8c0c-927b1f8eac2f"));
        }
    }
}
