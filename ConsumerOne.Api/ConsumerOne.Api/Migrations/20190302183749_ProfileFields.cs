using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConsumerOne.Api.Migrations
{
    public partial class ProfileFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UseTerms",
                keyColumn: "Id",
                keyValue: new Guid("7e9b1a8c-6d33-4a02-a701-7d52880d6c3c"));

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "About",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressAddon",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BusinessPhone",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cep",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.InsertData(
                table: "UseTerms",
                columns: new[] { "Id", "Value", "ValueEnUs", "ValueEs" },
                values: new object[] { new Guid("b56ed459-76f6-4a61-bbb8-4bec0db14171"), @"Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est eopksio laborum. Sed ut perspiciatis unde omnis istpoe natus error sit voluptatem accusantium doloremque eopsloi laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunot explicabo. Nemo ernim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sedopk quia consequuntur magni dolores eos qui rationesopl voluptatem sequi nesciunt. Neque porro quisquameo est, qui dolorem ipsum quia dolor sit amet, eopsmiep consectetur, adipisci velit, seisud quia non numquam eius modi tempora incidunt ut labore et dolore wopeir magnam aliquam quaerat voluptatem eoplmuriquisqu. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est eopksio laborum. Sed ut perspiciatis unde omnis istpoe natus error sit voluptatem accusantium doloremque eopsloi laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunot explicabo. Nemo ernim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sedopk quia consequuntur magni dolores eos qui rationesopl voluptatem sequi nesciunt. Neque porro quisquameo est, qui dolorem ipsum quia dolor sit amet, eopsmiep consectetur, adipisci velit, seisud quia non numquam eius modi tempora incidunt ut labore et dolore wopeir magnam aliquam quaerat voluptatem eoplmuriquisqu
Excepteur sint occaecat cupidatat non proident,
                sunt in culpa qui officia deserunt mollit anim id est eopksio laborum.Sed ut perspiciatis unde omnis istpoe natus error sit voluptatem accusantium doloremque eopsloi laudantium,
                totam rem aperiam,
                eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunot explicabo.Nemo ernim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit,
                sedopk quia consequuntur magni dolores eos qui rationesopl voluptatem sequi nesciunt.Neque porro quisquameo est,
                qui dolorem ipsum quia dolor sit amet,
                eopsmiep consectetur,
                adipisci velit,
                seisud quia non numquam eius modi tempora incidunt ut labore et dolore wopeir magnam aliquam quaerat voluptatem eoplmuriquisqu", null, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UseTerms",
                keyColumn: "Id",
                keyValue: new Guid("b56ed459-76f6-4a61-bbb8-4bec0db14171"));

            migrationBuilder.DropColumn(
                name: "About",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AddressAddon",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BusinessPhone",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Cep",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "City",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "State",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.InsertData(
                table: "UseTerms",
                columns: new[] { "Id", "Value", "ValueEnUs", "ValueEs" },
                values: new object[] { new Guid("7e9b1a8c-6d33-4a02-a701-7d52880d6c3c"), @"Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est eopksio laborum. Sed ut perspiciatis unde omnis istpoe natus error sit voluptatem accusantium doloremque eopsloi laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunot explicabo. Nemo ernim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sedopk quia consequuntur magni dolores eos qui rationesopl voluptatem sequi nesciunt. Neque porro quisquameo est, qui dolorem ipsum quia dolor sit amet, eopsmiep consectetur, adipisci velit, seisud quia non numquam eius modi tempora incidunt ut labore et dolore wopeir magnam aliquam quaerat voluptatem eoplmuriquisqu. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est eopksio laborum. Sed ut perspiciatis unde omnis istpoe natus error sit voluptatem accusantium doloremque eopsloi laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunot explicabo. Nemo ernim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sedopk quia consequuntur magni dolores eos qui rationesopl voluptatem sequi nesciunt. Neque porro quisquameo est, qui dolorem ipsum quia dolor sit amet, eopsmiep consectetur, adipisci velit, seisud quia non numquam eius modi tempora incidunt ut labore et dolore wopeir magnam aliquam quaerat voluptatem eoplmuriquisqu
Excepteur sint occaecat cupidatat non proident,
                sunt in culpa qui officia deserunt mollit anim id est eopksio laborum.Sed ut perspiciatis unde omnis istpoe natus error sit voluptatem accusantium doloremque eopsloi laudantium,
                totam rem aperiam,
                eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunot explicabo.Nemo ernim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit,
                sedopk quia consequuntur magni dolores eos qui rationesopl voluptatem sequi nesciunt.Neque porro quisquameo est,
                qui dolorem ipsum quia dolor sit amet,
                eopsmiep consectetur,
                adipisci velit,
                seisud quia non numquam eius modi tempora incidunt ut labore et dolore wopeir magnam aliquam quaerat voluptatem eoplmuriquisqu", null, null });
        }
    }
}
