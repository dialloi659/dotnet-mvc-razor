using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dotnet.MVC.Razor.Keycloak.Migrations
{
    /// <inheritdoc />
    public partial class RenameIsHandledByIsArchiveForFeedbackReportTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
            name: "IsHandled",
            table: "FeedbackReports",
            newName: "IsArchived");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
            name: "IsArchived",
            table: "FeedbackReports",
            newName: "IsHandled");
        }
    }
}
