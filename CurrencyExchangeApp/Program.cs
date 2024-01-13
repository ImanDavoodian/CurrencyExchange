using CurrencyConverterModule;

List<Tuple<string, string, double>> mockData = new List<Tuple<string, string, double>>()
{
    new Tuple<string, string, double>("USD", "EUR", 1.2),
    new Tuple<string, string, double>("USD", "AWS", 0.84),
    new Tuple<string, string, double>("USD", "JDP", 0.63),
    new Tuple<string, string, double>("EUR", "AWS", 0.47),
    new Tuple<string, string, double>("EUR", "CAD", 0.79),
    new Tuple<string, string, double>("JDP", "CAD", 1.35),
    new Tuple<string, string, double>("CAD", "AWS", 1.36),
    new Tuple<string, string, double>("AWS", "LTC", 1.7)
};
CurrencyConverter currencyConverter = new CurrencyConverter();
currencyConverter.UpdateConfiguration(mockData);
var test = await currencyConverter.Convert("JDP", "LTC", 10);
Console.WriteLine($"Final Result:{test}");