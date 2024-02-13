using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reupload.Server.Data.Migrations
{
    public partial class PackageIsActiveUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PackageIsActive",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PackageIsActive",
                table: "AspNetUsers");
        }
    }
}
