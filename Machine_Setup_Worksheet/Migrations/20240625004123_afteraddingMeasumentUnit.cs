using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Machine_Setup_Worksheet.Migrations
{
    /// <inheritdoc />
    public partial class afteraddingMeasumentUnit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Jaws",
                keyColumn: "JawId",
                keyValue: new Guid("57d8ed55-77bc-4f9c-9f03-721606e6335e"));

            migrationBuilder.DeleteData(
                table: "Machines",
                keyColumn: "MachineId",
                keyValue: new Guid("357a6353-e064-4b78-8c68-ae5b44c070d4"));

            migrationBuilder.AddColumn<string>(
                name: "MeasurementUnit",
                table: "Setups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "inches");

            migrationBuilder.InsertData(
                table: "Jaws",
                columns: new[] { "JawId", "JawName" },
                values: new object[] { new Guid("ccc2d7a1-edce-40a6-8291-ca5c03e16b99"), "Hard Jaws" });

            migrationBuilder.InsertData(
                table: "Machines",
                columns: new[] { "MachineId", "MachineName" },
                values: new object[] { new Guid("d6cbd46c-4d29-4ddc-b91d-30c08892092a"), "Hwacheon" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Jaws",
                keyColumn: "JawId",
                keyValue: new Guid("ccc2d7a1-edce-40a6-8291-ca5c03e16b99"));

            migrationBuilder.DeleteData(
                table: "Machines",
                keyColumn: "MachineId",
                keyValue: new Guid("d6cbd46c-4d29-4ddc-b91d-30c08892092a"));

            migrationBuilder.DropColumn(
                name: "MeasurementUnit",
                table: "Setups");

            migrationBuilder.InsertData(
                table: "Jaws",
                columns: new[] { "JawId", "JawName" },
                values: new object[] { new Guid("57d8ed55-77bc-4f9c-9f03-721606e6335e"), "Hard Jaws" });

            migrationBuilder.InsertData(
                table: "Machines",
                columns: new[] { "MachineId", "MachineName" },
                values: new object[] { new Guid("357a6353-e064-4b78-8c68-ae5b44c070d4"), "Hwacheon" });
        }
    }
}
