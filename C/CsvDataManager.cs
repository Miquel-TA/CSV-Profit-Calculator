using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace ProfitCalculator
{
    public class CsvDataManager
    {

        private string csvFilePath;
        private int csvDateColumnColumnIndex;
        private int csvOpeningPriceColumnIndex;
        private int csvClosingPriceColumnIndex;
        private char csvValueSeparator;
        private CultureInfo csvCultureInfo;

        public CsvDataManager(string csvFilePath, int csvDateColumnColumnIndex, int csvOpeningPriceColumnIndex, int csvClosingPriceColumnIndex, char csvValueSeparator, CultureInfo csvCultureInfo)
        {
            this.csvFilePath = csvFilePath;
            this.csvDateColumnColumnIndex = csvDateColumnColumnIndex;
            this.csvOpeningPriceColumnIndex = csvOpeningPriceColumnIndex;
            this.csvClosingPriceColumnIndex = csvClosingPriceColumnIndex;
            this.csvValueSeparator = csvValueSeparator;
            this.csvCultureInfo = csvCultureInfo;
        }
        
        public DataPoint FindDataPoint(DateTime requestedDate, List<DataPoint> dataPoints)
        {
            return dataPoints.FirstOrDefault(dataPoint => dataPoint.date >= requestedDate);
        }

        public List<DataPoint> GetDataPointsFromCsv()
        {
            var config = new CsvConfiguration(csvCultureInfo)
            {
                Delimiter = csvValueSeparator.ToString(),
                Encoding = Encoding.UTF8
            };
            using (var csvStreamReader = new StreamReader(csvFilePath))
            using (var csvHelperReader = new CsvReader(csvStreamReader, config))
            {
                // Context: We use the Data-Column relationship along with custom mapping to find the correct columns for each value.
                StockDataMap customDataMapping = new StockDataMap(csvDateColumnColumnIndex, csvOpeningPriceColumnIndex, csvClosingPriceColumnIndex);
                csvHelperReader.Context.RegisterClassMap(customDataMapping);

                List<DataPoint> dataPoints = csvHelperReader.GetRecords<DataPoint>().ToList();
                dataPoints = dataPoints.OrderBy(x => x.date).ToList();

                return dataPoints;
            }
        }

        private sealed class StockDataMap : ClassMap<DataPoint>
        {
            public StockDataMap(int csvDateColumnColumnIndex, int csvOpeningPriceColumnIndex, int csvClosingPriceColumnIndex)
            {
                Map(m => m.date).Index(csvDateColumnColumnIndex);
                Map(m => m.openingPrice).Index(csvOpeningPriceColumnIndex);
                Map(m => m.closingPrice).Index(csvClosingPriceColumnIndex);
            }
        }

    }
}
