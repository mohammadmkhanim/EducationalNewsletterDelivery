using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationalNewsletterDelivery.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsSuccessInDeliveredNewsletter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSuccess",
                table: "DeliveredNewsletters");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSuccess",
                table: "DeliveredNewsletters",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
