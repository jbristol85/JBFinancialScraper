﻿// <auto-generated />
using System;
using JBFinancialScraper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace JBFinancialScraper.Migrations
{
    [DbContext(typeof(JBFinancialScraperContext))]
    [Migration("20180628160440_switchDatatypes")]
    partial class switchDatatypes
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("JBFinancialScraper.Models.PortfolioInfo", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CaptureDate");

                    b.Property<decimal>("DayGain");

                    b.Property<double>("PercentDayGain");

                    b.Property<double>("PercentTotalGain");

                    b.Property<decimal>("PortfolioValue");

                    b.Property<decimal>("TotalGain");

                    b.HasKey("ID");

                    b.ToTable("PortfolioInfo");
                });

            modelBuilder.Entity("JBFinancialScraper.Models.StockInfo", b =>
                {
                    b.Property<int>("StockInfoId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("PortfolioInfoID");

                    b.Property<double>("StockCostBasis");

                    b.Property<decimal>("StockCurrentPrice");

                    b.Property<decimal>("StockDayGain");

                    b.Property<string>("StockDayGainPercent");

                    b.Property<int>("StockLots");

                    b.Property<double>("StockMarketValue");

                    b.Property<string>("StockNotes");

                    b.Property<decimal>("StockPriceChange");

                    b.Property<string>("StockPriceChangePercent");

                    b.Property<double>("StockShares");

                    b.Property<string>("StockSymbol");

                    b.Property<decimal>("StockTotalGain");

                    b.Property<string>("StockTotalGainPercent");

                    b.HasKey("StockInfoId");

                    b.HasIndex("PortfolioInfoID");

                    b.ToTable("StockInfo");
                });

            modelBuilder.Entity("JBFinancialScraper.Models.StockInfo", b =>
                {
                    b.HasOne("JBFinancialScraper.Models.PortfolioInfo", "PortfolioInfo")
                        .WithMany("StockInfo")
                        .HasForeignKey("PortfolioInfoID");
                });
#pragma warning restore 612, 618
        }
    }
}
