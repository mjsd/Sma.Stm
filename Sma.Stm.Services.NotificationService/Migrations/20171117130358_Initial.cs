using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Sma.Stm.Services.NotificationService.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "Notification_hilo",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Body = table.Column<string>(nullable: true),
                    Fetched = table.Column<bool>(nullable: false),
                    FromOrgId = table.Column<string>(maxLength: 250, nullable: true),
                    FromOrgName = table.Column<string>(maxLength: 250, nullable: true),
                    FromServiceId = table.Column<string>(maxLength: 250, nullable: true),
                    NotificationCreatedAt = table.Column<DateTime>(nullable: false),
                    NotificationSource = table.Column<int>(maxLength: 10, nullable: false),
                    NotificationType = table.Column<int>(maxLength: 100, nullable: false),
                    ReceivedAt = table.Column<DateTime>(nullable: false),
                    Subject = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subscribers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    NotificationEndpointUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscribers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "Subscribers");

            migrationBuilder.DropSequence(
                name: "Notification_hilo");
        }
    }
}
