using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DirectAlertBot.Migrations
{
    public partial class AddIsTriggeredProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTriggered",
                table: "Alerts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTriggered",
                table: "Alerts");
        }
    }
}
