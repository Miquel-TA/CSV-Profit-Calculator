using System;
using System.Collections.Generic;
using System.Linq;

namespace ProfitCalculator
{
    public class ProfitCalculator
    {
        private DateTime sellStocksDate;
        private double buyStocksBudget;
        private DayOfWeek buyStocksDay;
        private int buyStocksDayDelay;
        private List<DataPoint> dataPoints;
        private DateTime buyStocksDate;
        private DataPoint dataPoint;
        private double stockCount = 0;
        private double totalDeposited = 0;

        public ProfitCalculator(List<DataPoint> dataPoints, DateTime buyStocksDate, DateTime sellStocksDate, double buyStocksBudget, DayOfWeek buyStocksDay, int buyStocksDayDelay)
        {
            this.dataPoints = dataPoints;
            this.sellStocksDate = sellStocksDate;
            this.buyStocksBudget = buyStocksBudget;
            this.buyStocksDay = buyStocksDay;
            this.buyStocksDayDelay = buyStocksDayDelay;

            buyStocksDate = GetDepositDate(buyStocksDate);
        }

        public List<string> CalculateProfitOverTime()
        {
            List<string> outputLogs = new List<string>();

            while (buyStocksDate < sellStocksDate)
            {
                DateTime buyStocksDateWithDelay = buyStocksDate.AddDays(buyStocksDayDelay);

                dataPoint = FindClosestDataPoint(buyStocksDateWithDelay);

                double stocksToBuy = Round(buyStocksBudget / dataPoint.openingPrice);
                stockCount = stockCount + stocksToBuy;
                totalDeposited = totalDeposited + buyStocksBudget;

                if (!buyStocksDateWithDelay.Equals(dataPoint.date))
                {
                    outputLogs.Add(
                        $"{dataPoint.date:dd-MMM-yyyy}:\n" +
                        $"  The day {buyStocksDateWithDelay:dd-MMM-yyyy} ({buyStocksDay}) was not available for deposits.\n" +
                        $"  The deposit was made on {dataPoint.date} ({dataPoint.date.ToString("dddd")}) instead.\n" +
                        $"  Your balance is {(stockCount * dataPoint.openingPrice):0.###}.\n" +
                        $"  Your stock count is {stockCount}.\n" +
                        $"  Today's opening stock value is {dataPoint.openingPrice}.\n" +
                        $"  Today's closing stock value is {dataPoint.closingPrice}.\n" +
                        $"  Your total investment is {totalDeposited}."
                    );
                } 
                else
                {
                    outputLogs.Add(
                        $"{dataPoint.date:dd-MMM-yyyy}:\n" +
                        $"  Your balance is {(stockCount * dataPoint.openingPrice):0.###}.\n" +
                        $"  Your stock count is {stockCount}.\n" +
                        $"  Today's opening stock value is {dataPoint.openingPrice}.\n" +
                        $"  Today's closing stock value is {dataPoint.closingPrice}.\n" +
                        $"  Your total investment is {totalDeposited}."
                    );
                }
                buyStocksDate = new DateTime(buyStocksDate.Year, buyStocksDate.Month, 1).AddMonths(1);
                buyStocksDate = GetDepositDate(buyStocksDate);
            }

            dataPoint = FindClosestDataPoint(sellStocksDate);

            outputLogs.Add(
                $"===================================================\n" +
                $"{dataPoint.date:dd-MMM-yyyy}:\n" +
                $"  Your final balance is {(stockCount * dataPoint.closingPrice):0.###}.\n" +
                $"  Your final stock count is {stockCount}.\n" +
                $"  Today's opening stock value is {dataPoint.openingPrice}.\n" +
                $"  Today's closing stock value is {dataPoint.closingPrice}.\n" +
                $"  Your total investment is {totalDeposited}.\n" +
                $"  You made {(stockCount * dataPoint.closingPrice) - totalDeposited} in profit. \n" +
                $"\nANALYSIS COMPLETED."
            );
            return outputLogs;
        }

        private DateTime GetDepositDate(DateTime datePointer)
        {
            int newPointerYear = datePointer.Year;
            int newPointerMonth = datePointer.Month;

            datePointer = new DateTime(
                newPointerYear,
                newPointerMonth,
                DateTime.DaysInMonth(newPointerYear, newPointerMonth)
                );

            while (datePointer.DayOfWeek != buyStocksDay)
            {
                datePointer = datePointer.AddDays(-1);
            }
            return datePointer;
        }

        private DataPoint FindClosestDataPoint(DateTime dateToSearch)
        {
            return dataPoints.FirstOrDefault(x => x.date >= dateToSearch);
        }

        private double Round(double value)
        {
            return Math.Round(value, 3);
        }

    }
}
