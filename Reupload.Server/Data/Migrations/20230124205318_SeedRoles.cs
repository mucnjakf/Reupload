using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reupload.Server.Data.Migrations
{
    public partial class SeedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2e6ca4da-8fef-4167-bfd3-4ea02e5d2062", "8bfd0f35-0243-41cd-87ac-f7fdaf3d458a", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "71e23399-bcad-4fb1-bede-179e3bc7410b", "b0568e7e-67d4-43ef-aa33-aa87af3a4728", "User", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2e6ca4da-8fef-4167-bfd3-4ea02e5d2062");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "71e23399-bcad-4fb1-bede-179e3bc7410b");
        }
    }
}
