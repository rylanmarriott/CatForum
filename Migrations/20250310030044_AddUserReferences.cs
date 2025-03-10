using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatForum.Migrations
{
    /// <inheritdoc />
    public partial class AddUserReferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_AspNetUsers_ApplicationUserId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Discussion_AspNetUsers_ApplicationUserId",
                table: "Discussion");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "Discussion",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Discussion_ApplicationUserId1",
                table: "Discussion",
                column: "ApplicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_AspNetUsers_ApplicationUserId",
                table: "Comment",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Discussion_AspNetUsers_ApplicationUserId",
                table: "Discussion",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Discussion_AspNetUsers_ApplicationUserId1",
                table: "Discussion",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_AspNetUsers_ApplicationUserId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Discussion_AspNetUsers_ApplicationUserId",
                table: "Discussion");

            migrationBuilder.DropForeignKey(
                name: "FK_Discussion_AspNetUsers_ApplicationUserId1",
                table: "Discussion");

            migrationBuilder.DropIndex(
                name: "IX_Discussion_ApplicationUserId1",
                table: "Discussion");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "Discussion");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_AspNetUsers_ApplicationUserId",
                table: "Comment",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Discussion_AspNetUsers_ApplicationUserId",
                table: "Discussion",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
