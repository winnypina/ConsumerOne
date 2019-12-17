using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConsumerOne.Api.Migrations
{
    public partial class changeProfileType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UseTerms",
                keyColumn: "Id",
                keyValue: new Guid("9e9b59cc-998e-49ee-b716-2e576812506d"));

            migrationBuilder.InsertData(
                table: "UseTerms",
                columns: new[] { "Id", "Value", "ValueEnUs", "ValueEs" },
                values: new object[] { new Guid("b05f3e09-689d-481d-a19d-003dbac36880"), @"Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est eopksio laborum. Sed ut perspiciatis unde omnis istpoe natus error sit voluptatem accusantium doloremque eopsloi laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunot explicabo. Nemo ernim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sedopk quia consequuntur magni dolores eos qui rationesopl voluptatem sequi nesciunt. Neque porro quisquameo est, qui dolorem ipsum quia dolor sit amet, eopsmiep consectetur, adipisci velit, seisud quia non numquam eius modi tempora incidunt ut labore et dolore wopeir magnam aliquam quaerat voluptatem eoplmuriquisqu. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est eopksio laborum. Sed ut perspiciatis unde omnis istpoe natus error sit voluptatem accusantium doloremque eopsloi laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunot explicabo. Nemo ernim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sedopk quia consequuntur magni dolores eos qui rationesopl voluptatem sequi nesciunt. Neque porro quisquameo est, qui dolorem ipsum quia dolor sit amet, eopsmiep consectetur, adipisci velit, seisud quia non numquam eius modi tempora incidunt ut labore et dolore wopeir magnam aliquam quaerat voluptatem eoplmuriquisqu
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
                keyValue: new Guid("b05f3e09-689d-481d-a19d-003dbac36880"));

            migrationBuilder.InsertData(
                table: "UseTerms",
                columns: new[] { "Id", "Value", "ValueEnUs", "ValueEs" },
                values: new object[] { new Guid("9e9b59cc-998e-49ee-b716-2e576812506d"), @"Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est eopksio laborum. Sed ut perspiciatis unde omnis istpoe natus error sit voluptatem accusantium doloremque eopsloi laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunot explicabo. Nemo ernim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sedopk quia consequuntur magni dolores eos qui rationesopl voluptatem sequi nesciunt. Neque porro quisquameo est, qui dolorem ipsum quia dolor sit amet, eopsmiep consectetur, adipisci velit, seisud quia non numquam eius modi tempora incidunt ut labore et dolore wopeir magnam aliquam quaerat voluptatem eoplmuriquisqu. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est eopksio laborum. Sed ut perspiciatis unde omnis istpoe natus error sit voluptatem accusantium doloremque eopsloi laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunot explicabo. Nemo ernim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sedopk quia consequuntur magni dolores eos qui rationesopl voluptatem sequi nesciunt. Neque porro quisquameo est, qui dolorem ipsum quia dolor sit amet, eopsmiep consectetur, adipisci velit, seisud quia non numquam eius modi tempora incidunt ut labore et dolore wopeir magnam aliquam quaerat voluptatem eoplmuriquisqu
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
