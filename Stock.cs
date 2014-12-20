public class Stock : IStock
{
	private string stockName;
	private Dictionary<DateTime,Decimal> stockPrice;
	private long quantity;

	public Stock(string stockName, long quantity, decimal stockPrice, DateTime timestamp)
	{
		if (stockName == null){
			throw new StockExchangeException("Stock name must exist.");
		}

		if (stockPrice <= 0){
			throw new StockExchangeException("Stock price must positive.");
		}

		if (quantity <= 0) {
			throw new StockExchangeException("Stock quantity must positive.");
		}

		this.stockPrice = new Dictionary<DateTime, decimal> ();

		this.stockName = stockName;
		this.quantity = quantity;
		this.stockPrice[roundTimestamp(timestamp)] = stockPrice;
	}

	public long getStockQuantity(){
		return this.quantity;
	}

	public string getStockName(){
		return stockName;
	}

	public void SetStockPrice(DateTime inTimeStamp, Decimal inStockValue)
	{
		if (inStockValue <= 0){
			throw new StockExchangeException("Stock price must positive.");
		}

		if (this.stockPrice.ContainsKey(inTimeStamp)){
			throw new StockExchangeException("Already exist value for that time");
		}

		this.stockPrice[inTimeStamp] = inStockValue;
	}

	public Decimal GetStockPrice(DateTime inTimeStamp) //dohvaća cijenu dionice za neko vrijeme
	{
		Dictionary<DateTime, decimal> allStocksPricesBeforeSelectedTime = 
			this.stockPrice.Where (x => x.Key <= inTimeStamp).ToDictionary (x => x.Key, x => x.Value);

		if (allStocksPricesBeforeSelectedTime.Count == 0) {
			throw new StockExchangeException ("Referenced a stock value before the stock was created");
		}

		DateTime timeStampForSelectedTime = stockPricesBeforeSelectedTime.Keys.Max ();

		return this.stockPrice [timeStampForSelectedTime];

	}

	public Decimal GetInitialStockPrice() //dohvaća početnu cijenu dionice
	{
		DateTime minimumKey = this.stockPrice.Keys.Min();
		return this.stockPrice [minimumKey];

	}

	public Decimal GetLastStockPrice() //dohvaća zadnju cijenu dionice
	{
		DateTime maximalKey = this.stockPrice.Keys.Max();
		return this.stockPrice [maximalKey];
	}
}

