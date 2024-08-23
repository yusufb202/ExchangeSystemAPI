using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class WalletService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExchangeTransactions_Currencies_ToCurrencyId",
                table: "ExchangeTransactions");

            migrationBuilder.DropTable(
                name: "ExchangeTransactionHistories");

            migrationBuilder.CreateTable(
                name: "UserActivityReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TotalTransactions = table.Column<int>(type: "int", nullable: false),
                    TotalAmountExchanged = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MostTradedCurrency = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserActivityReports", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangeTransactions_Currencies_ToCurrencyId",
                table: "ExchangeTransactions",
                column: "ToCurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExchangeTransactions_Currencies_ToCurrencyId",
                table: "ExchangeTransactions");

            migrationBuilder.DropTable(
                name: "UserActivityReports");

            migrationBuilder.CreateTable(
                name: "ExchangeTransactionHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExchangeTransactionId = table.Column<int>(type: "int", nullable: false),
                    DateRecorded = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeTransactionHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExchangeTransactionHistories_ExchangeTransactions_ExchangeTransactionId",
                        column: x => x.ExchangeTransactionId,
                        principalTable: "ExchangeTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeTransactionHistories_ExchangeTransactionId",
                table: "ExchangeTransactionHistories",
                column: "ExchangeTransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangeTransactions_Currencies_ToCurrencyId",
                table: "ExchangeTransactions",
                column: "ToCurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id");
        }
    }
}
