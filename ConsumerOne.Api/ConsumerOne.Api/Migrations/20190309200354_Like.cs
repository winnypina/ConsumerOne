using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConsumerOne.Api.Migrations
{
    public partial class Like : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostComments_AspNetUsers_AppUserId1",
                table: "PostComments");

            migrationBuilder.DropForeignKey(
                name: "FK_PostMessages_AspNetUsers_AppUserId1",
                table: "PostMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_PostRatings_AspNetUsers_AppUserId1",
                table: "PostRatings");

            migrationBuilder.DropForeignKey(
                name: "FK_PostReports_AspNetUsers_AppUserId1",
                table: "PostReports");

            migrationBuilder.DropIndex(
                name: "IX_PostReports_AppUserId1",
                table: "PostReports");

            migrationBuilder.DropIndex(
                name: "IX_PostRatings_AppUserId1",
                table: "PostRatings");

            migrationBuilder.DropIndex(
                name: "IX_PostMessages_AppUserId1",
                table: "PostMessages");

            migrationBuilder.DropIndex(
                name: "IX_PostComments_AppUserId1",
                table: "PostComments");

            migrationBuilder.DeleteData(
                table: "UseTerms",
                keyColumn: "Id",
                keyValue: new Guid("49d932e2-0487-4caf-83ff-3c6d7cbb0b97"));

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "PostReports");

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "PostRatings");

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "PostMessages");

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "PostComments");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "PostReports",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "PostRatings",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "PostMessages",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "PostComments",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.CreateTable(
                name: "PostLikes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AppUserId = table.Column<string>(nullable: true),
                    PostId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostLikes_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostLikes_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostReports_AppUserId",
                table: "PostReports",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PostRatings_AppUserId",
                table: "PostRatings",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PostMessages_AppUserId",
                table: "PostMessages",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PostComments_AppUserId",
                table: "PostComments",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PostLikes_AppUserId",
                table: "PostLikes",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PostLikes_PostId",
                table: "PostLikes",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostComments_AspNetUsers_AppUserId",
                table: "PostComments",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostMessages_AspNetUsers_AppUserId",
                table: "PostMessages",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostRatings_AspNetUsers_AppUserId",
                table: "PostRatings",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostReports_AspNetUsers_AppUserId",
                table: "PostReports",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostComments_AspNetUsers_AppUserId",
                table: "PostComments");

            migrationBuilder.DropForeignKey(
                name: "FK_PostMessages_AspNetUsers_AppUserId",
                table: "PostMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_PostRatings_AspNetUsers_AppUserId",
                table: "PostRatings");

            migrationBuilder.DropForeignKey(
                name: "FK_PostReports_AspNetUsers_AppUserId",
                table: "PostReports");

            migrationBuilder.DropTable(
                name: "PostLikes");

            migrationBuilder.DropIndex(
                name: "IX_PostReports_AppUserId",
                table: "PostReports");

            migrationBuilder.DropIndex(
                name: "IX_PostRatings_AppUserId",
                table: "PostRatings");

            migrationBuilder.DropIndex(
                name: "IX_PostMessages_AppUserId",
                table: "PostMessages");

            migrationBuilder.DropIndex(
                name: "IX_PostComments_AppUserId",
                table: "PostComments");

            migrationBuilder.AlterColumn<Guid>(
                name: "AppUserId",
                table: "PostReports",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId1",
                table: "PostReports",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AppUserId",
                table: "PostRatings",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId1",
                table: "PostRatings",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AppUserId",
                table: "PostMessages",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId1",
                table: "PostMessages",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AppUserId",
                table: "PostComments",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId1",
                table: "PostComments",
                nullable: true);

            migrationBuilder.InsertData(
                table: "UseTerms",
                columns: new[] { "Id", "Value", "ValueEnUs", "ValueEs" },
                values: new object[] { new Guid("49d932e2-0487-4caf-83ff-3c6d7cbb0b97"), @"Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est eopksio laborum. Sed ut perspiciatis unde omnis istpoe natus error sit voluptatem accusantium doloremque eopsloi laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunot explicabo. Nemo ernim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sedopk quia consequuntur magni dolores eos qui rationesopl voluptatem sequi nesciunt. Neque porro quisquameo est, qui dolorem ipsum quia dolor sit amet, eopsmiep consectetur, adipisci velit, seisud quia non numquam eius modi tempora incidunt ut labore et dolore wopeir magnam aliquam quaerat voluptatem eoplmuriquisqu. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est eopksio laborum. Sed ut perspiciatis unde omnis istpoe natus error sit voluptatem accusantium doloremque eopsloi laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunot explicabo. Nemo ernim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sedopk quia consequuntur magni dolores eos qui rationesopl voluptatem sequi nesciunt. Neque porro quisquameo est, qui dolorem ipsum quia dolor sit amet, eopsmiep consectetur, adipisci velit, seisud quia non numquam eius modi tempora incidunt ut labore et dolore wopeir magnam aliquam quaerat voluptatem eoplmuriquisqu
Excepteur sint occaecat cupidatat non proident,
                sunt in culpa qui officia deserunt mollit anim id est eopksio laborum.Sed ut perspiciatis unde omnis istpoe natus error sit voluptatem accusantium doloremque eopsloi laudantium,
                totam rem aperiam,
                eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunot explicabo.Nemo ernim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit,
                sedopk quia consequuntur magni dolores eos qui rationesopl voluptatem sequi nesciunt.Neque porro quisquameo est,
                qui dolorem ipsum quia dolor sit amet,
                eopsmiep consectetur,
                adipisci velit,
                seisud quia non numquam eius modi tempora incidunt ut labore et dolore wopeir magnam aliquam quaerat voluptatem eoplmuriquisqu", null, null });

            migrationBuilder.CreateIndex(
                name: "IX_PostReports_AppUserId1",
                table: "PostReports",
                column: "AppUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_PostRatings_AppUserId1",
                table: "PostRatings",
                column: "AppUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_PostMessages_AppUserId1",
                table: "PostMessages",
                column: "AppUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_PostComments_AppUserId1",
                table: "PostComments",
                column: "AppUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PostComments_AspNetUsers_AppUserId1",
                table: "PostComments",
                column: "AppUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostMessages_AspNetUsers_AppUserId1",
                table: "PostMessages",
                column: "AppUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostRatings_AspNetUsers_AppUserId1",
                table: "PostRatings",
                column: "AppUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostReports_AspNetUsers_AppUserId1",
                table: "PostReports",
                column: "AppUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
