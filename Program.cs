using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace ProfitCalculator
{
    public class Program
    {
        private string csvFilePath;
        private DateTime buyStocksDate;
        private DateTime sellStocksDate;
        private double taxPercentage;
        private double buyStocksBudget;
        private DayOfWeek buyStocksDay;
        private int buyStocksDayDelay;
        private int csvDateColumnColumnIndex;
        private int csvOpeningPriceColumnIndex;
        private int csvClosingPriceColumnIndex;
        private char csvValueSeparator;
        private CultureInfo csvCultureInfo;

        static void Main(string[] args)
        {
            Program program = new Program();
            program.Run();
            Console.ReadLine();
        }

        public void Run()
        {
            try
            {
                GetUserParameters();

                Stopwatch stopwatch = Stopwatch.StartNew();

                CsvDataManager csvDataManager = new CsvDataManager(csvFilePath, csvDateColumnColumnIndex, csvOpeningPriceColumnIndex, csvClosingPriceColumnIndex, csvValueSeparator, csvCultureInfo);
                List<DataPoint> dataPoints = csvDataManager.GetDataPointsFromCsv();

                ProfitCalculator profitCalculator = new ProfitCalculator(dataPoints, buyStocksDate, sellStocksDate, buyStocksBudget, buyStocksDay, buyStocksDayDelay);
                List<string> outputLogs = profitCalculator.CalculateProfitOverTime();

                string outputFilePath = csvFilePath + "-output- " + DateTime.Now.ToString("dd-MM-yyyy HH.mm.ss") + ".txt";
                File.WriteAllLines(outputFilePath, outputLogs, Encoding.UTF8);

                Console.WriteLine("Output file: " + outputFilePath);

                stopwatch.Stop();
                long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                Console.WriteLine("Execution Time: " + elapsedMilliseconds + " ms");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
            }
        }

        private void GetUserParameters()
        {
            csvFilePath = InputManager.Ask<string>("Type the path or drop the CSV file.", true);
            
            buyStocksDate = InputManager.Ask<DateTime>("Type the first month for analysis (23-may.-2001).");
            sellStocksDate = InputManager.Ask<DateTime>("Type the date to sell all stocks (28-dic.-2017).");
            taxPercentage = InputManager.Ask<double>("Type the deposit tax cut percentage without the percentage symbol (2).");
            double buyStocksBudgetUntaxxed = InputManager.Ask<double>("Type funds to deposit each time, before tax (50).");
            buyStocksBudget = buyStocksBudgetUntaxxed - (buyStocksBudgetUntaxxed * (taxPercentage / 100));
            buyStocksDay = InputManager.Ask<DayOfWeek>("Type the weekday on which you would like to make the monthly deposits. (thursday).");
            buyStocksDayDelay = InputManager.Ask<int>("Type a number to advance or delay the deposit date relative to the day of the weekday you entered. (1).");

            csvCultureInfo = InputManager.Ask<CultureInfo>("Type the ISO code of the CSV (es-US).");
            csvValueSeparator = InputManager.Ask<char>("Type the character used for separating values (;).");
            csvDateColumnColumnIndex = InputManager.Ask<int>("Type the index number of the date column for the CSV file (1).") - 1;
            csvClosingPriceColumnIndex = InputManager.Ask<int>("Type the index number of the closing column for the CSV file (2).") - 1;
            csvOpeningPriceColumnIndex = InputManager.Ask<int>("Type the index number of the opening column for the Csv file (3).") - 1;
        }

    }
}
