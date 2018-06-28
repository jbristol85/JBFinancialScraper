using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JBFinancialScraper.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PortfolioInfo",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CaptureDate = table.Column<DateTime>(nullable: false),
                    PortfolioValue = table.Column<decimal>(nullable: false),
                    DayGain = table.Column<decimal>(nullable: false),
                    PercentDayGain = table.Column<double>(nullable: false),
                    TotalGain = table.Column<decimal>(nullable: false),
                    PercentTotalGain = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortfolioInfo", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "StockInfo",
                columns: table => new
                {
                    StockSymbol = table.Column<string>(nullable: true),
                    StockCurrentPrice = table.Column<decimal>(nullable: false),
                    StockPriceChange = table.Column<decimal>(nullable: false),
                    StockPriceChangePercent = table.Column<double>(nullable: false),
                    StockShares = table.Column<double>(nullable: false),
                    StockCostBasis = table.Column<double>(nullable: false),
                    StockMarketValue = table.Column<double>(nullable: false),
                    StockDayGain = table.Column<decimal>(nullable: false),
                    StockDayGainPercent = table.Column<double>(nullable: false),
                    StockTotalGain = table.Column<decimal>(nullable: false),
                    StockTotalGainPercent = table.Column<double>(nullable: false),
                    StockLots = table.Column<int>(nullable: false),
                    StockNotes = table.Column<string>(nullable: true),
                    StockInfoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PortfolioInfoID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockInfo", x => x.StockInfoId);
                    table.ForeignKey(
                        name: "FK_StockInfo_PortfolioInfo_PortfolioInfoID",
                        column: x => x.PortfolioInfoID,
                        principalTable: "PortfolioInfo",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockInfo_PortfolioInfoID",
                table: "StockInfo",
                column: "PortfolioInfoID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockInfo");

            migrationBuilder.DropTable(
                name: "PortfolioInfo");
        }
    }
}
