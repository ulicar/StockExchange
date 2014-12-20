public class Portfolio
{
	private string portfolioId;
	private Dictionary<string, Stock> stocksInPortfolio;
	private Dictionary<string, int> stockShares;

	public Portfolio(string inPortfolioID){
		if (inPortfolioID == null) {
			throw new StockExchangeException ("Portforlio must have a name");
		}

		this.stockShares = new Dictionary<string, int> ();
		this.stocksInPortfolio = new Dictionary<string, Stock> ();
		this.portfolioId = inPortfolioID;

	} //stvara novi portfelj na burzi

	public string getPortfolioName (){
		return portfolioId;
	}

	private Stock getStockFromPortfolio(string inStockName){
		return stocksInPortfolio [inStockName];
	}

	public void AddStockToPortfolio(Stock stock, int numberOfShares){
		string stockName = stock.getStockName ();

		if (numberOfShares <= 0) {
			throw new StockExchangeException ("Number of shares must be grather than 0");
		}

		if (this.IsStockPartOfPortfolio(stockName)){
			this.stockShares [stockName] += numberOfShares;

		} else {
			this.stocksInPortfolio.Add (stockName, stock);
			this.stockShares.Add (stockName, numberOfShares);

		}

	}
	public void RemoveStockFromPortfolio(string inStockName, int numberOfShares){
		if (this.IsStockPartOfPortfolio (inStockName) == false) {
			throw new StockExchangeException ("Stock not in Portfolio!");
		}

		if (this.NumberOfSharesOfStockInPortfolio (inStockName) < numberOfShares) {
			throw new StockExchangeException ("Not enough shares.");

		} else if (this.NumberOfSharesOfStockInPortfolio (inStockName) == numberOfShares) {
			this.RemoveStockFromPortfolio (inStockName);

		} else {
			this.stockShares [inStockName] -= numberOfShares;
		}
	}

	public void RemoveStockFromPortfolio(string inStockName){
		this.stockShares.Remove (inStockName);
		this.stocksInPortfolio.Remove (inStockName);
	}


	public bool IsStockPartOfPortfolio(string inStockName){
		return this.stocksInPortfolio.ContainsKey (inStockName);

	} //provjerava nalazi li se dionica u portfelju

	public int NumberOfStocksInPorfolio(){
		return this.stocksInPortfolio.Count ();
	}


	public int NumberOfSharesOfStockInPortfolio(string inStockName){
		return this.stockShares [inStockName];


	}//dohvaća broj dionice u traženom portfelj

	public Decimal GetPortfolioValue(DateTime timeStamp){
		decimal portfolioValue = 0;
		foreach (var stock in this.stockShares.Keys) {
			portfolioValue += (this.NumberOfSharesOfStockInPortfolio (stock.Key) 
			                   * stock.Key.GetStockPrice (timeStamp));
		}

		return portfolioValue;
	}//dohvaća vrijednost portfelja u određenom trenutku TODO: sto ako neka dionica nije postojala tada (treutno baca Exception)

	public Decimal GetPortfolioPercentChangeInValueForMonth(int Year, int Month){
		DateTime firstDayOfCurrentMonth;
		DateTime firstDayOfNextMonth; // TODO create a function that handles date creation and checking

		try {
			firstDayOfCurrentMonth = new DateTime (Year, Month, 1);
			firstDayOfNextMonth = firstDayOfCurrentMonth.AddMonths (1);

		} catch (Exception e){
			throw new StockExchangeException ("Improper date");
		}


		decimal startingValue = this.GetPortfolioValue (firstDayOfCurrentMonth);
		decimal endValue = this.GetPortfolioValue (firstDayOfNextMonth);
		decimal valueChange = 0;

		if (Math.Abs(startingValue) > Convert.ToDecimal(Math.Pow(1, -7))) { // startingValue > 0
			valueChange = ((endValue - startingValue) / startingValue);
		}

		return valueChange * 100;

	} //dohvaća mjeseću promjenu vrijednosti portfelja
}

