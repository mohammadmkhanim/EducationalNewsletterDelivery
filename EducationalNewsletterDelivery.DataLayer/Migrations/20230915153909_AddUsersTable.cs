using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationalNewsletterDelivery.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "DeliveredNewsletters");

            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "DeliveredNewsletters",
                newName: "DeliveredDateTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReceivedDateTime",
                table: "DeliveredNewsletters",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SeenDateTime",
                table: "DeliveredNewsletters",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "DeliveredNewsletters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveredNewsletters_UserId",
                table: "DeliveredNewsletters",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveredNewsletters_Users_UserId",
                table: "DeliveredNewsletters",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveredNewsletters_Users_UserId",
                table: "DeliveredNewsletters");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_DeliveredNewsletters_UserId",
                table: "DeliveredNewsletters");

            migrationBuilder.DropColumn(
                name: "ReceivedDateTime",
                table: "DeliveredNewsletters");

            migrationBuilder.DropColumn(
                name: "SeenDateTime",
                table: "DeliveredNewsletters");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "DeliveredNewsletters");

            migrationBuilder.RenameColumn(
                name: "DeliveredDateTime",
                table: "DeliveredNewsletters",
                newName: "DateTime");

            migrationBuilder.AddColumn<string>(
                name: "ReceiverId",
                table: "DeliveredNewsletters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
