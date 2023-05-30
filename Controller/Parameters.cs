using System;
using System.Globalization;

namespace ProfitCalculator
{
    [Serializable]
    public class Parameters
    {
        public string CsvFilePath { get; set; }
        public DateTime BuyStocksDate { get; set; }
        public DateTime SellStocksDate { get; set; }
        public double BuyStocksBudget { get; set; }
        public DayOfWeek BuyStocksDay { get; set; }
        public int BuyStocksDayDelay { get; set; }
        public CultureInfo CsvCultureInfo { get; set; }
        public char CsvValueSeparator { get; set; }
        public int CsvDateColumnColumnIndex { get; set; }
        public int CsvClosingPriceColumnIndex { get; set; }
        public int CsvOpeningPriceColumnIndex { get; set; }

        public Parameters(string csvFilePath, DateTime buyStocksDate, DateTime sellStocksDate, double buyStocksBudget, DayOfWeek buyStocksDay, int buyStocksDayDelay, CultureInfo csvCultureInfo, char csvValueSeparator, int csvDateColumnColumnIndex, int csvClosingPriceColumnIndex, int csvOpeningPriceColumnIndex)
        {
            CsvFilePath = csvFilePath;
            BuyStocksDate = buyStocksDate;
            SellStocksDate = sellStocksDate;
            BuyStocksBudget = buyStocksBudget;
            BuyStocksDay = buyStocksDay;
            BuyStocksDayDelay = buyStocksDayDelay;
            CsvCultureInfo = csvCultureInfo;
            CsvValueSeparator = csvValueSeparator;
            CsvDateColumnColumnIndex = csvDateColumnColumnIndex;
            CsvClosingPriceColumnIndex = csvClosingPriceColumnIndex;
            CsvOpeningPriceColumnIndex = csvOpeningPriceColumnIndex;
        }

        public Parameters() { }
    }
}
