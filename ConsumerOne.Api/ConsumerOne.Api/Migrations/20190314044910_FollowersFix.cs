using Microsoft.EntityFrameworkCore.Migrations;

namespace ConsumerOne.Api.Migrations
{
    public partial class FollowersFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserFollowerRelation");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AppUserId",
                table: "AspNetUsers",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_AppUserId",
                table: "AspNetUsers",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_AppUserId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AppUserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "AppUserFollowerRelation",
                columns: table => new
                {
                    AppUserFollowerId = table.Column<string>(nullable: false),
                    AppUserId = table.Column<string>(nullable: false),
                    AppUserId1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserFollowerRelation", x => new { x.AppUserFollowerId, x.AppUserId });
                    table.ForeignKey(
                        name: "FK_AppUserFollowerRelation_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppUserFollowerRelation_AspNetUsers_AppUserId1",
                        column: x => x.AppUserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserFollowerRelation_AppUserId",
                table: "AppUserFollowerRelation",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserFollowerRelation_AppUserId1",
                table: "AppUserFollowerRelation",
                column: "AppUserId1");
        }
    }
}
