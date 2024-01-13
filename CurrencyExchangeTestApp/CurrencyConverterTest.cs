using CurrencyConverterModule;

namespace CurrencyExchangeTestApp
{
    public class CurrencyConverterTest
    {
        private CurrencyConverter currencyConverter = new CurrencyConverter();

        private List<Tuple<string, string, double>> mockData = new List<Tuple<string, string, double>>()
        {
            new Tuple<string, string, double>("USD", "EUR", 0.9132),
            new Tuple<string, string, double>("USD", "GBP", 0.7842),
            new Tuple<string, string, double>("USD", "JPY", 144.93),
            new Tuple<string, string, double>("EUR", "GBP", 0.8588),
            new Tuple<string, string, double>("EUR", "CAD", 1.4681),
            new Tuple<string, string, double>("JPY", "CAD", 0.92510),
            new Tuple<string, string, double>("CAD", "GBP", 0.5850),
            new Tuple<string, string, double>("GBP", "AUD", 1.9072)
        };
        [Fact]
        public async Task ConvertUSDAUD_10_return_14_9562()
        {
            currencyConverter.UpdateConfiguration(mockData);
            var test = await currencyConverter.Convert("USD", "AUD", 10);
            Assert.Equal(14.9562,test,0.0001);
        }
        [Fact]
        public async Task ConvertUSDEUR_10_return_9_132()
        {
            currencyConverter.UpdateConfiguration(mockData);
            var test = await currencyConverter.Convert("USD", "EUR", 10);
            Assert.Equal(9.132, test, 0.0001);
        }
        [Fact]
        public async Task ConvertEURUSD_10_return_10_9505()
        {
            currencyConverter.UpdateConfiguration(mockData);
            var test = await currencyConverter.Convert("EUR", "USD", 10);
            Assert.Equal(10.9505, test, 0.0001);
        }
    }
}