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


//          

//            var options = new ChromeOptions();
//            options.AddArgument("--disable-gpu");
            // creates new chromeDriver and goes to webpage
            var chromeDriver = new ChromeDriver(@"./");
            chromeDriver.Navigate().GoToUrl("https://login.yahoo.com/");
            // waits for page to load
            //            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(14);
            // login sequence
            //            chromeDriver.FindElementById("uh-signedin").Click();
            chromeDriver.FindElementById("login-username").Click();
//            chromeDriver.Keyboard.SendKeys(username);
            chromeDriver.Keyboard.SendKeys(usernamePassword.YahooUsername());
            chromeDriver.Keyboard.SendKeys(Keys.Enter);
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            chromeDriver.FindElementById("login-passwd").Click();
//            chromeDriver.Keyboard.SendKeys(password1);
            chromeDriver.Keyboard.SendKeys(usernamePassword.YahooPassword());
            chromeDriver.Keyboard.SendKeys(Keys.Enter);
            //navigate to portfolio with stocks
            //            chromeDriver.Navigate().GoToUrl("https://finance.yahoo.com/portfolios?bypass=true");

            //            chromeDriver.FindElementById("__dialog").Click();
            //            chromeDriver.Keyboard.SendKeys(Keys.Escape);
            //navigate to Jbristol Portfolio
            chromeDriver.Navigate().GoToUrl("https://finance.yahoo.com/portfolio/p_1/view/v2");
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            chromeDriver.FindElement(By.Id("__dialog")).Click();
            chromeDriver.Keyboard.SendKeys(Keys.Escape);
            // get currentValue dayGains and totalGain
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
            //            capture.PercentDayGain = double.Parse(dayGainPercent.Remove(0, 1).Remove(dayGainPercent.Length - 2, dayGainPercent.Length - 1));
            capture.PercentDayGain = (double.Parse(dayGainPercent.Trim(new char[] { ' ', '(', '%', ')' }))/100);
            capture.TotalGain = decimal.Parse(totalGain[0]);
            //            capture.PercentTotalGain = double.Parse(totalGainPercent.Remove(0, 1).Remove(dayGainPercent.Length - 2, dayGainPercent.Length - 1));
            capture.PercentTotalGain = (double.Parse(totalGainPercent.Trim(new char[] { ' ', '(', '%', ')' }))/100);
//            Console.WriteLine(
//                "currentValue: {0}, dayGain: {1}, totalGain: {2}, dayGainPercent: {3}, totalGainPercent: {4}",
//                PortfolioValue, DayGain, TotalGain, PercentDayGain, PercentTotalGain);

            //            // scrape symbols
            var portfolioStockInfo = new List<StockInfo>();
            //            var table = chromeDriver.FindElement(By.TagName("table"));
            var table = chromeDriver.FindElement(By.ClassName("tJDbU"));
            //            Console.WriteLine("Table" + table);
            //            var tableRows = table.FindElements(By.TagName("tr"));
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

                    Console.WriteLine("ChangePrcentDollar type: " + changePercentDollar[0].GetType());
                    var StockPriceChangePercentTemp = changePercentDollar[0].Trim( new char[] { '+', '\n'});
                    Console.WriteLine("StockPriceChangePercentTemp: " + StockPriceChangePercentTemp + StockPriceChangePercentTemp.GetType());
                    var StockDayGainPercentTemp = dayGainPercentDollar[0].Trim(new char[] { '+','\n' });
                    Console.WriteLine("StockDayGainPercentTemp: " + StockDayGainPercentTemp);
                    var StockTotalGainPercentTemp = totalGainPercentDollar[0].Trim(new char[] { '+', '\n' });
                    Console.WriteLine("StockTotalGainPercentTemp: " + StockTotalGainPercentTemp);

                    var StockSymbol = symbolPrice[0].Trim(new char [] { '\n' });
                    Console.WriteLine("Stock Symbol: " + StockSymbol);
                    var StockCurrentPrice = Convert.ToDecimal(symbolPrice[1]);
                    Console.WriteLine("Current Price: " + StockCurrentPrice);
                    var StockPriceChange = Convert.ToDecimal(changePercentDollar[1]);
                    Console.WriteLine("StockPriceChange: " + StockPriceChange);
                    var StockPriceChangePercent = StockPriceChangePercentTemp;
                    Console.WriteLine("StockPriceChangePErcent: " + StockPriceChangePercent);
                    var StockShares = Convert.ToDouble(listRowData[2]);
                    Console.WriteLine("Stock Shares: " + StockShares);
                    var StockCostBasis = Convert.ToDouble(listRowData[3]);
                    Console.WriteLine("StockCostBasis: " + StockCostBasis);
                    var StockMarketValue = Convert.ToDouble(listRowData[4]);
                    Console.WriteLine("StockMarketValue: "+ StockMarketValue);
                    var StockDayGain = Convert.ToDecimal(dayGainPercentDollar[1]);
                    Console.WriteLine("StockDayGain: " + StockDayGain);
                    var StockDayGainPercent = StockDayGainPercentTemp;
                    Console.WriteLine("StockDayGainPercent: " + StockDayGainPercent);
                    var StockTotalGain = Convert.ToDecimal(totalGainPercentDollar[1]);
                    Console.WriteLine("StockTotalGain: " + StockTotalGain);
                    var StockTotalGainPercent = StockTotalGainPercentTemp;
                    Console.WriteLine("StockTotalGainPercent: " + StockTotalGainPercent);
                    var StockLots = Convert.ToInt32(numLots[0]);
                    Console.WriteLine("StockLots: " + StockLots);
                    var StockNotes = listRowData[8];
                    Console.WriteLine("StockNotes: " + StockNotes);

                    portfolioStockInfo.Add(new StockInfo()
                    {

                        //                        StockSymbol = symbolPrice[0],
                        //                        StockCurrentPrice = symbolPrice[1],
                        //                        StockPriceChange = changePercentDollar[1],
                        //                        StockPriceChangePercent = changePercentDollar[0].Trim(new char[] {' ', '+', '%'}),
                        //                        StockShares = listRowData[2],
                        //                        StockCostBasis = listRowData[3],
                        //                        StockMarketValue = listRowData[4],
                        //                        StockDayGain = dayGainPercentDollar[1],
                        //                        StockDayGainPercent = dayGainPercentDollar[0].Trim(new char[] {' ', '+', '%'}),
                        //                        StockTotalGain = totalGainPercentDollar[1],
                        //                        StockTotalGainPercent = totalGainPercentDollar[0].Trim(new char[] {' ', '+', '%'}),
                        //                        StockLots = numLots[0],
                        //                        StockNotes = listRowData[8]

                        //                        StockSymbol = symbolPrice[0],
                        //                        StockCurrentPrice = Convert.ToDecimal(symbolPrice[1]),
                        //                        StockPriceChange = Convert.ToDecimal(changePercentDollar[1]),
                        //                        StockPriceChangePercent =(Convert.ToDouble(StockPriceChangePercentTemp) / 100),
                        //                        StockShares = Convert.ToDouble(listRowData[2]),
                        //                        StockCostBasis = Convert.ToDouble(listRowData[3]),
                        //                        StockMarketValue = Convert.ToDouble(listRowData[4]),
                        //                        StockDayGain = Convert.ToDecimal(dayGainPercentDollar[1]),
                        //                        StockDayGainPercent =((Convert.ToDouble(StockDayGainPercentTemp)) / 100),
                        //                        StockTotalGain = Convert.ToDecimal(totalGainPercentDollar[1]),
                        //                        StockTotalGainPercent =((Convert.ToDouble(StockTotalGainPercentTemp)) / 100),
                        //                        StockLots = Convert.ToInt32(numLots[0]),
                        //                        StockNotes = listRowData[8]

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
            chromeDriver.Quit();
//            Console.WriteLine("portfolioStockInfo:   " + portfolioStockInfo);
            capture.StockInfo = portfolioStockInfo;
            return capture;
        }
    }
}
