using System;
using System.Collections.Generic;
using System.Linq;

namespace ProfitCalculator
{
    public class ProfitCalculator
    {
        private DateTime analyseUntilDate;
        private double depositQuantity;
        private DayOfWeek depositDay;
        private int depositDayDelay;
        private List<DataPoint> dataPoints;
        private DateTime depositDate;
        private DataPoint closestDataPointToDepositDate;
        private double currentStockValue;
        private double stockCount = 0;
        private double totalDeposited = 0;

        public ProfitCalculator(List<DataPoint> dataPoints, DateTime analyseSinceDate, DateTime analyseUntilDate, double depositQuantity, DayOfWeek depositDay, int depositDayDelay)
        {
            this.analyseUntilDate = analyseUntilDate;
            this.depositQuantity = depositQuantity;
            this.depositDay = depositDay;
            this.dataPoints = dataPoints;
            this.depositDayDelay = depositDayDelay;

            depositDate = GetDepositDate(analyseSinceDate);
        }

        public List<string> CalculateProfitOverTime()
        {
            List<string> outputLogs = new List<string>();

            while (depositDate < analyseUntilDate)
            {
                DateTime depositDateWithDelay = depositDate.AddDays(depositDayDelay);

                closestDataPointToDepositDate = FindClosestDataPoint(depositDateWithDelay);

                currentStockValue = closestDataPointToDepositDate.openingPrice;
                double stocksToBuy = Round(depositQuantity / currentStockValue);
                stockCount = stockCount + stocksToBuy;
                totalDeposited = totalDeposited + depositQuantity;

                if (!depositDateWithDelay.Equals(closestDataPointToDepositDate.date))
                {
                    outputLogs.Add(
                        $"{closestDataPointToDepositDate.date:yyyy-MM-dd}:\n" +
                        $"  The day {depositDateWithDelay:yyyy-MM-dd} ({depositDay}) was not available for deposits.\n" +
                        $"  The deposit was made on {closestDataPointToDepositDate.date} ({closestDataPointToDepositDate.date.ToString("dddd")}) instead.\n" +
                        $"  Your balance is {stockCount * currentStockValue}.\n" +
                        $"  Your stock count is {stockCount}.\n" +
                        $"  Today's stock value is {currentStockValue}.\n" +
                        $"  Your total investment is {totalDeposited}."
                    );
                } 
                else
                {
                    outputLogs.Add(
                        $"{closestDataPointToDepositDate.date:yyyy-MM-dd}:\n" +
                        $"  Your balance is {stockCount * currentStockValue}.\n" +
                        $"  Your stock count is {stockCount}.\n" +
                        $"  Today's stock value is {currentStockValue}.\n" +
                        $"  Your total investment is {totalDeposited}."
                    );
                }
                depositDate = new DateTime(depositDate.Year, depositDate.Month, 1).AddMonths(1);
                depositDate = GetDepositDate(depositDate);
            }

            closestDataPointToDepositDate = FindClosestDataPoint(analyseUntilDate);
            currentStockValue = closestDataPointToDepositDate.closingPrice;

            outputLogs.Add(
                $"===================================================\n" + 
                $"{closestDataPointToDepositDate.date:yyyy-MM-dd}:\n"+
                $"  Your final balance is {stockCount * currentStockValue}.\n" +
                $"  Your stock count is {stockCount}.\n" +
                $"  Today's stock value is {currentStockValue}.\n" +
                $"  Your total investment is {totalDeposited}.\n" +
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

            while (datePointer.DayOfWeek != depositDay)
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
