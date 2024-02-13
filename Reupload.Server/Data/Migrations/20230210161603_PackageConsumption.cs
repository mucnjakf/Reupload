using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reupload.Server.Data.Migrations
{
    public partial class PackageConsumption : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PhotoUploadLimit",
                table: "Packages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "PackageConsumptionId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "PackageConsumption",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PhotoUploadAmount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageConsumption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PackageConsumption_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PackageConsumption_Packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            
            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: new Guid("49DD61D6-E931-498E-AD34-346039054D87"),
                column: "PhotoUploadLimit",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: new Guid("DC197CA8-5129-49C2-BBE0-B75FAC7B8EA6"),
                column: "PhotoUploadLimit",
                value: 100);

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: new Guid("CF47D685-F74A-428A-A5F9-583085973958"),
                column: "PhotoUploadLimit",
                value: -1);

            migrationBuilder.CreateIndex(
                name: "IX_PackageConsumption_PackageId",
                table: "PackageConsumption",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_PackageConsumption_UserId",
                table: "PackageConsumption",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PackageConsumption");

            migrationBuilder.DropColumn(
                name: "PhotoUploadLimit",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "PackageConsumptionId",
                table: "AspNetUsers");
        }
    }
}
