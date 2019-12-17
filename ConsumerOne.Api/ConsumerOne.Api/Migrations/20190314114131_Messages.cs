using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConsumerOne.Api.Migrations
{
    public partial class Messages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostMessages");

            migrationBuilder.DropTable(
                name: "PostRatings");

            migrationBuilder.CreateTable(
                name: "UserMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ToId = table.Column<string>(nullable: true),
                    FromId = table.Column<string>(nullable: true),
                    SentDate = table.Column<DateTime>(nullable: false),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserMessages_AspNetUsers_FromId",
                        column: x => x.FromId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserMessages_AspNetUsers_ToId",
                        column: x => x.ToId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserMessages_FromId",
                table: "UserMessages",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMessages_ToId",
                table: "UserMessages",
                column: "ToId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserMessages");

            migrationBuilder.CreateTable(
                name: "PostMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AppUserId = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    PostId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostMessages_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostMessages_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostRatings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AppUserId = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    PostId = table.Column<Guid>(nullable: false),
                    Price = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostRatings_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostRatings_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostMessages_AppUserId",
                table: "PostMessages",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PostMessages_PostId",
                table: "PostMessages",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostRatings_AppUserId",
                table: "PostRatings",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PostRatings_PostId",
                table: "PostRatings",
                column: "PostId");
        }
    }
}
