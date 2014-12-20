abstract public class StockIndex
{
	protected Dictionary<string, Stock> stocks;
	protected string indexName;
	protected IndexTypes indexType;
	
	public StockIndex(string inIndexName){

		if (inIndexName == null){
			throw new StockExchangeException("stockIndex name must exist.");
		}

		this.indexName = inIndexName;
		this.stocks = new Dictionary<string, Stock> ();
	}

	public string getStockIndexName(){
		return indexName;
	}

	private Stock getStockFromIndex(string inStockName){
		return this.stocks [inStockName];
	}

	public void AddStockToIndex(Stock inStock){
		string stockName = inStock.getStockName ();

		if (IsStockPartOfIndex(stockName)){
			throw new StockExchangeException ("Stock already in Index!");
		}

		this.stocks.Add (stockName, inStock);
	}

	public void RemoveStockFromIndex(string inStockName) //briše dionicu iz indeksa
	{
		if (IsStockPartOfIndex (inStockName)) {
			this.stocks.Remove (inStockName);	
		} else {
			throw new StockExchangeException ("Stock not in Index!");
		}
		
	}

	public bool IsStockPartOfIndex(string inStockName){
		return this.stocks.ContainsKey (inStockName);

	}

	public int NumberOfStocksInIndex(){
		return this.stocks.Count ();
	}

	abstract public decimal GetIndexValue (DateTime inTimeStamp, decimal stockExchangeValue);
}

public class WeightedStockIndex : StockIndex
{

	public WeightedStockIndex (string inIndexName) : base(inIndexName) {
		this.indexType = IndexTypes.WEIGHTED;
	}

	public override decimal GetIndexValue(DateTime inTimeStamp, decimal stockExchangeValue){
		decimal totalIndexValue = 0;
		decimal stockShare = 0;
		decimal stockPrice = 0;

		if (this.NumberOfStocksInIndex() == 0){
			return totalIndexValue;
		}

		foreach (var stock in this.stocks) {
			stockPrice = stock.Value.GetStockPrice(inTimeStamp);
		
			stockShare = (stockPrice * stock.Value.getStockQuantity()) / stockExchangeValue;

			totalIndexValue += (stockPrice * stockShare);
		}

		return Decimal.Round(totalIndexValue, 3);
	}
}

public class AverageStockIndex: StockIndex
{

	public AverageStockIndex (string inIndexName): base(inIndexName){
		this.indexType = IndexTypes.AVERAGE;
	}

	public override decimal GetIndexValue(DateTime inTimestamp, decimal stockExchangeValue){
		decimal totalIndexValue = 0;
		decimal numberOfStocks = Convert.ToDecimal(this.NumberOfStocksInIndex ());

		if (this.NumberOfStocksInIndex() == 0){
			return totalIndexValue;
		}

		foreach (var stock in this.stocks) {
			totalIndexValue += stock.Value.GetStockPrice (inTimestamp);
		}

		return Decimal.Round(totalIndexValue / numberOfStocks, 3) ; 
	}
}
