using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Machine_Setup_Worksheet.Migrations
{
    /// <inheritdoc />
    public partial class AddedWorkSetupCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Jaws",
                keyColumn: "JawId",
                keyValue: new Guid("38834df6-17d9-439f-b969-0e9e830788fc"));

            migrationBuilder.DeleteData(
                table: "Machines",
                keyColumn: "MachineId",
                keyValue: new Guid("0a1cee36-36fa-43f1-a943-86799ba5edcc"));

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "WorkSetups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WorkSetupCode",
                table: "WorkSetups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WorkSetupName",
                table: "WorkSetups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Jaws",
                columns: new[] { "JawId", "JawName" },
                values: new object[] { new Guid("da0ece3f-9c79-4ed1-a92f-cf72926922ed"), "Hard Jaws" });

            migrationBuilder.InsertData(
                table: "Machines",
                columns: new[] { "MachineId", "MachineName" },
                values: new object[] { new Guid("dcf79b70-f2b2-470a-9ab7-a74d5ae277b9"), "Hwacheon" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Jaws",
                keyColumn: "JawId",
                keyValue: new Guid("da0ece3f-9c79-4ed1-a92f-cf72926922ed"));

            migrationBuilder.DeleteData(
                table: "Machines",
                keyColumn: "MachineId",
                keyValue: new Guid("dcf79b70-f2b2-470a-9ab7-a74d5ae277b9"));

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "WorkSetups");

            migrationBuilder.DropColumn(
                name: "WorkSetupCode",
                table: "WorkSetups");

            migrationBuilder.DropColumn(
                name: "WorkSetupName",
                table: "WorkSetups");

            migrationBuilder.InsertData(
                table: "Jaws",
                columns: new[] { "JawId", "JawName" },
                values: new object[] { new Guid("38834df6-17d9-439f-b969-0e9e830788fc"), "Hard Jaws" });

            migrationBuilder.InsertData(
                table: "Machines",
                columns: new[] { "MachineId", "MachineName" },
                values: new object[] { new Guid("0a1cee36-36fa-43f1-a943-86799ba5edcc"), "Hwacheon" });
        }
    }
}
