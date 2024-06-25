using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Machine_Setup_Worksheet.Migrations
{
    /// <inheritdoc />
    public partial class afteraddingMaterialShape : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Jaws",
                keyColumn: "JawId",
                keyValue: new Guid("ccc2d7a1-edce-40a6-8291-ca5c03e16b99"));

            migrationBuilder.DeleteData(
                table: "Machines",
                keyColumn: "MachineId",
                keyValue: new Guid("d6cbd46c-4d29-4ddc-b91d-30c08892092a"));

            migrationBuilder.AlterColumn<string>(
                name: "MeasurementUnit",
                table: "Setups",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "inches",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "inches");

            migrationBuilder.AddColumn<string>(
                name: "MaterialShape",
                table: "Setups",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Jaws",
                columns: new[] { "JawId", "JawName" },
                values: new object[] { new Guid("2fc7b698-a207-4f3a-975b-f4f1f3c74bf4"), "Hard Jaws" });

            migrationBuilder.InsertData(
                table: "Machines",
                columns: new[] { "MachineId", "MachineName" },
                values: new object[] { new Guid("12796546-0d0c-42bc-828d-f4e77534a00d"), "Hwacheon" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Jaws",
                keyColumn: "JawId",
                keyValue: new Guid("2fc7b698-a207-4f3a-975b-f4f1f3c74bf4"));

            migrationBuilder.DeleteData(
                table: "Machines",
                keyColumn: "MachineId",
                keyValue: new Guid("12796546-0d0c-42bc-828d-f4e77534a00d"));

            migrationBuilder.DropColumn(
                name: "MaterialShape",
                table: "Setups");

            migrationBuilder.AlterColumn<string>(
                name: "MeasurementUnit",
                table: "Setups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "inches",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValue: "inches");

            migrationBuilder.InsertData(
                table: "Jaws",
                columns: new[] { "JawId", "JawName" },
                values: new object[] { new Guid("ccc2d7a1-edce-40a6-8291-ca5c03e16b99"), "Hard Jaws" });

            migrationBuilder.InsertData(
                table: "Machines",
                columns: new[] { "MachineId", "MachineName" },
                values: new object[] { new Guid("d6cbd46c-4d29-4ddc-b91d-30c08892092a"), "Hwacheon" });
        }
    }
}
