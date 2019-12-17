using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConsumerOne.Api.Migrations
{
    public partial class FollowerFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Relations_AspNetUsers_AppUserId",
                table: "Relations");

            migrationBuilder.DropForeignKey(
                name: "FK_Relations_AspNetUsers_AppUserId1",
                table: "Relations");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Relations_ChildId_ParentId",
                table: "Relations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Relations",
                table: "Relations");

            migrationBuilder.DropIndex(
                name: "IX_Relations_AppUserId",
                table: "Relations");

            migrationBuilder.DropIndex(
                name: "IX_Relations_AppUserId1",
                table: "Relations");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Relations");

            migrationBuilder.DropColumn(
                name: "ChildId",
                table: "Relations");

            migrationBuilder.RenameColumn(
                name: "AppUserId1",
                table: "Relations",
                newName: "FollowerId");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "Relations",
                newName: "FollowedId");

            migrationBuilder.AlterColumn<string>(
                name: "FollowerId",
                table: "Relations",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FollowedId",
                table: "Relations",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Relations",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Relations",
                table: "Relations",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Relations",
                table: "Relations");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Relations");

            migrationBuilder.RenameColumn(
                name: "FollowerId",
                table: "Relations",
                newName: "AppUserId1");

            migrationBuilder.RenameColumn(
                name: "FollowedId",
                table: "Relations",
                newName: "AppUserId");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId1",
                table: "Relations",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "Relations",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParentId",
                table: "Relations",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ChildId",
                table: "Relations",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Relations_ChildId_ParentId",
                table: "Relations",
                columns: new[] { "ChildId", "ParentId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Relations",
                table: "Relations",
                columns: new[] { "ParentId", "ChildId" });

            migrationBuilder.CreateIndex(
                name: "IX_Relations_AppUserId",
                table: "Relations",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Relations_AppUserId1",
                table: "Relations",
                column: "AppUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Relations_AspNetUsers_AppUserId",
                table: "Relations",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Relations_AspNetUsers_AppUserId1",
                table: "Relations",
                column: "AppUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
