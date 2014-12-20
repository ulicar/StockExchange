using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrugaDomacaZadaca_Burza
{
	public static class Factory
	{
		public static IStockExchange CreateStockExchange()
		{
			return new StockExchange();
		}
	}
	

	public interface IStock{

		//void Stock (string stockName, decimal stockPrice, DateTime timestamp);
		//void setStockPrice (DateTime inTimeStamp, Decimal inStockValue);
		Decimal GetStockPrice (DateTime inTimeStamp); 
		Decimal GetLastStockPrice ();
		Decimal GetInitialStockPrice ();

	}
	


	public class Stock : IStock
	{
		private string stockName;
		private Dictionary<DateTime,Decimal> stockPrice;

		public Stock(string stockName, decimal stockPrice, DateTime timestamp)
		{
			if (stockName == null){
				throw new StockExchangeException("Stock name must exist.");
			}

			if (stockPrice <= 0){
				throw new StockExchangeException("Stock price must positive.");
			}

			this.stockPrice = new Dictionary<DateTime, decimal> ();

			this.stockName = stockName;
			this.stockPrice[roundTimestamp(timestamp)] = stockPrice;
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

			/*if (this.stockPrice.Keys.Min () > inTimeStamp) {
				throw new StockExchangeException("Request from the past");
			}*/

			this.stockPrice[inTimeStamp] = inStockValue;
		}

		public Decimal GetStockPrice(DateTime inTimeStamp) //dohvaća cijenu dionice za neko vrijeme
		{
			Dictionary<DateTime, decimal> stockPricesBeforeSelectedTime = 
				this.stockPrice.Where (x => x.Key <= inTimeStamp).ToDictionary (x => x.Key, x => x.Value);

			if (stockPricesBeforeSelectedTime.Count == 0) {
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




	abstract public class StockIndex
	{
		protected List<Stock> stocks;
		protected string indexName;
		protected IndexTypes indexType;


		public StockIndex(string inIndexName){
			this.indexName = inIndexName;

			this.stocks = new List<Stock> ();
		}

		public string getStockIndexName(){
			return indexName;
		}

		private Stock getStockFromIndex(string inStockName){

			foreach (Stock stock in this.stocks) {
				if (stock.getStockName() == inStockName) {
					return stock;
				}
			}
			throw new StockExchangeException ("Stock doesn't exit");
		}

		public void AddStockToIndex(Stock inStock){
			// TODO:

			this.stocks.Add (inStock);
		}

		public void RemoveStockFromIndex(string inStockName) //briše dionicu iz indeksa
		{
			this.stocks.Remove (getStockFromIndex (inStockName));
		}

		public bool IsStockPartOfIndex(string inStockName){
			try {
				getStockFromIndex(inStockName);
				return true;
			} catch (StockExchangeException){
				return false;
			}

		}

		public int NumberOfStocksInIndex(){
			return this.stocks.Count ();
		}

		abstract public decimal GetIndexValue (DateTime inTimeStamp, Dictionary <Stock, long> stockInStockExchange);
	}

	public class WeightedStockIndex : StockIndex
	{

		public WeightedStockIndex (string inIndexName) : base(inIndexName) {
			this.indexType = IndexTypes.WEIGHTED;
		}

		public override decimal GetIndexValue(DateTime inTimeStamp, Dictionary <Stock, long> stockInStockExchange){
			decimal totalIndexValue = 0;
			decimal stockShareValue = 0;
			decimal numberOfStockSharesInStockExchange = 0;
			decimal valueOfStockExchange = 0;
			decimal stockPrice = 0;

			if (this.NumberOfStocksInIndex() == 0){
				return totalIndexValue;
			}


			foreach (Stock stock in stockInStockExchange.Keys) {
				numberOfStockSharesInStockExchange = Convert.ToDecimal (stockInStockExchange [stock]);
				stockPrice = stock.GetStockPrice (inTimeStamp);

				valueOfStockExchange += stockPrice * numberOfStockSharesInStockExchange;
			}


			foreach (Stock stock in this.stocks) {
				numberOfStockSharesInStockExchange = Convert.ToDecimal (stockInStockExchange [stock]);
				stockPrice = stock.GetStockPrice (inTimeStamp);

				stockShareValue = stockPrice * numberOfStockSharesInStockExchange;
				totalIndexValue += (stockPrice * weightOfStock(stockShareValue, valueOfStockExchange));
			}

			return Decimal.Round(totalIndexValue, 3);
		}

		private decimal weightOfStock(decimal stockShareValue, decimal valueOfStockExchange){
			decimal weight = 0;

			weight = stockShareValue / valueOfStockExchange;

			return weight;
		}
	}

	public class AverageStockIndex: StockIndex
	{

		public AverageStockIndex (string inIndexName): base(inIndexName){
			this.indexType = IndexTypes.AVERAGE;
		}

		public override decimal GetIndexValue(DateTime inTimestamp, Dictionary <Stock, long> stocksInStockExchange){
			decimal totalIndexValue = 0;
			decimal numberOfStocks = Convert.ToDecimal(this.NumberOfStocksInIndex ());

			if (this.NumberOfStocksInIndex() == 0){
				return totalIndexValue;
			}
			
			foreach (Stock stock in this.stocks) {
				totalIndexValue += stock.GetStockPrice (inTimestamp);
			}

			return Decimal.Round(totalIndexValue / numberOfStocks, 3) ; 
		}
	}
	


	public class Portfolio
	{
		string portfolioId;
		Dictionary<Stock, Int32> stocks;

		public Portfolio(string inPortfolioID){
			if (inPortfolioID == "\0") {
				throw new StockExchangeException ("Portforlio must have a name");
			}

			this.stocks = new Dictionary<Stock, int> ();
			this.portfolioId = inPortfolioID;

		} //stvara novi portfelj na burzi

		public string getPortfolioName (){
			return portfolioId;
		}

		private Stock getStockFromPortfolio(string inStockName){
			foreach (Stock stock in this.stocks.Keys) {
				if (stock.getStockName () == inStockName)
					return stock;
			}
			throw new StockExchangeException ("Stock doesn't exist in Portfolio");
		}

		public void AddStockToPortfolio(Stock stock, int numberOfShares){
			if (numberOfShares <= 0) {
				throw new StockExchangeException ("Number of shares must be grather than 0");
			}
			// assert stock?? TODO

			if (this.IsStockPartOfPortfolio(stock.getStockName())){
				this.stocks[stock] += numberOfShares;
			} else {
				this.stocks.Add (stock, numberOfShares);
			}

		}
		public void RemoveStockFromPortfolio(string inStockName, int numberOfShares){
			if (this.IsStockPartOfPortfolio (inStockName) == false) {}

			if (this.NumberOfSharesOfStockInPortfolio (inStockName) < numberOfShares) {
				throw new StockExchangeException ("Not enough shares.");

			} else if (this.NumberOfSharesOfStockInPortfolio (inStockName) == numberOfShares) {
				this.RemoveStockFromPortfolio (inStockName);

			} else {
				Stock stock = getStockFromPortfolio (inStockName);
				this.stocks [stock] -= numberOfShares;
			}
		}

		public void RemoveStockFromPortfolio(string inStockName){
			Stock stock = getStockFromPortfolio (inStockName);
			this.stocks.Remove (stock);
		}


		public bool IsStockPartOfPortfolio(string inStockName){
			try {
				getStockFromPortfolio(inStockName);
				return true;

			} catch (StockExchangeException){
				return false;
			}

		} //provjerava nalazi li se dionica u portfelju

		public int NumberOfStocksInPorfolio(){
			return this.stocks.Count;
		}


		public int NumberOfSharesOfStockInPortfolio(string inStockName){
			int numberOfStocksInPorfolio = 0;

			if (this.IsStockPartOfPortfolio (inStockName)) {
				Stock stock = this.getStockFromPortfolio (inStockName);

				numberOfStocksInPorfolio = Convert.ToInt32 (this.stocks [stock]);
			}

			return numberOfStocksInPorfolio;

		}//dohvaća broj dionice u traženom portfelj

		public Decimal GetPortfolioValue(DateTime timeStamp){
			decimal portfolioValue = 0;
			foreach (Stock stock in this.stocks.Keys) {
				portfolioValue += (this.NumberOfSharesOfStockInPortfolio (stock.getStockName()) 
				                   	* stock.GetStockPrice (timeStamp));
			}

			return portfolioValue;
		}//dohvaća vrijednost portfelja u određenom trenutku

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
	

	public class StockExchange : IStockExchange
	{
		private List<StockIndex> stockIndexes;
		private List<Portfolio> portfolios;
		private Dictionary<Stock, long> stocks;

		public StockExchange (){
			this.stockIndexes = new List<StockIndex> ();
			this.stocks = new Dictionary<Stock, long> ();
			this.portfolios = new List<Portfolio> ();
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
			if (StockExists(inStockName)) {
				throw new StockExchangeException ("Stock already in the StockExchange");
			}

			if (inNumberOfShares <= 0) {
				throw new StockExchangeException ("Number of shares must be positiv.");
			}

			Stock newStock = new Stock (inStockName, inInitialPrice, inTimeStamp);
			this.stocks.Add (newStock, inNumberOfShares); // TODO: must be >0
		}

		public void DelistStock(string inStockName)
		{
			inStockName = inStockName.ToUpper ();

			foreach (StockIndex stockIndex in this.stockIndexes) {
				if (stockIndex.IsStockPartOfIndex (inStockName)) {
					stockIndex.RemoveStockFromIndex (inStockName);
				}
			}

			foreach (Portfolio portfolio in this.portfolios) {
				if (portfolio.IsStockPartOfPortfolio (inStockName)) {
					portfolio.RemoveStockFromPortfolio (inStockName);
				}
			}

			this.stocks.Remove (getStockFromStockExchange (inStockName) );
		}

		public bool StockExists(string inStockName)
		{
			inStockName = inStockName.ToUpper ();

			foreach (Stock stock in this.stocks.Keys) {
				if (stock.getStockName() == inStockName) {
					return true;
				}
			}

			return false;
		}

		private Stock getStockFromStockExchange(string inStockName){
			foreach (Stock stock in this.stocks.Keys) {
				if (stock.getStockName() == inStockName) {
					return stock;
				}
			}
			throw new StockExchangeException ("Stock doesn't exit");
		}

		private StockIndex getStockIndexFromStockExchange(string inStockIndexName){
			foreach (StockIndex stockIndex in this.stockIndexes) {
				if (stockIndex.getStockIndexName() == inStockIndexName) {
					return stockIndex;
				}
			}
			throw new StockExchangeException ("StockIndex doesn't exit");
		}

		private Portfolio getPortfolioFromStockExchange(string inPortfolioName){
			foreach (Portfolio portfolio in this.portfolios) {
				if (portfolio.getPortfolioName() == inPortfolioName) {
					return portfolio;
				}
			}
			throw new StockExchangeException ("Portfolio doesn't exit");
		}

		public int NumberOfStocks()
		{
			return this.stocks.Count ();
		}

		public int NumberOfSharesOfStocksInStockExchange(string inStockName){
			inStockName = inStockName.ToUpper ();

			Stock stock = getStockFromStockExchange (inStockName);
			
			return Convert.ToInt32(this.stocks [stock]);
		}

		public void SetStockPrice(string inStockName, DateTime inTimeStamp, decimal inStockValue)
		{
			inStockName = inStockName.ToUpper ();
			Stock stock = getStockFromStockExchange (inStockName);
			stock.SetStockPrice (inTimeStamp, inStockValue);
		}

		public decimal GetStockPrice(string inStockName, DateTime inTimeStamp)
		{
			inStockName = inStockName.ToUpper ();
			Stock stock = getStockFromStockExchange (inStockName);
			return stock.GetStockPrice(inTimeStamp) ;
		}

		public decimal GetInitialStockPrice(string inStockName)
		{
			inStockName = inStockName.ToUpper ();

			Stock stock = getStockFromStockExchange (inStockName);
			return stock.GetInitialStockPrice ();
		}

		public decimal GetLastStockPrice(string inStockName)
		{
			inStockName = inStockName.ToUpper (); // TODO create a function

			Stock stock = getStockFromStockExchange (inStockName);
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

			this.stockIndexes.Add (stockIndex);
		}

		public void AddStockToIndex(string inIndexName, string inStockName)
		{
			StockIndex stockIndex = getStockIndexFromStockExchange (inIndexName);
			Stock stock = getStockFromStockExchange (inStockName);

			if (stockIndex.IsStockPartOfIndex (inStockName)) {
				throw new StockExchangeException ("Stock is already in Index.");
			}

			stockIndex.AddStockToIndex (stock);
		}

		public void RemoveStockFromIndex(string inIndexName, string inStockName)
		{
			StockIndex stockIndex = getStockIndexFromStockExchange (inIndexName);
			stockIndex.RemoveStockFromIndex (inStockName);
		}

		public bool IsStockPartOfIndex(string inIndexName, string inStockName)
		{
			StockIndex stockIndex = getStockIndexFromStockExchange (inIndexName);
			return stockIndex.IsStockPartOfIndex (inStockName);

		}

		public decimal GetIndexValue(string inIndexName, DateTime inTimeStamp)
		{
			StockIndex stockIndex = getStockIndexFromStockExchange (inIndexName);
			return stockIndex.GetIndexValue (inTimeStamp, this.stocks);
		}

		public bool IndexExists(string inIndexName)
		{	
			foreach (StockIndex stockIndex in this.stockIndexes) {
				if (stockIndex.getStockIndexName().ToUpper() == inIndexName.ToUpper()) {
					return true;
				}
			}
			return false;
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
			this.portfolios.Add (portfolio);
		}

		public void AddStockToPortfolio(string inPortfolioID, string inStockName, int numberOfShares)
		{
			Portfolio portfolio = this.getPortfolioFromStockExchange (inPortfolioID);
			Stock stock = getStockFromStockExchange (inStockName);

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