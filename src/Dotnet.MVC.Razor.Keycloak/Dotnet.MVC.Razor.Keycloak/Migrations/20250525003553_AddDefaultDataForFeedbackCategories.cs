using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dotnet.MVC.Razor.Keycloak.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultDataForFeedbackCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FeedbackCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { -4, "NotInteresting" },
                    { -3, "Processed" },
                    { -2, "InProgress" },
                    { -1, "New" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FeedbackCategories",
                keyColumn: "Id",
                keyValue: -4);

            migrationBuilder.DeleteData(
                table: "FeedbackCategories",
                keyColumn: "Id",
                keyValue: -3);

            migrationBuilder.DeleteData(
                table: "FeedbackCategories",
                keyColumn: "Id",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "FeedbackCategories",
                keyColumn: "Id",
                keyValue: -1);
        }
    }
}
