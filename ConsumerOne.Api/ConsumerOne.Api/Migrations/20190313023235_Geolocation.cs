using GeoAPI.Geometries;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConsumerOne.Api.Migrations
{
    public partial class Geolocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserFriendRelation_AspNetUsers_AppUserFriendId",
                table: "AppUserFriendRelation");

            migrationBuilder.AddColumn<IPoint>(
                name: "Location",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUserFriendId1",
                table: "AppUserFriendRelation",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUserFriendRelation_AppUserFriendId1",
                table: "AppUserFriendRelation",
                column: "AppUserFriendId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserFriendRelation_AspNetUsers_AppUserFriendId1",
                table: "AppUserFriendRelation",
                column: "AppUserFriendId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserFriendRelation_AspNetUsers_AppUserFriendId1",
                table: "AppUserFriendRelation");

            migrationBuilder.DropIndex(
                name: "IX_AppUserFriendRelation_AppUserFriendId1",
                table: "AppUserFriendRelation");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "AppUserFriendId1",
                table: "AppUserFriendRelation");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserFriendRelation_AspNetUsers_AppUserFriendId",
                table: "AppUserFriendRelation",
                column: "AppUserFriendId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
