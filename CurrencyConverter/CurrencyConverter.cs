namespace CurrencyConverterModule;
public class CurrencyConverter: ICurrencyConverter
{
    const byte round_digit = 5;

    // List of all available currencies for generating graph vertexes
    private static List<string> _currencyVertexes = new List<string>();
    // list of the index of adjusant node against appropriate node index from the main currency lists as the edje list
    private static List<List<int>> _adjusantEdges = new List<List<int>>();
    // List of the appropriate currency exchange rates and its reverse
    private static List<Tuple<string, string, double>> _conversionRates = new List<Tuple<string, string, double>>();
    /// <summary>
    ///  Clearing Currency Graph Data Structure
    /// </summary>
    public void ClearConfiguration()
    {
        _currencyVertexes.Clear();
        _adjusantEdges.Clear();
        _conversionRates.Clear();
    }
    /// <summary>
    /// Filling Mock Data Of Currency Exchanges
    /// </summary>
    /// <param name="conversionRateList">List of appropriate currency exchange rates</param>
    public void UpdateConfiguration(IEnumerable<Tuple<string, string, double>> conversionRateList)
    {
        foreach (var conversionRate in conversionRateList)
        {
            //Add conversion rate to the ConversionRate List if it does not exits
            if (!_conversionRates.Exists(c => c.Item1 == conversionRate.Item1 && c.Item2 == conversionRate.Item2))
                _conversionRates.Add(conversionRate);
            //Add reverse of conversion rate to the ConversionRate List if its reverse version does not exits
            //While reverse version is calcultable, it helps to optimize the performace of algorithm execution
            if (!_conversionRates.Exists(c => c.Item1 == conversionRate.Item2 && c.Item2 == conversionRate.Item1))
                _conversionRates.Add(new Tuple<string, string, double>(conversionRate.Item2, conversionRate.Item1, (Math.Round(1 / conversionRate.Item3, round_digit))));
            // Add source currency to list of vertexes, if does not exists and generate its appropriate adjusant index
            if (!_currencyVertexes.Exists(c => c == conversionRate.Item1))
            {
                _currencyVertexes.Add(conversionRate.Item1);
                _adjusantEdges.Add(new List<int>());
            }
            // Add source currency to list of vertexes, if does not exists and generate its appropriate adjusant index
            if (!_currencyVertexes.Exists(c => c == conversionRate.Item2))
            {
                _currencyVertexes.Add(conversionRate.Item2);
                _adjusantEdges.Add(new List<int>());
            }
            //Finding the source of exchange rate index in vertexes list
            var rightIndex = _currencyVertexes.FindIndex(c => c == conversionRate.Item1);
            //Finding the destination of exchange rate index in certexes list
            var leftIndex = _currencyVertexes.FindIndex(c => c == conversionRate.Item2);

            //Adding source adjusant edge to the edge list
            _adjusantEdges[rightIndex].Add(leftIndex);
            //Adding the reverce destination adjusant edge to the edge list
            _adjusantEdges[leftIndex].Add(rightIndex);
        }
    }

    public async Task<double> Convert(string fromCurrency, string toCurrency, double amount)
    {
        //Finding the index of source Currency in Vertexes list
        var fromIndex = _currencyVertexes.FindIndex(c => c == fromCurrency);
        if (fromIndex < 0)
            return -1;
        //Finding the index of destination Currency in Vertexes list
        var toIndex = _currencyVertexes.FindIndex(c => c == toCurrency);
        if (toIndex < 0)
            return -2;
        //Traversing the currency exchange graph using BFS algorithm
        //The result is Tuple of destination vertex and its amount accounting to the source amount
        var result = await BFS(fromIndex, toIndex, amount);

        if (result.Item1 == 0 && result.Item2 == 0)
            Console.WriteLine($"There is not conversion path for ({fromCurrency} => {toCurrency})");
        else
            Console.WriteLine($"Conversion From {amount}({fromCurrency}) Will Be {result.Item2}({toCurrency})");

        return result.Item2;
    }
    // Breath-First-Search is being used to find to shortest path from source node to destination node
    private async Task<Tuple<int, double>> BFS(int fromIndex, int toIndex, double amount)
    {
        var result = new Tuple<int, double>(0, 0);

        if (fromIndex == toIndex) return result;

        await Task.Run(() =>
        {
            // In order to reduce the amount of times visiting the previsited nodes, visitedNodes List is being used
            bool[] visitedNodes = new bool[_currencyVertexes.Count];
            for (int i = 0; i < _currencyVertexes.Count; i++)
                visitedNodes[i] = false;

            LinkedList<Tuple<int, double>> queue = new LinkedList<Tuple<int, double>>();
            visitedNodes[fromIndex] = true;
            var startNode = new Tuple<int, double>(fromIndex, amount);
            queue.AddLast(startNode);

            while (queue.Any() && result.Item1 == 0)
            {
                startNode = queue.First();
                queue.RemoveFirst();
                List<int> list = _adjusantEdges[startNode.Item1];
                foreach (var val in list)
                {
                    if (!visitedNodes[val])
                    {
                        visitedNodes[val] = true;

                        var startCur = _currencyVertexes[startNode.Item1];
                        var nextCur = _currencyVertexes[val];
                        var currentConversionRate = _conversionRates.FirstOrDefault(c => c.Item1 == startCur && c.Item2 == nextCur);
                        double nextAmount = 0;
                        if (currentConversionRate != null)
                        {
                            nextAmount = Math.Round(startNode.Item2 * currentConversionRate.Item3, round_digit);
                            var nextNode = new Tuple<int, double>(val, nextAmount);
                            if (val != toIndex)
                                queue.AddLast(nextNode);
                            else
                            {
                                result = nextNode;
                                break;
                            }
                        }
                    }
                }
            }
        });
        return result;
    }
}
