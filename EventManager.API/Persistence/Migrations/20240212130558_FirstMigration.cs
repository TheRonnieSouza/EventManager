﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevEvents.API.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DevEvents",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TITLE = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    DESCRIPTION = table.Column<string>(type: "VARCHAR(300)", maxLength: 300, nullable: true),
                    START_DATE = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    END_DATE = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevEvents", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DevEventsSpeaker",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TalkTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TalkDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkedInProfile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DevEventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevEventsSpeaker", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DevEventsSpeaker_DevEvents_DevEventId",
                        column: x => x.DevEventId,
                        principalTable: "DevEvents",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DevEventsSpeaker_DevEventId",
                table: "DevEventsSpeaker",
                column: "DevEventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DevEventsSpeaker");

            migrationBuilder.DropTable(
                name: "DevEvents");
        }
    }
}
