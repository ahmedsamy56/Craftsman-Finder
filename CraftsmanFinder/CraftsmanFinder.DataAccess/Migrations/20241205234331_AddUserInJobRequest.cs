using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CraftsmanFinder.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddUserInJobRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "jobRequests",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_jobRequests_ApplicationUserId",
                table: "jobRequests",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_jobRequests_AspNetUsers_ApplicationUserId",
                table: "jobRequests",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_jobRequests_AspNetUsers_ApplicationUserId",
                table: "jobRequests");

            migrationBuilder.DropIndex(
                name: "IX_jobRequests_ApplicationUserId",
                table: "jobRequests");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "jobRequests");
        }
    }
}
