using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Data.Migrations
{
    public partial class ThirdMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserBook_AspNetUsers_ApplicationUserId",
                table: "ApplicationUserBook");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserBook_Books_BookId",
                table: "ApplicationUserBook");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUserBook",
                table: "ApplicationUserBook");

            migrationBuilder.RenameTable(
                name: "ApplicationUserBook",
                newName: "ApplicationUserBooks");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUserBook_ApplicationUserId",
                table: "ApplicationUserBooks",
                newName: "IX_ApplicationUserBooks_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUserBooks",
                table: "ApplicationUserBooks",
                columns: new[] { "BookId", "ApplicationUserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserBooks_AspNetUsers_ApplicationUserId",
                table: "ApplicationUserBooks",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserBooks_Books_BookId",
                table: "ApplicationUserBooks",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserBooks_AspNetUsers_ApplicationUserId",
                table: "ApplicationUserBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserBooks_Books_BookId",
                table: "ApplicationUserBooks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUserBooks",
                table: "ApplicationUserBooks");

            migrationBuilder.RenameTable(
                name: "ApplicationUserBooks",
                newName: "ApplicationUserBook");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUserBooks_ApplicationUserId",
                table: "ApplicationUserBook",
                newName: "IX_ApplicationUserBook_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUserBook",
                table: "ApplicationUserBook",
                columns: new[] { "BookId", "ApplicationUserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserBook_AspNetUsers_ApplicationUserId",
                table: "ApplicationUserBook",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserBook_Books_BookId",
                table: "ApplicationUserBook",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
