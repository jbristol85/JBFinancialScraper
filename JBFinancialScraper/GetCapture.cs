using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using JBFinancialScraper.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace JBFinancialScraper
{
    public class GetCapture
    {
        public static PortfolioInfo Capture()
        {
            var capture = new PortfolioInfo();

            var options = new ChromeOptions();
            options.AddArgument("--headless");
            // creates new chromeDriver and goes to webpage
            var chromeDriver = new ChromeDriver(@"./", options);
            chromeDriver.Navigate().GoToUrl("https://login.yahoo.com/");
         
            chromeDriver.FindElementById("login-username").Click();
            chromeDriver.Keyboard.SendKeys(usernamePassword.YahooUsername());
            chromeDriver.Keyboard.SendKeys(Keys.Enter);
            chromeDriver.FindElementById("login-passwd").Click();
            chromeDriver.Keyboard.SendKeys(usernamePassword.YahooPassword());
            chromeDriver.Keyboard.SendKeys(Keys.Enter);
            //navigate to Jbristol Portfolio
            chromeDriver.Navigate().GoToUrl("https://finance.yahoo.com/portfolio/p_1/view/v2");
            chromeDriver.FindElement(By.Id("__dialog")).Click();
            chromeDriver.Keyboard.SendKeys(Keys.Escape);


            getPortfolioData(chromeDriver, capture);
            var portfolioStockInfo = getStockData(chromeDriver);



            chromeDriver.Quit();
            capture.StockInfo = portfolioStockInfo;
            return capture;
        }


        private static List<StockInfo> getStockData(ChromeDriver chromeDriver)
        {
//            // scrape symbols
            var portfolioStockInfo = new List<StockInfo>();
            //            var table = chromeDriver.FindElement(By.TagName("table"));
            var table = chromeDriver.FindElement(By.ClassName("tJDbU"));
            var tableRows = table.FindElements(By.ClassName("_14MJo"));
            Console.WriteLine("tablerows" + tableRows.Count);
            var listRowData = new List<string>();

            // each row in the table
            foreach (var row in tableRows)
            {
                var tableColumn = row.FindElements(By.TagName("td"));
                if (tableColumn.Count > 0)
                {
                    foreach (var col in tableColumn)
                    {
                        listRowData.Add(col.Text);
                    }

                    var symbolPrice = listRowData[0].ToString().Split("\n");
                    var changePercentDollar = listRowData[1].ToString().Split("\n");
                    var dayGainPercentDollar = listRowData[5].ToString().Split("\n");
                    var totalGainPercentDollar = listRowData[6].ToString().Split("\n");
                    var numLots = listRowData[7].Split(' ');
                    var splitNotes = listRowData[8].Split("Trade");

                    var StockPriceChangePercentTemp = changePercentDollar[0].Trim(new char[] {'+', '\n'});
                    var StockDayGainPercentTemp = dayGainPercentDollar[0].Trim(new char[] {'+', '\n'});
                    var StockTotalGainPercentTemp = totalGainPercentDollar[0].Trim(new char[] {'+', '\n'});

                    var StockSymbol = symbolPrice[0].Trim(new char[] {'\n'});
                    var StockCurrentPrice = Convert.ToDecimal(symbolPrice[1]);
                    var StockPriceChange = Convert.ToDecimal(changePercentDollar[1]);
                    var StockPriceChangePercent = StockPriceChangePercentTemp;
                    var StockShares = Convert.ToDouble(listRowData[2]);
                    var StockCostBasis = Convert.ToDouble(listRowData[3]);
                    var StockMarketValue = Convert.ToDouble(listRowData[4]);
                    var StockDayGain = Convert.ToDecimal(dayGainPercentDollar[1]);
                    var StockDayGainPercent = StockDayGainPercentTemp;
                    var StockTotalGain = Convert.ToDecimal(totalGainPercentDollar[1]);
                    var StockTotalGainPercent = StockTotalGainPercentTemp;
                    var StockLots = Convert.ToInt32(numLots[0]);
                    var StockNotes = splitNotes[0];

                    portfolioStockInfo.Add(new StockInfo()
                    {
                        StockSymbol = StockSymbol,
                        StockCurrentPrice = StockCurrentPrice,
                        StockPriceChange = StockPriceChange,
                        StockPriceChangePercent = StockPriceChangePercent,
                        StockShares = StockShares,
                        StockCostBasis = StockCostBasis,
                        StockMarketValue = StockMarketValue,
                        StockDayGain = StockDayGain,
                        StockDayGainPercent = StockDayGainPercent,
                        StockTotalGain = StockTotalGain,
                        StockTotalGainPercent = StockTotalGainPercent,
                        StockLots = StockLots,
                        StockNotes = StockNotes
                    });
                }

                Console.WriteLine("going to clear");
                listRowData.Clear();
            }

            return portfolioStockInfo;
        }

        private static void getPortfolioData(ChromeDriver chromeDriver, PortfolioInfo capture)
        {
            var currentValue = chromeDriver
                .FindElementByXPath("//*[@id=\"main\"]/section/header/div/div[1]/div/div[2]/p[1]").Text;
            var dayGain = chromeDriver
                .FindElementByXPath("//*[@id=\"main\"]/section/header/div/div[1]/div/div[2]/p[2]/span").Text.Split(' ');
            var totalGain = chromeDriver
                .FindElementByXPath("//*[@id=\"main\"]/section/header/div/div[1]/div/div[2]/p[3]/span").Text.Split(' ');
            var dayGainPercent = dayGain[1];
            var totalGainPercent = totalGain[1];
            capture.CaptureDate = DateTime.Now;
            capture.PortfolioValue = decimal.Parse(currentValue, NumberStyles.Currency);
            capture.DayGain = decimal.Parse(dayGain[0]);
            capture.PercentDayGain = (double.Parse(dayGainPercent.Trim(new char[] {' ', '(', '%', ')'})) / 100);
            capture.TotalGain = decimal.Parse(totalGain[0]);
            capture.PercentTotalGain = (double.Parse(totalGainPercent.Trim(new char[] {' ', '(', '%', ')'})) / 100);
//            Console.WriteLine(
//                "currentValue: {0}, dayGain: {1}, totalGain: {2}, dayGainPercent: {3}, totalGainPercent: {4}",
//                PortfolioValue, DayGain, TotalGain, PercentDayGain, PercentTotalGain);
        }
    }
}
