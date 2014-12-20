public class StockExchange : IStockExchange
{
	private Dictionary<string, Stock> stocks;
	private Dictionary<string, int> soldStocks;
	private Dictionary<string, StockIndex> stockIndexes;
	private Dictionary<string, Portfolio> portfolios;


	public StockExchange (){
		this.stockIndexes = new Dictionary<string, StockIndex> ();
		this.stocks = new Dictionary<string, Stock> ();
		this.soldStocks = new Dictionary<string, int> ();
		this.portfolios = new Dictionary<string, Portfolio> ();
	}



public string unifyName(string name){
	return name.ToUpper ();
}

private DateTime roundTimestamp(DateTime timestamp){
	int ticksInMillisecond = 10000;

	return DateTime( timestamp.Ticks / ticksInMillisecond * ticksInMillisecond );
}

public void ListStock(string inStockName, long inNumberOfShares, decimal inInitialPrice, DateTime inTimeStamp)
{
	string unifiedName = unifyName (inStockName);
	
	if (StockExists(unifiedName)) {
		throw new StockExchangeException ("Stock already in the StockExchange");
	}
	
	Stock newStock = new Stock (unifiedName, inNumberOfShares, inInitialPrice, roundTimestamp(inTimeStamp));
	this.stocks.Add (unifiedName, newStock);
	this.soldStocks.Add (unifiedName, 0);
}

public void DelistStock(string inStockName)
{
	string unifiedName = unifyName (inStockName);

	if (StockExists (inStockName) == false) {
		throw new StockExchangeException ("Stock not in the StockExchange");
	}

	foreach (var stockIndex in this.stockIndexes) {
		if (stockIndex.Value.IsStockPartOfIndex (unifiedName)) {
			stockIndex.Value.RemoveStockFromIndex (unifiedName);
		}
	}

	foreach (var portfolio in this.portfolios) {
		if (portfolio.Value.IsStockPartOfPortfolio (unifiedName)) {
			portfolio.Value.RemoveStockFromPortfolio (unifiedName);
		}
	}

	this.stocks.Remove (unifiedName);
	this.stocks.Remove (unifiedName);
	
}

public bool StockExists(string inStockName)
{
	string unifiedName = unifyName (inStockName);

	return this.stocks.ContainsKey (unifiedName);
}

private Stock getStockFromStockExchange(string inStockName){
		Stock stock;
		string unifiedName = unifyName (inStockName);

		if (StockExists (unifiedName) == false) {
			throw new StockExchangeException ("Stock doesn't exits");
		}

		return this.stocks[inStockName];
}

private StockIndex getStockIndexFromStockExchange(string inStockIndexName){
		return this.stockIndexes[inStockIndexName];
}

private Portfolio getPortfolioFromStockExchange(string inPortfolioName){
		return this.portfolios[inPortfolioName];

}

public int NumberOfStocks()
{
	return this.stocks.Count ();
}

public int NumberOfSharesOfStocksInStockExchange(string inStockName){
	string unifiedName = unifyName(inStockName);
	
		Stock stock = getStockFromStockExchange(unifiedName);
		return stock.getStockQuantity ();
	}

public void SetStockPrice(string inStockName, DateTime inTimeStamp, decimal inStockValue)
{
	string unifiedName = unifyName(inStockName);
	DateTime roundedTS = roundTimestamp (inTimeStamp);

	Stock stock = getStockFromStockExchange (unifiedName);
	stock.SetStockPrice (roundedTS, inStockValue);

}

public decimal GetStockPrice(string inStockName, DateTime inTimeStamp)
{	
	string unifiedName = unifyName(inStockName);
	DateTime roundedTS = roundTimestamp (inTimeStamp);

	Stock stock = getStockFromStockExchange(unifiedName);
	return stock.GetStockPrice (roundedTS);


}

public decimal GetInitialStockPrice(string inStockName)
{
	string unifiedName = unifyName(inStockName);

	Stock stock = getStockFromStockExchange(unifiedName);
	return stock.GetInitialStockPrice ();
	
}

public decimal GetLastStockPrice(string inStockName)
{
	string unifiedName = unifyName(inStockName);

	Stock stock = getStockFromStockExchange(unifiedName);
	
	return stock.GetLastStockPrice ();

}

public void CreateIndex(string inIndexName, IndexTypes inIndexType)
{
	StockIndex stockIndex;

	if (IndexExists (inIndexName)) {
		throw new StockExchangeException ("Index with that name already exist!");
	}

	switch (inIndexType) {
		case (IndexTypes.AVERAGE):
			stockIndex = new AverageStockIndex (inIndexName);
			break;

		case (IndexTypes.WEIGHTED):
			stockIndex = new WeightedStockIndex (inIndexName);
			break;

		default:
			throw new StockExchangeException ("Index Type doeesn't exist!");

	}

	this.stockIndexes.Add (inIndexName, stockIndex);
}

public void AddStockToIndex(string inIndexName, string inStockName)
{
	string unifiedName = unifyName (inStockName);
	StockIndex stockIndex;
	Stock stock;


	stock = getStockFromStockExchange (unifiedName);
	stockIndex = getStockIndexFromStockExchange (inIndexName);
	
	stockIndex.AddStockToIndex (stock);
}

public void RemoveStockFromIndex(string inIndexName, string inStockName)
{
	StockIndex stockIndex;
		string unifiedName = unifyName (inStockName);
	
	stockIndex = getStockIndexFromStockExchange (inIndexName);


	stockIndex.RemoveStockFromIndex (unifiedName);
}

public bool IsStockPartOfIndex(string inIndexName, string inStockName)
{
	StockIndex stockIndex = getStockIndexFromStockExchange (inIndexName);
	return stockIndex.IsStockPartOfIndex (inStockName);

}

public decimal GetIndexValue(string inIndexName, DateTime inTimeStamp)
{
	DateTime roundedTS = roundTimestamp (inTimeStamp);
	StockIndex stockIndex = getStockIndexFromStockExchange (inIndexName);
	
	return stockIndex.GetIndexValue (roundedTS, getStockExchangeValue(roundedTS));
}

public bool IndexExists(string inIndexName)
{	
		return this.stockIndexes.ContainsKey (inIndexName);
}

public int NumberOfIndices()
{
	return this.stockIndexes.Count ();
}

public int NumberOfStocksInIndex(string inIndexName)
{
	StockIndex stockIndex = getStockIndexFromStockExchange (inIndexName);
	return stockIndex.NumberOfStocksInIndex ();
}

public void CreatePortfolio(string inPortfolioID)
{
	if (this.PortfolioExists (inPortfolioID)) {
		throw new StockExchangeException ("Portfolio exists!");
	}

	Portfolio portfolio = new Portfolio (inPortfolioID);
	this.portfolios.Add (inPortfolioID, portfolio);
}

public void AddStockToPortfolio(string inPortfolioID, string inStockName, int numberOfShares)
{
	Portfolio portfolio = this.getPortfolioFromStockExchange (inPortfolioID);
	Stock stock = getStockFromStockExchange (inStockName);

	#TODO: stao ovdje
	int sharesToAdd = numberOfShares;
	if (portfolio.NumberOfSharesOfStockInPortfolio (inStockName) + numberOfShares > Convert.ToInt32 (this.stocks [stock])) {
		sharesToAdd = Convert.ToInt32(this.stocks[stock]) - portfolio.NumberOfSharesOfStockInPortfolio (inStockName);
	}

	portfolio.AddStockToPortfolio (stock, sharesToAdd);
}

public void RemoveStockFromPortfolio(string inPortfolioID, string inStockName, int numberOfShares)
{
	Portfolio portfolio = this.getPortfolioFromStockExchange (inPortfolioID);

	portfolio.RemoveStockFromPortfolio (inStockName, numberOfShares);
}

public void RemoveStockFromPortfolio(string inPortfolioID, string inStockName)
{
	Portfolio portfolio = this.getPortfolioFromStockExchange (inPortfolioID);

	portfolio.RemoveStockFromPortfolio (inStockName);
}

public int NumberOfPortfolios()
{
	return this.portfolios.Count;
}

public int NumberOfStocksInPortfolio(string inPortfolioID)
{
	Portfolio portfolio = this.getPortfolioFromStockExchange (inPortfolioID);
	return portfolio.NumberOfStocksInPorfolio();
}

public bool PortfolioExists(string inPortfolioID)
{
	try {
		getPortfolioFromStockExchange(inPortfolioID);
		return true;

	} catch (StockExchangeException) {
		return false;
	}
}

public bool IsStockPartOfPortfolio(string inPortfolioID, string inStockName)
{
	Portfolio portfolio = this.getPortfolioFromStockExchange (inPortfolioID);
	return portfolio.IsStockPartOfPortfolio (inStockName);
}

public int NumberOfSharesOfStockInPortfolio(string inPortfolioID, string inStockName)
{
	Portfolio portfolio = this.getPortfolioFromStockExchange (inPortfolioID);
	return portfolio.NumberOfSharesOfStockInPortfolio (inStockName);
}

public decimal GetPortfolioValue(string inPortfolioID, DateTime timeStamp)
{
	Portfolio portfolio = this.getPortfolioFromStockExchange (inPortfolioID);
	return portfolio.GetPortfolioValue (timeStamp);
}

public decimal GetPortfolioPercentChangeInValueForMonth(string inPortfolioID, int Year, int Month)
{
	Portfolio portfolio = this.getPortfolioFromStockExchange (inPortfolioID);
	return portfolio.GetPortfolioPercentChangeInValueForMonth (Year, Month);
}
}
}
