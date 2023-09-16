using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationalNewsletterDelivery.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class ChangeReceiverIDToReceiverIdInDeliveredNewsletter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReceiverID",
                table: "DeliveredNewsletters",
                newName: "ReceiverId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "DeliveredNewsletters",
                newName: "ReceiverID");
        }
    }
}
