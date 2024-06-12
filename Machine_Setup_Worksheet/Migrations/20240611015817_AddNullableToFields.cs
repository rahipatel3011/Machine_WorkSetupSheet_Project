using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Machine_Setup_Worksheet.Migrations
{
    /// <inheritdoc />
    public partial class AddNullableToFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Jaws",
                keyColumn: "JawId",
                keyValue: new Guid("da0ece3f-9c79-4ed1-a92f-cf72926922ed"));

            migrationBuilder.DeleteData(
                table: "Machines",
                keyColumn: "MachineId",
                keyValue: new Guid("dcf79b70-f2b2-470a-9ab7-a74d5ae277b9"));

            migrationBuilder.AlterColumn<string>(
                name: "WorkSetupCode",
                table: "WorkSetups",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "WorkSetups",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Jaws",
                columns: new[] { "JawId", "JawName" },
                values: new object[] { new Guid("bab2d697-bef4-4db9-b065-1bf441e7ce7a"), "Hard Jaws" });

            migrationBuilder.InsertData(
                table: "Machines",
                columns: new[] { "MachineId", "MachineName" },
                values: new object[] { new Guid("eb691098-dee4-430b-b2e9-871a8002b941"), "Hwacheon" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Jaws",
                keyColumn: "JawId",
                keyValue: new Guid("bab2d697-bef4-4db9-b065-1bf441e7ce7a"));

            migrationBuilder.DeleteData(
                table: "Machines",
                keyColumn: "MachineId",
                keyValue: new Guid("eb691098-dee4-430b-b2e9-871a8002b941"));

            migrationBuilder.AlterColumn<string>(
                name: "WorkSetupCode",
                table: "WorkSetups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "WorkSetups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Jaws",
                columns: new[] { "JawId", "JawName" },
                values: new object[] { new Guid("da0ece3f-9c79-4ed1-a92f-cf72926922ed"), "Hard Jaws" });

            migrationBuilder.InsertData(
                table: "Machines",
                columns: new[] { "MachineId", "MachineName" },
                values: new object[] { new Guid("dcf79b70-f2b2-470a-9ab7-a74d5ae277b9"), "Hwacheon" });
        }
    }
}
