using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FinPlan.Infrastructure.EntityFramework.Migrations
{
    public partial class Transaction_ChangeDate_ToNonNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Transactions",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Transactions",
                nullable: true,
                oldClrType: typeof(DateTime));
        }
    }
}
