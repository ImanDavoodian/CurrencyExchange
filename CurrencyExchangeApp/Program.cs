using CurrencyConverterModule;

List<Tuple<string, string, double>> mockData = new List<Tuple<string, string, double>>()
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
CurrencyConverter currencyConverter = new CurrencyConverter();
currencyConverter.UpdateConfiguration(mockData);
var test = await currencyConverter.Convert("USD", "CAD", 10);
Console.WriteLine($"Final Result:{test}");