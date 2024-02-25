using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Usable_Security_Project_Key_Registry.Migrations
{
    /// <inheritdoc />
    public partial class AddVillaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "phoneNumber",
                table: "User",
                newName: "QRSignature");

            migrationBuilder.AddColumn<string>(
                name: "PINSignature",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PINSignature",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "QRSignature",
                table: "User",
                newName: "phoneNumber");
        }
    }
}
