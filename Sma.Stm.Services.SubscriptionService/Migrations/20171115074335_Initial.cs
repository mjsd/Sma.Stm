using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Sma.Stm.Services.SubscriptionService.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "SubscriptionItem_hilo",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "SubscriptionItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false),
                    CallbackEndpoint = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    DataId = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    OrgId = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    ServiceId = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionItem", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubscriptionItem");

            migrationBuilder.DropSequence(
                name: "SubscriptionItem_hilo");
        }
    }
}
