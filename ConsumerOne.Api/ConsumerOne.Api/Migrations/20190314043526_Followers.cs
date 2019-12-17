using Microsoft.EntityFrameworkCore.Migrations;

namespace ConsumerOne.Api.Migrations
{
    public partial class Followers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserFriendRelation");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUserFriendRelation",
                columns: table => new
                {
                    AppUserFriendId = table.Column<string>(nullable: false),
                    AppUserId = table.Column<string>(nullable: false),
                    AppUserFriendId1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserFriendRelation", x => new { x.AppUserFriendId, x.AppUserId });
                    table.ForeignKey(
                        name: "FK_AppUserFriendRelation_AspNetUsers_AppUserFriendId1",
                        column: x => x.AppUserFriendId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppUserFriendRelation_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserFriendRelation_AppUserFriendId1",
                table: "AppUserFriendRelation",
                column: "AppUserFriendId1");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserFriendRelation_AppUserId",
                table: "AppUserFriendRelation",
                column: "AppUserId");
        }
    }
}
