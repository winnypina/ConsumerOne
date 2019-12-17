using Microsoft.EntityFrameworkCore.Migrations;

namespace ConsumerOne.Api.Migrations
{
    public partial class FollowersFix2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "Relations",
                columns: table => new
                {
                    ParentId = table.Column<string>(nullable: false),
                    ChildId = table.Column<string>(nullable: false),
                    AppUserId = table.Column<string>(nullable: true),
                    AppUserId1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relations", x => new { x.ParentId, x.ChildId });
                    table.UniqueConstraint("AK_Relations_ChildId_ParentId", x => new { x.ChildId, x.ParentId });
                    table.ForeignKey(
                        name: "FK_Relations_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Relations_AspNetUsers_AppUserId1",
                        column: x => x.AppUserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Relations_AppUserId",
                table: "Relations",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Relations_AppUserId1",
                table: "Relations",
                column: "AppUserId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Relations");

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
    }
}
