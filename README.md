# JBScraper
CSharp webscraper using .Net MVC Core, Entity Framework Core, and Selenium

This app is designed to autonomously log into your Yahoo Finance account and scrape your portfolio and stock data for you.  



Project Requirements:

 -   Please create an account @ https://finance.yahoo.com/ and add the stocks of your choice.
 

 -   App requires a "usernamePassword.cs" class in root of the project using the following format:

    namespace JBFinancialScraper
    {
        public class usernamePassword
        {
            public static string YahooUsername()
            {
                var username = <Your username here>;
                return username;
            }
            public static string YahooPassword()
            {
                var password = <Your password here>;
                return password;
            }
        }
    }
