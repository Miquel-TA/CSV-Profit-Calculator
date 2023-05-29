using System;

namespace ProfitCalculator
{
    public class DataPoint
    {
        public DateTime date { get; set; }
        public double openingPrice {
            get
            {
                return openingPrice;
            }
            set
            {
                if ((value.ToString().Contains(",") || value.ToString().Contains(".")) && value > 1000)
                {
                    openingPrice = value / 1000;
                } else
                {
                    openingPrice = value;
                }
            }
        }
        public double closingPrice { 
            get
            {
                return closingPrice;
            }
            set
            {
                if ((value.ToString().Contains(",") || value.ToString().Contains(".")) && value > 1000)
                {
                    closingPrice = value / 1000;
                }
                else
                {
                    closingPrice = value;
                }
            }
        }
    }
}