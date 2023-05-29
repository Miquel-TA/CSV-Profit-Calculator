using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace ProfitCalculator
{
    internal class Program
    {
        private string csvFilePath;
        private DateTime analyseSinceDate;
        private DateTime analyseUntilDate;
        private double taxPercentage;
        private double depositQuantity;
        private DayOfWeek depositDay;
        private int depositDayDelay;
        private int csvDateColumnColumnIndex;
        private int csvOpeningPriceColumnIndex;
        private int csvClosingPriceColumnIndex;
        private char csvValueSeparator;
        private CultureInfo csvCultureInfo;

        static void Main(string[] args)
        {
            Program program = new Program();
            program.Run();
        }

        private void Run()
        {
            GetUserParameters();

            CsvDataManager csvDataManager = new CsvDataManager(csvFilePath, csvDateColumnColumnIndex, csvOpeningPriceColumnIndex, csvClosingPriceColumnIndex, csvValueSeparator, csvCultureInfo);
            List<DataPoint> dataPoints = csvDataManager.GetDataPointsFromCsv();

            ProfitCalculator profitCalculator = new ProfitCalculator(dataPoints, analyseSinceDate, analyseUntilDate, depositQuantity, depositDay, depositDayDelay);
            List<string> outputLogs = profitCalculator.CalculateProfitOverTime();

            string outputFilePath = csvFilePath + "-output- " + DateTime.Now.ToString("dd-MM-yyyy HH.mm.ss") + ".txt";
            File.WriteAllLines(outputFilePath, outputLogs, Encoding.UTF8);

            Console.WriteLine(outputFilePath);
        }

        private void GetUserParameters()
        {
            csvFilePath = InputManager.Ask<string>("Type the path or drop the Csv file.", true);
            
            analyseSinceDate = InputManager.Ask<DateTime>("Type the first month for analysis (23-may.-2001).");
            analyseUntilDate = InputManager.Ask<DateTime>("Type the last month for analysis (28-dic.-2017).");
            taxPercentage = InputManager.Ask<double>("Type the deposit tax cut percentage without the percentage symbol (2).");
            double depositQuantityUntaxxed = InputManager.Ask<double>("Type funds deposited each time, before tax (50).");
            depositQuantity = depositQuantityUntaxxed - (depositQuantityUntaxxed * (taxPercentage / 100));
            depositDay = InputManager.Ask<DayOfWeek>("Type the day of the week when you receive your paycheck at the end of each month (thursday).");
            depositDayDelay = InputManager.Ask<int>("Type the advance or delay for the deposit relative to your paycheck day. Can be zero. (+1).");

            csvDateColumnColumnIndex = InputManager.Ask<int>("Type the index number of the date column for the Csv file (1).") - 1;
            csvOpeningPriceColumnIndex = InputManager.Ask<int>("Type the index number of the opening column for the Csv file (2).") - 1;
            csvClosingPriceColumnIndex = InputManager.Ask<int>("Type the index number of the closing column for the Csv file (3).") - 1;
            csvCultureInfo = InputManager.Ask<CultureInfo>("Type the ISO code of the CSV (es-US).");
            csvValueSeparator = InputManager.Ask<char>("Type the character used for separating values (;).");
        }

    }
}
