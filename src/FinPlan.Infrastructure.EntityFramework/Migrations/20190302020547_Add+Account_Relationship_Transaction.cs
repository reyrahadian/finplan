using Microsoft.EntityFrameworkCore.Migrations;

namespace FinPlan.Infrastructure.EntityFramework.Migrations
{
    public partial class AddAccount_Relationship_Transaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Categories_CategoryId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Transactions",
                newName: "TransactionCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_CategoryId",
                table: "Transactions",
                newName: "IX_Transactions_TransactionCategoryId");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Transactions",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Categories_TransactionCategoryId",
                table: "Transactions",
                column: "TransactionCategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Categories_TransactionCategoryId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "TransactionCategoryId",
                table: "Transactions",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_TransactionCategoryId",
                table: "Transactions",
                newName: "IX_Transactions_CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Categories_CategoryId",
                table: "Transactions",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
