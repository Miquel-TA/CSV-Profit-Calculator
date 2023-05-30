using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace ProfitCalculator
{
    public class Program
    {
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
                Parameters parameters = GetUserParameters();

                Stopwatch stopwatch = Stopwatch.StartNew();

                CsvDataManager csvDataManager = new CsvDataManager(parameters.CsvFilePath, parameters.CsvDateColumnColumnIndex, parameters.CsvOpeningPriceColumnIndex, parameters.CsvClosingPriceColumnIndex, parameters.CsvValueSeparator, parameters.CsvCultureInfo);
                List<DataPoint> dataPoints = csvDataManager.GetDataPointsFromCsv();

                ProfitCalculator profitCalculator = new ProfitCalculator(dataPoints, parameters.BuyStocksDate, parameters.SellStocksDate, parameters.BuyStocksBudget, parameters.BuyStocksDay, parameters.BuyStocksDayDelay);
                List<string> outputLogs = profitCalculator.CalculateProfitOverTime();

                string outputFilePath = parameters.CsvFilePath + "-output- " + DateTime.Now.ToString("dd-MM-yyyy HH.mm.ss") + ".txt";
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

        private Parameters GetUserParameters()
        {
            InputManager inputManager = new InputManager();
            Parameters parameters;
            string parametersFilePath = ReadValueFromAppConfig("configFilePath");

            if (parametersFilePath == null)
            {
                string csvFilePath = inputManager.AskUser<string>("Type the path or drop the CSV file.", true);

                DateTime buyStocksDate = inputManager.AskUser<DateTime>("Type the first month for analysis (23-may.-2001).");
                DateTime sellStocksDate = inputManager.AskUser<DateTime>("Type the date to sell all stocks (28-dic.-2017).");
                double taxPercentage = inputManager.AskUser<double>("Type the deposit tax cut percentage without the percentage symbol (2).");
                double buyStocksBudgetUntaxxed = inputManager.AskUser<double>("Type funds to deposit each time, before tax (50).");
                double buyStocksBudget = buyStocksBudgetUntaxxed - (buyStocksBudgetUntaxxed * (taxPercentage / 100));
                DayOfWeek buyStocksDay = inputManager.AskUser<DayOfWeek>("Type the weekday on which you would like to make the monthly deposits. (thursday).");
                int buyStocksDayDelay = inputManager.AskUser<int>("Type a number to advance or delay the deposit date relative to the day of the weekday you entered. (1).");

                CultureInfo csvCultureInfo = inputManager.AskUser<CultureInfo>("Type the ISO code of the CSV (es-US).");
                char csvValueSeparator = inputManager.AskUser<char>("Type the character used for separating values (;).");
                int csvDateColumnColumnIndex = inputManager.AskUser<int>("Type the index number of the date column for the CSV file (1).") - 1;
                int csvClosingPriceColumnIndex = inputManager.AskUser<int>("Type the index number of the closing column for the CSV file (2).") - 1;
                int csvOpeningPriceColumnIndex = inputManager.AskUser<int>("Type the index number of the opening column for the CSV file (3).") - 1;

                parameters = new Parameters(csvFilePath, buyStocksDate, sellStocksDate, buyStocksBudget, buyStocksDay, buyStocksDayDelay, csvCultureInfo, csvValueSeparator, csvDateColumnColumnIndex, csvClosingPriceColumnIndex, csvOpeningPriceColumnIndex);
                parametersFilePath = CreateParametersFile(parameters);
            }
            else
            {
                try
                {
                    parameters = ReadParametersFile(parametersFilePath);
                } catch (Exception ex)
                {
                    Console.WriteLine("Could not load the parameters file " + parametersFilePath + ", the file will be ignored, please restart the program.");
                    RemoveValueFromAppConfig("configFilePath");
                    throw ex;
                }
            }

            return parameters;
            
        }

        private Parameters ReadParametersFile(string parametersFilePath)
        {
            Console.WriteLine("Reading parameters from " + parametersFilePath);
            Parameters parameters;
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(parametersFilePath, FileMode.Open, FileAccess.Read))
            {
                parameters = (Parameters)formatter.Deserialize(stream);
            }
            return parameters;
        }

        private string CreateParametersFile(Parameters parameters)
        {
            bool validConfigFilePath = false;
            string parametersFilePath;
            do
            {
                Console.WriteLine("No parameters file found, type the directory for the new parameters file.");
                parametersFilePath = Console.ReadLine();
                Console.Clear();
                if (Directory.Exists(parametersFilePath))
                {
                    validConfigFilePath = true;
                    parametersFilePath = Path.Combine(parametersFilePath, "ProfitCalculator.params");
                } else
                {
                    Console.WriteLine("That path does not exist! Check and try again.");
                }
            } while (!validConfigFilePath);

            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(parametersFilePath, FileMode.Create, FileAccess.Write))
            {
                formatter.Serialize(stream, parameters);
            }
            WriteValueToAppConfig("configFilePath", parametersFilePath);
            return parametersFilePath;
        }

        private void WriteValueToAppConfig(string key, string value)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings.Add(key, value);
            configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private string ReadValueFromAppConfig(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        static void RemoveValueFromAppConfig(string key)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings.Remove(key);
            configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

    }
}
