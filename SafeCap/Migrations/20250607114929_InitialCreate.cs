﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SafeCap.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SC_Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SC_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SC_Alerts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    UserId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    AlertType = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Message = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SC_Alerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SC_Alerts_SC_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "SC_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SC_SensorReadings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    UserId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Temperature = table.Column<float>(type: "BINARY_FLOAT", nullable: false),
                    Humidity = table.Column<float>(type: "BINARY_FLOAT", nullable: false),
                    Light = table.Column<float>(type: "BINARY_FLOAT", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SC_SensorReadings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SC_SensorReadings_SC_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "SC_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SC_Alerts_UserId",
                table: "SC_Alerts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SC_SensorReadings_UserId",
                table: "SC_SensorReadings",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SC_Alerts");

            migrationBuilder.DropTable(
                name: "SC_SensorReadings");

            migrationBuilder.DropTable(
                name: "SC_Users");
        }
    }
}
