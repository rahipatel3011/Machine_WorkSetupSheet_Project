using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Machine_Setup_Worksheet.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Jaws",
                columns: table => new
                {
                    JawId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JawName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jaws", x => x.JawId);
                });

            migrationBuilder.CreateTable(
                name: "Machines",
                columns: table => new
                {
                    MachineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MachineName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Machines", x => x.MachineId);
                });

            migrationBuilder.CreateTable(
                name: "WorkSetups",
                columns: table => new
                {
                    WorkSetupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkSetups", x => x.WorkSetupId);
                });

            migrationBuilder.CreateTable(
                name: "Setups",
                columns: table => new
                {
                    SetupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SetupNumber = table.Column<int>(type: "int", nullable: false),
                    JawId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Toothinfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SetupImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaterialSize = table.Column<double>(type: "float", nullable: false),
                    WorkSetupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setups", x => x.SetupId);
                    table.ForeignKey(
                        name: "FK_Setups_Jaws_JawId",
                        column: x => x.JawId,
                        principalTable: "Jaws",
                        principalColumn: "JawId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Setups_WorkSetups_WorkSetupId",
                        column: x => x.WorkSetupId,
                        principalTable: "WorkSetups",
                        principalColumn: "WorkSetupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Jaws",
                columns: new[] { "JawId", "JawName" },
                values: new object[] { new Guid("38834df6-17d9-439f-b969-0e9e830788fc"), "Hard Jaws" });

            migrationBuilder.InsertData(
                table: "Machines",
                columns: new[] { "MachineId", "MachineName" },
                values: new object[] { new Guid("0a1cee36-36fa-43f1-a943-86799ba5edcc"), "Hwacheon" });

            migrationBuilder.CreateIndex(
                name: "IX_Setups_JawId",
                table: "Setups",
                column: "JawId");

            migrationBuilder.CreateIndex(
                name: "IX_Setups_WorkSetupId",
                table: "Setups",
                column: "WorkSetupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Machines");

            migrationBuilder.DropTable(
                name: "Setups");

            migrationBuilder.DropTable(
                name: "Jaws");

            migrationBuilder.DropTable(
                name: "WorkSetups");
        }
    }
}
