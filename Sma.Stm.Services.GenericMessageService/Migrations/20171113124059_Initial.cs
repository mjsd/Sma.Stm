using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Sma.Stm.Services.GenericMessageService.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "PublishedMessage_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "UploadedMessage_hilo",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "PublishedMessage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: true),
                    DataId = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    PublishTime = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublishedMessage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UploadedMessage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: true),
                    DataId = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    FromOrgId = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    FromServiceId = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    ReceiveTime = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadedMessage", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PublishedMessage");

            migrationBuilder.DropTable(
                name: "UploadedMessage");

            migrationBuilder.DropSequence(
                name: "PublishedMessage_hilo");

            migrationBuilder.DropSequence(
                name: "UploadedMessage_hilo");
        }
    }
}
