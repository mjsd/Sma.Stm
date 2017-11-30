using Microsoft.EntityFrameworkCore.Migrations;

namespace Sma.Stm.Services.AuthorizationService.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "AuthorizationItem_hilo",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "AuthorizationItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false),
                    DataId = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    OrgId = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorizationItem", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorizationItem");

            migrationBuilder.DropSequence(
                name: "AuthorizationItem_hilo");
        }
    }
}
