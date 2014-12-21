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
	/// <summary>
	/// Stock.
	/// </summary>
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
			this.stockPrice[timestamp] = stockPrice;
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

			DateTime timeStampForSelectedTime = allStocksPricesBeforeSelectedTime.Keys.Max ();

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


	/// <summary>
	/// Stock index.
	/// </summary>
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


	/// <summary>
	/// Weighted stock index.
	/// </summary>
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


	/// <summary>
	/// Average stock index.
	/// </summary>
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

	/// <summary>
	/// Portfolio.
	/// </summary>
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
			
			if (this.IsStockPartOfPortfolio (inStockName) == false) {
				throw new StockExchangeException ("Stock not in Portfolio!");
			}

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
			int stockShares = 0;
			if (IsStockPartOfPortfolio(inStockName) == false) {
				return stockShares;
			}
			return this.stockShares[inStockName];


		}//dohvaća broj dionice u traženom portfelj

		public Decimal GetPortfolioValue(DateTime timeStamp){
			decimal portfolioValue = 0;
			foreach (var stock in this.stocksInPortfolio) {
				portfolioValue += (this.NumberOfSharesOfStockInPortfolio (stock.Key) 
				                   * stock.Value.GetStockPrice (timeStamp));
			}

			return portfolioValue;
		}//dohvaća vrijednost portfelja u određenom trenutku TODO: sto ako neka dionica nije postojala tada (treutno baca Exception)

		public Decimal GetPortfolioPercentChangeInValueForMonth(int Year, int Month){
			DateTime firstDayOfCurrentMonth;
			DateTime firstDayOfNextMonth; // TODO create a function that handles date creation and checking

			try {
				firstDayOfCurrentMonth = new DateTime (Year, Month, 1);
				firstDayOfNextMonth = firstDayOfCurrentMonth.AddMonths (1);

			} catch (Exception){
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





	/// <summary>
	/// 
	/// 
	/// </summary>
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
			DateTime roundedTS = new DateTime( timestamp.Ticks / ticksInMillisecond * ticksInMillisecond );

			return roundedTS;
		}

		private decimal getStockExchangeValue(DateTime timestamp){
			decimal totalValue = 0;
			DateTime roundedTimestamp = roundTimestamp (timestamp);

			foreach (var stock in this.stocks.Values) {
				totalValue += stock.GetStockPrice(roundedTimestamp) * stock.getStockQuantity();
			}

			return totalValue;
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
			string unifiedName = unifyName (inStockName);

			if (StockExists (unifiedName) == false) {
				throw new StockExchangeException ("Stock doesn't exits");
			}

			return this.stocks[unifiedName];
		}

		private StockIndex getStockIndexFromStockExchange(string inStockIndexName){
			if (IndexExists (inStockIndexName) == false) {
				throw new StockExchangeException ("StockIndex doesn't exits");
			}

			return this.stockIndexes[inStockIndexName];
		}

		private Portfolio getPortfolioFromStockExchange(string inPortfolioName){
			if (PortfolioExists (inPortfolioName) == false) {
				throw new StockExchangeException ("Portfolio doesn't exits");
			}

			return this.portfolios[inPortfolioName];

		}

		public int NumberOfStocks()
		{
			return this.stocks.Count ();
		}

		public int NumberOfSharesOfStocksInStockExchange(string inStockName){
			string unifiedName = unifyName(inStockName);

			Stock stock = getStockFromStockExchange(unifiedName);
			return Convert.ToInt32(stock.getStockQuantity ());
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
			string unifiedName = unifyName (inIndexName);

			if (IndexExists (unifiedName)) {
				throw new StockExchangeException ("Index with that name already exist!");
			}

			switch (inIndexType) {
				case (IndexTypes.AVERAGE):
				stockIndex = new AverageStockIndex (unifiedName);
				break;

				case (IndexTypes.WEIGHTED):
				stockIndex = new WeightedStockIndex (unifiedName);
				break;

				default:
				throw new StockExchangeException ("Index Type doeesn't exist!");

			}

			this.stockIndexes.Add (unifiedName, stockIndex);
		}

		public void AddStockToIndex(string inIndexName, string inStockName)
		{
			string unifiedName = unifyName (inStockName);
			string unifiedIndexName = unifyName (inIndexName);
			StockIndex stockIndex;
			Stock stock;


			stock = getStockFromStockExchange (unifiedName);
			stockIndex = getStockIndexFromStockExchange (unifiedIndexName);

			stockIndex.AddStockToIndex (stock);
		}

		public void RemoveStockFromIndex(string inIndexName, string inStockName)
		{
			StockIndex stockIndex;
			string unifiedIndexName = unifyName (inIndexName);
			string unifiedName = unifyName (inStockName);

			stockIndex = getStockIndexFromStockExchange (unifiedIndexName);


			stockIndex.RemoveStockFromIndex (unifiedName);
		}

		public bool IsStockPartOfIndex(string inIndexName, string inStockName)
		{
			string unifiedName = unifyName (inStockName);
			string unifiedIndexName = unifyName (inIndexName);

			StockIndex stockIndex = getStockIndexFromStockExchange (unifiedIndexName);
			return stockIndex.IsStockPartOfIndex (unifiedName);

		}

		public decimal GetIndexValue(string inIndexName, DateTime inTimeStamp)
		{
			DateTime roundedTS = roundTimestamp (inTimeStamp);
			string unifiedIndexName = unifyName (inIndexName);
			StockIndex stockIndex = getStockIndexFromStockExchange (unifiedIndexName);

			return stockIndex.GetIndexValue (roundedTS, getStockExchangeValue(roundedTS));
		}

		public bool IndexExists(string inIndexName)
		{	
			string unifiedIndexName = unifyName (inIndexName);
			return this.stockIndexes.ContainsKey (unifiedIndexName);
		}

		public int NumberOfIndices()
		{
			return this.stockIndexes.Count ();
		}

		public int NumberOfStocksInIndex(string inIndexName)
		{
			string unifiedIndexName = unifyName (inIndexName);
			StockIndex stockIndex = getStockIndexFromStockExchange (unifiedIndexName);
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
			string unifiedName = unifyName (inStockName);
			Stock stock = getStockFromStockExchange (unifiedName);
			int stocksSharesSold = 0;

			if (this.soldStocks.ContainsKey (unifiedName)) {
				stocksSharesSold = this.soldStocks [unifiedName];
			} else {
				this.soldStocks.Add (unifiedName, stocksSharesSold); 
			}

			if (stocksSharesSold + numberOfShares  > Convert.ToInt32(stock.getStockQuantity()) ){
				throw new StockExchangeException("Not enough stocks");
			}

			portfolio.AddStockToPortfolio(stock, numberOfShares);
			this.soldStocks [unifiedName] += numberOfShares;
		}

		public void RemoveStockFromPortfolio(string inPortfolioID, string inStockName, int numberOfShares)
		{
			Portfolio portfolio = this.getPortfolioFromStockExchange (inPortfolioID);
			string unifiedName = unifyName (inStockName);

			portfolio.RemoveStockFromPortfolio (unifiedName, numberOfShares);
			this.soldStocks [unifiedName] += numberOfShares;
		}

		public void RemoveStockFromPortfolio(string inPortfolioID, string inStockName)
		{
			Portfolio portfolio = this.getPortfolioFromStockExchange (inPortfolioID);
			string unifiedName = unifyName (inStockName);

			int stocksToRemove = portfolio.NumberOfSharesOfStockInPortfolio (unifiedName);
			portfolio.RemoveStockFromPortfolio (unifiedName);
			this.soldStocks [unifiedName] += stocksToRemove;

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
			return this.portfolios.ContainsKey (inPortfolioID);
		}

		public bool IsStockPartOfPortfolio(string inPortfolioID, string inStockName)
		{
			Portfolio portfolio = this.getPortfolioFromStockExchange (inPortfolioID);
			string unifiedName = unifyName (inStockName);
			return portfolio.IsStockPartOfPortfolio (unifiedName);
		}

		public int NumberOfSharesOfStockInPortfolio(string inPortfolioID, string inStockName)
		{
			Portfolio portfolio = this.getPortfolioFromStockExchange (inPortfolioID);
			string unifiedName = unifyName (inStockName);
			return portfolio.NumberOfSharesOfStockInPortfolio (unifiedName);
		}

		public decimal GetPortfolioValue(string inPortfolioID, DateTime timeStamp)
		{
			Portfolio portfolio = this.getPortfolioFromStockExchange (inPortfolioID);
			DateTime roundedTS = roundTimestamp (timeStamp);

			return portfolio.GetPortfolioValue (roundedTS);
		}

		public decimal GetPortfolioPercentChangeInValueForMonth(string inPortfolioID, int Year, int Month)
		{
			Portfolio portfolio = this.getPortfolioFromStockExchange (inPortfolioID);
			return portfolio.GetPortfolioPercentChangeInValueForMonth (Year, Month);
		}
	}
}
