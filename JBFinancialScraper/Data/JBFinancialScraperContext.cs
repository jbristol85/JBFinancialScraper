using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace JBFinancialScraper.Models
{
    public class JBFinancialScraperContext : DbContext
    {
        public JBFinancialScraperContext (DbContextOptions<JBFinancialScraperContext> options)
            : base(options)
        {
        }

        public DbSet<JBFinancialScraper.Models.PortfolioInfo> PortfolioInfo { get; set; }

        public DbSet<JBFinancialScraper.Models.StockInfo> StockInfo
        {
            get; set;
        }
    }
}
