using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jegymester.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class ticketUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Rooms_RoomId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Screenings_ScreeningId1",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_RoomId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_ScreeningId1",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ScreeningId1",
                table: "Tickets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ScreeningId1",
                table: "Tickets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_RoomId",
                table: "Tickets",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ScreeningId1",
                table: "Tickets",
                column: "ScreeningId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Rooms_RoomId",
                table: "Tickets",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Screenings_ScreeningId1",
                table: "Tickets",
                column: "ScreeningId1",
                principalTable: "Screenings",
                principalColumn: "Id");
        }
    }
}
