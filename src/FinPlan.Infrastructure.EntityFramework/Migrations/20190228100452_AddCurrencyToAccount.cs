using Microsoft.EntityFrameworkCore.Migrations;

namespace FinPlan.Infrastructure.EntityFramework.Migrations
{
    public partial class AddCurrencyToAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Accounts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Accounts");
        }
    }
}
