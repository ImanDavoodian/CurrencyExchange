namespace CurrencyConverterModule;

internal interface ICurrencyConverter
{
    void ClearConfiguration();
    void UpdateConfiguration(IEnumerable<Tuple<string, string, double>> conversionRateList);
    Task<double> Convert(string fromCurrency, string toCurrency, double amount);
}
