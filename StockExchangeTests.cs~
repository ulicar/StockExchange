using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace DrugaDomacaZadaca_Burza
{
	[TestFixture()]
	public class StockExchangeTests
	{
		private IStockExchange _stockExchange;

		[SetUp]
		public void SetUp()
		{
			_stockExchange = Factory.CreateStockExchange();
		}

		[Test()]
		public void Test_StockExchangeAtTheBeginig()
		{
			Assert.AreEqual(0, _stockExchange.NumberOfStocks());
			Assert.AreEqual(0, _stockExchange.NumberOfIndices());
			Assert.AreEqual(0, _stockExchange.NumberOfPortfolios());
		}

		[Test()]
		public void Test_ListStock_Simple()
		{
			Assert.AreEqual(0, _stockExchange.NumberOfStocks());
			string firstStockName = "IBM";
			_stockExchange.ListStock(firstStockName, 1000000, 10m, DateTime.Now);

			Assert.AreEqual(1, _stockExchange.NumberOfStocks());
			Assert.True(_stockExchange.StockExists(firstStockName));
			Assert.False(_stockExchange.StockExists("Bezveze"));

			string secondStockName = "MSFT";
			_stockExchange.ListStock(secondStockName, 100000, 15m, DateTime.Now);
			Assert.AreEqual(2, _stockExchange.NumberOfStocks());
			Assert.True(_stockExchange.StockExists(secondStockName));
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Test_ListStock_SimpleNoStockName()
		{
			Assert.AreEqual(0, _stockExchange.NumberOfStocks());
			string firstStockName = "";
			_stockExchange.ListStock(firstStockName, 1000000, 10m, DateTime.Now);

		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Test_ListStock_SameNameAlreadyExists()
		{
			_stockExchange.ListStock("IBM", 1000000, 10m, DateTime.Now);
			_stockExchange.ListStock("ibm", 1000000, 10m, DateTime.Now);
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Test_ListStock_IllegalPriceNegative()
		{
			_stockExchange.ListStock("IBM", 1000000, -10m, DateTime.Now);
		}

		[Test()]
		public void Test_SetStockPrice_NewPrice()
		{
			string stockName = "IBM";
			decimal oldPrice = 10m;
			_stockExchange.ListStock(stockName, 1000000, oldPrice, new DateTime(2012, 1, 10, 15, 22, 00));
			decimal newPrice = 20m;
			_stockExchange.SetStockPrice(stockName, new DateTime(2012, 1, 10, 15, 40, 00), newPrice);

			Assert.AreEqual(newPrice, _stockExchange.GetStockPrice(stockName, new DateTime(2012, 1, 10, 15, 50, 0, 0)));
		}

		[Test()]
		public void Test_CreateIndex_Simple()
		{
			string firstIndexName = "DOW JONES";
			_stockExchange.CreateIndex(firstIndexName, IndexTypes.AVERAGE);
			string secondIndexName = "S&P";
			_stockExchange.CreateIndex(secondIndexName, IndexTypes.WEIGHTED);
			Assert.AreEqual(2, _stockExchange.NumberOfIndices());
			Assert.True(_stockExchange.IndexExists(firstIndexName));
			Assert.True(_stockExchange.IndexExists(secondIndexName));
			Assert.False(_stockExchange.IndexExists("AB"));
		}

		[Test()]
		public void Test_AddStockToIndex_Simple()
		{
			string firstStockName = "IBM";
			_stockExchange.ListStock(firstStockName, 5, 100m, DateTime.Now);
			string secondStockName = "MSFT";
			_stockExchange.ListStock(secondStockName, 5, 200m, DateTime.Now);
			string thirdStockName = "GOOG";
			_stockExchange.ListStock(thirdStockName, 1, 300m, DateTime.Now);

			string indexName = "DOW JONES";
			_stockExchange.CreateIndex(indexName, IndexTypes.WEIGHTED);

			_stockExchange.AddStockToIndex(indexName, firstStockName);
			_stockExchange.AddStockToIndex(indexName, secondStockName);
			_stockExchange.AddStockToIndex(indexName, thirdStockName);

			Assert.True(_stockExchange.IsStockPartOfIndex(indexName, firstStockName));
			Assert.True(_stockExchange.IsStockPartOfIndex(indexName, secondStockName));
			Assert.True(_stockExchange.IsStockPartOfIndex(indexName, thirdStockName));
			Assert.AreEqual(3, _stockExchange.NumberOfStocksInIndex(indexName));
		}

		[Test()]
		public void Test_GetIndexValue_Weighted()
		{
			string firstStockName = "IBM";
			_stockExchange.ListStock(firstStockName, 1, 100m, new DateTime(2012, 1, 11, 14, 10, 00, 00));
			string secondStockName = "MSFT";
			_stockExchange.ListStock(secondStockName, 2, 200m, new DateTime(2012, 1, 11, 14, 10, 00, 00));

			string indexName = "DOW JONES";
			_stockExchange.CreateIndex(indexName, IndexTypes.WEIGHTED);

			_stockExchange.AddStockToIndex(indexName, firstStockName);
			_stockExchange.AddStockToIndex(indexName, secondStockName);


			Assert.AreEqual(180m, _stockExchange.GetIndexValue(indexName, new DateTime(2012, 1, 11, 14, 11, 00, 00)));
		}

		[Test()]
		public void Test_AddStockToPortfolio_SameStock()
		{
			string stockName = "IBM";
			_stockExchange.ListStock(stockName, 5, 100m, DateTime.Now);

			string portfolioID = "P1";
			_stockExchange.CreatePortfolio(portfolioID);

			_stockExchange.AddStockToPortfolio(portfolioID, stockName, 1);
			_stockExchange.AddStockToPortfolio(portfolioID, stockName, 2);

			Assert.True(_stockExchange.IsStockPartOfPortfolio(portfolioID, stockName));
			Assert.AreEqual(1, _stockExchange.NumberOfStocksInPortfolio(portfolioID));
			Assert.AreEqual(3, _stockExchange.NumberOfSharesOfStockInPortfolio(portfolioID, stockName));
		}

		[Test()]
		public void Test_RemoveStockFromPortfolio_NumOfShares()
		{
			string firstStockName = "IBM";
			_stockExchange.ListStock(firstStockName, 5, 100m, DateTime.Now);
			string secondStockName = "MSFT";
			_stockExchange.ListStock(secondStockName, 5, 200m, DateTime.Now);

			string portfolioID = "P1";
			_stockExchange.CreatePortfolio(portfolioID);
			_stockExchange.AddStockToPortfolio(portfolioID, firstStockName, 4);
			_stockExchange.AddStockToPortfolio(portfolioID, secondStockName, 1);

			_stockExchange.RemoveStockFromPortfolio(portfolioID, firstStockName, 2);

			Assert.True(_stockExchange.IsStockPartOfPortfolio(portfolioID, firstStockName));
			Assert.True(_stockExchange.IsStockPartOfPortfolio(portfolioID, secondStockName));
			Assert.AreEqual(2, _stockExchange.NumberOfStocksInPortfolio(portfolioID));
			Assert.AreEqual(2, _stockExchange.NumberOfSharesOfStockInPortfolio(portfolioID, firstStockName));
			Assert.AreEqual(1, _stockExchange.NumberOfSharesOfStockInPortfolio(portfolioID, secondStockName));
		}
		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Test_AddStockToIndex_Complicated()
		{
			// Dodaju se dionice u index, onda se jedna obriše s burze i pokuša se dohvatiti u indeksu 

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000000, 10m, DateTime.Now);
			string dionica2 = "Dionica2";
			_stockExchange.ListStock(dionica2, 1000000, 10m, DateTime.Now);

			string indeks1 = "indeks1";
			_stockExchange.CreateIndex(indeks1, IndexTypes.WEIGHTED);

			_stockExchange.AddStockToIndex(indeks1, dionica1);
			_stockExchange.AddStockToIndex(indeks1, dionica2);

			Assert.True(_stockExchange.IsStockPartOfIndex(indeks1, dionica1));
			Assert.True(_stockExchange.IsStockPartOfIndex(indeks1, dionica2));
			Assert.AreEqual(2, _stockExchange.NumberOfStocksInIndex(indeks1));

			_stockExchange.DelistStock(dionica1);

			_stockExchange.RemoveStockFromIndex(indeks1, dionica1);             // treba baciti exception
		}

		[Test()]
		public void Test_AddStockToIndex_MoreIndices()
		{
			// Dodaje se ista dionica u različite indexe 

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000000, 10m, DateTime.Now);

			string indeks1 = "indeks1";
			_stockExchange.CreateIndex(indeks1, IndexTypes.WEIGHTED);
			string indeks2 = "indeks2";
			_stockExchange.CreateIndex(indeks2, IndexTypes.AVERAGE);

			_stockExchange.AddStockToIndex(indeks1, dionica1);
			_stockExchange.AddStockToIndex(indeks2, dionica1);

			Assert.True(_stockExchange.IsStockPartOfIndex(indeks1, dionica1));
			Assert.True(_stockExchange.IsStockPartOfIndex(indeks2, dionica1));
			Assert.AreEqual(1, _stockExchange.NumberOfStocksInIndex(indeks1));
			Assert.AreEqual(1, _stockExchange.NumberOfStocksInIndex(indeks2));
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Test_AddStockToIndex_NoIndex()
		{
			// Dodaju se dionice u index koji ne postoji na burzi

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000000, 10m, DateTime.Now);
			string dionica2 = "Dionica2";
			_stockExchange.ListStock(dionica2, 1000000, 10m, DateTime.Now);

			string indeks1 = "IndeksKojiNePostoji";

			_stockExchange.AddStockToIndex("IndeksKojiNePostoji", dionica1);
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Test_AddStockToIndex_NoStock()
		{
			// Dodaju se dionice koje ne postoje na burzi u index koji postoji

			string indeks1 = "indeks1";
			_stockExchange.CreateIndex(indeks1, IndexTypes.WEIGHTED);

			_stockExchange.AddStockToIndex(indeks1, "dionicaKojaNePostoji");

		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Test_AddStockToIndex_SameStock()
		{
			// Dodaje se ista dionica više puta u index

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000000, 10m, DateTime.Now);

			string indeks1 = "indeks1";
			_stockExchange.CreateIndex(indeks1, IndexTypes.WEIGHTED);

			_stockExchange.AddStockToIndex(indeks1, dionica1);
			_stockExchange.AddStockToIndex(indeks1, dionica1);
		}

		[Test()]
		public void Test_AddStockToIndex_Simple_A()
		{
			// Dodaju se dionice u index
			string firstStockName = "IBM";
			_stockExchange.ListStock(firstStockName, 5, 100m, DateTime.Now);
			string secondStockName = "MSFT";
			_stockExchange.ListStock(secondStockName, 5, 200m, DateTime.Now);
			string thirdStockName = "GOOG";
			_stockExchange.ListStock(thirdStockName, 1, 300m, DateTime.Now);

			string indexName = "DOW JONES";
			_stockExchange.CreateIndex(indexName, IndexTypes.WEIGHTED);

			_stockExchange.AddStockToIndex(indexName, firstStockName);
			_stockExchange.AddStockToIndex(indexName, secondStockName);
			_stockExchange.AddStockToIndex(indexName, thirdStockName);

			Assert.True(_stockExchange.IsStockPartOfIndex(indexName, firstStockName));
			Assert.True(_stockExchange.IsStockPartOfIndex(indexName, secondStockName));
			Assert.True(_stockExchange.IsStockPartOfIndex(indexName, thirdStockName));
			Assert.AreEqual(3, _stockExchange.NumberOfStocksInIndex(indexName));
		}

		[Test()]
		public void Test_AddStockToPortfolio_Complicated()
		{
			// Dodaju se dionice u portfelj, onda se jedna obriše s burze i pokuša se dohvatiti u portfelju

			string dionica1 = "Dionica1";
			_stockExchange.ListStock (dionica1, 1000000, 10m, DateTime.Now);
			string dionica2 = "Dionica2";
			_stockExchange.ListStock (dionica2, 1000000, 10m, DateTime.Now);

			string portfelj1 = "portfelj1";
			_stockExchange.CreatePortfolio (portfelj1);
			_stockExchange.AddStockToPortfolio (portfelj1, dionica1, 1);
			_stockExchange.AddStockToPortfolio (portfelj1, dionica2, 1);

			Assert.True (_stockExchange.IsStockPartOfPortfolio (portfelj1, dionica1));
			Assert.True (_stockExchange.IsStockPartOfPortfolio (portfelj1, dionica2));
			Assert.AreEqual (2, _stockExchange.NumberOfStocksInPortfolio (portfelj1));

			_stockExchange.DelistStock (dionica1);

			Assert.False(_stockExchange.IsStockPartOfPortfolio (portfelj1, dionica1));   // treba baciti Exception		}
		}
		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Test_AddStockToPortfolio_GreaterThenNumOfShares()
		{
			// Dodaje se ista dionica više puta u portfelj - ukupno više od postojećeg broja 

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 100, 10m, DateTime.Now);

			string portfelj1 = "portfelj1";
			_stockExchange.CreatePortfolio(portfelj1);

			_stockExchange.AddStockToPortfolio(portfelj1, dionica1, 50);
			_stockExchange.AddStockToPortfolio(portfelj1, dionica1, 150);   // TODO: previše ih dodamo, treba ih dodati još 50 (ukupno ih mora biti 100)

			Assert.True(_stockExchange.IsStockPartOfPortfolio(portfelj1, dionica1));
			Assert.AreEqual(100, _stockExchange.NumberOfSharesOfStockInPortfolio(portfelj1, dionica1));
		}

		[Test()]
		public void Test_AddStockToPortfolio_MorePortfolios()
		{
			// Dodaje se ista dionica u različiti portfelj

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 100, 10m, DateTime.Now);

			string portfelj1 = "portfelj1";
			_stockExchange.CreatePortfolio(portfelj1);
			string portfelj2 = "portfelj2";
			_stockExchange.CreatePortfolio(portfelj2);

			_stockExchange.AddStockToPortfolio(portfelj1, dionica1, 50);
			_stockExchange.AddStockToPortfolio(portfelj2, dionica1, 30);

			Assert.True(_stockExchange.IsStockPartOfPortfolio(portfelj1, dionica1));
			Assert.True(_stockExchange.IsStockPartOfPortfolio(portfelj2, dionica1));

			Assert.AreEqual(50, _stockExchange.NumberOfSharesOfStockInPortfolio(portfelj1, dionica1));
			Assert.AreEqual(30, _stockExchange.NumberOfSharesOfStockInPortfolio(portfelj2, dionica1));
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Test_AddStockToPortfolio_NegativeNumberOfShares()
		{
			// dodaje se neispravan broj dionica u portfelj

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 100, 10m, DateTime.Now);

			string portfelj1 = "portfelj1";
			_stockExchange.CreatePortfolio(portfelj1);

			_stockExchange.AddStockToPortfolio(portfelj1, dionica1, -100);
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Test_AddStockToPortfolio_NoPortfolio()
		{
			// Dodaju se dionice u portfelj koji ne postoji na burzi

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 100, 10m, DateTime.Now);

			_stockExchange.AddStockToPortfolio("PortfeljKojiNePostoji", dionica1, 100);
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Test_AddStockToPortfolio_NoStock()
		{
			// Dodaju se dionice koje ne postoje na burzi u portfelj koji postoji

			string portfelj1 = "portfelj1";
			_stockExchange.CreatePortfolio(portfelj1);

			_stockExchange.AddStockToPortfolio(portfelj1, "NepostojeceDionice", 100);
		}

		[Test()]
		public void Test_AddStockToPortfolio_SameStock_A()
		{
			// Dodaje se ista dionica više puta u portfelj

			string stockName = "IBM";
			_stockExchange.ListStock(stockName, 5, 100m, DateTime.Now);

			string portfolioID = "P1";
			_stockExchange.CreatePortfolio(portfolioID);

			_stockExchange.AddStockToPortfolio(portfolioID, stockName, 1);
			_stockExchange.AddStockToPortfolio(portfolioID, stockName, 2);

			Assert.True(_stockExchange.IsStockPartOfPortfolio(portfolioID, stockName));
			Assert.AreEqual(1, _stockExchange.NumberOfStocksInPortfolio(portfolioID));
			Assert.AreEqual(3, _stockExchange.NumberOfSharesOfStockInPortfolio(portfolioID, stockName));
		}

		[Test()]
		public void Test_AddStockToPortfolio_Simple()
		{
			// Dodaju se dionice u portfelj

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 100, 10m, DateTime.Now);
			string dionica2 = "Dionica2";
			_stockExchange.ListStock(dionica2, 100, 10m, DateTime.Now);

			string portfelj1 = "portfelj1";
			_stockExchange.CreatePortfolio(portfelj1);

			_stockExchange.AddStockToPortfolio(portfelj1, dionica1, 50);
			_stockExchange.AddStockToPortfolio(portfelj1, dionica2, 50);

			Assert.True(_stockExchange.IsStockPartOfPortfolio(portfelj1, dionica1));
			Assert.True(_stockExchange.IsStockPartOfPortfolio(portfelj1, dionica2));

			Assert.AreEqual(2, _stockExchange.NumberOfStocksInPortfolio(portfelj1));
			Assert.AreEqual(50, _stockExchange.NumberOfSharesOfStockInPortfolio(portfelj1, dionica1));
			Assert.AreEqual(50, _stockExchange.NumberOfSharesOfStockInPortfolio(portfelj1, dionica2));
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Test_AddStockToPortfolios_GreaterThenNumOfShares()
		{
			// Dodaje se ista dionica u različiti portfelj - ukupno više od postojećeg broja 

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 100, 10m, DateTime.Now);

			string portfelj1 = "portfelj1";
			_stockExchange.CreatePortfolio(portfelj1);
			string portfelj2 = "portfelj2";
			_stockExchange.CreatePortfolio(portfelj2);

			_stockExchange.AddStockToPortfolio(portfelj1, dionica1, 50);
			_stockExchange.AddStockToPortfolio(portfelj2, dionica1, 150);   // OPREZ!!!!  u portfelju 2 ih treba biti samo 50 (jer ukupno u svim portfeljima mora biti 100)

			Assert.True(_stockExchange.IsStockPartOfPortfolio(portfelj1, dionica1));
			Assert.True(_stockExchange.IsStockPartOfPortfolio(portfelj2, dionica1));

			Assert.AreEqual(1, _stockExchange.NumberOfStocksInPortfolio(portfelj1));
			Assert.AreEqual(1, _stockExchange.NumberOfStocksInPortfolio(portfelj2));
			Assert.AreEqual(50, _stockExchange.NumberOfSharesOfStockInPortfolio(portfelj1, dionica1));
			Assert.AreEqual(50, _stockExchange.NumberOfSharesOfStockInPortfolio(portfelj2, dionica1));
		}




		// Test_ComplicatedChanges_Values
		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Test_ComplicatedChanges_Values()
		{
			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 100, 10m, DateTime.Now);

			string portfelj1 = "portfelj1";
			_stockExchange.CreatePortfolio(portfelj1);
			string portfelj2 = "portfelj2";
			_stockExchange.CreatePortfolio(portfelj2);

			_stockExchange.AddStockToPortfolio(portfelj1, dionica1, 50);
			_stockExchange.AddStockToPortfolio(portfelj2, dionica1, 150);

			_stockExchange.SetStockPrice ("dIonica1", DateTime.Now, 90m);
			_stockExchange.SetStockPrice ("dIonica1", DateTime.Now, 190m);
			_stockExchange.SetStockPrice ("dIonIca1", DateTime.Now, 70m);

			string index1 = "Index1";
			_stockExchange.CreateIndex (index1, IndexTypes.WEIGHTED);

			_stockExchange.AddStockToIndex (index1, "dIONica1");

			Assert.AreEqual( 70m ,_stockExchange.GetPortfolioValue (portfelj2, DateTime.Now));


		}



		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void IllegalTypeNegative()
		{
			string index1 = "Index";
			IndexTypes indexType = (IndexTypes) (-1);
			_stockExchange.CreateIndex (index1, indexType);
		}






		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Test_CreateIndex_SameNameAlreadyExists()
		{
			// Dodaje se indeks imena koje već postoji na burzi, ali drugog tipa 

			string indeks1 = "indeks1";
			_stockExchange.CreateIndex(indeks1, IndexTypes.AVERAGE);
			_stockExchange.CreateIndex(indeks1, IndexTypes.WEIGHTED);
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Test_CreateIndex_SameNameAndTypeAlreadyExists()
		{
			// Dodaje se indeks koji već postoji na burzi

			string indeks1 = "indeks1";
			_stockExchange.CreateIndex(indeks1, IndexTypes.AVERAGE);
			_stockExchange.CreateIndex(indeks1, IndexTypes.AVERAGE);
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Test_CreateIndex_SimilarNameAlreadyExists()
		{
			// Dodaje se indeks koja već postoji na burzi ali s promjenom velika/mala slova u imenu 

			_stockExchange.CreateIndex("abc", IndexTypes.AVERAGE);
			_stockExchange.CreateIndex("ABC", IndexTypes.AVERAGE);
		}

		[Test()]
		public void Test_CreateIndex_Simple_A()
		{
			// Dodaje se par indeksa i provjerava postoje li na burzi

			string firstIndexName = "DOW JONES";
			_stockExchange.CreateIndex(firstIndexName, IndexTypes.AVERAGE);
			string secondIndexName = "S&P";
			_stockExchange.CreateIndex(secondIndexName, IndexTypes.WEIGHTED);
			Assert.AreEqual(2, _stockExchange.NumberOfIndices());
			Assert.True(_stockExchange.IndexExists(firstIndexName));
			Assert.True(_stockExchange.IndexExists(secondIndexName));
			Assert.False(_stockExchange.IndexExists("AB"));
		}

		[Test()]
		public void Test_CreateIPortfolio_Simple()
		{
			// Dodaje se par portfelja i provjerava postoje li na burzi 

			string portfelj1 = "portfelj1";
			_stockExchange.CreatePortfolio(portfelj1);
			string portfelj2 = "portfelj2";
			_stockExchange.CreatePortfolio(portfelj2);

			Assert.True(_stockExchange.PortfolioExists(portfelj1));
			Assert.True(_stockExchange.PortfolioExists(portfelj2));
			Assert.AreEqual(2, _stockExchange.NumberOfPortfolios());
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Test_CreatePortfolio_SameIdAlreadyExists()
		{
			// Dodaje se portfelj koji već postoji na burzi 

			string portfelj1 = "portfelj1";
			_stockExchange.CreatePortfolio(portfelj1);
			_stockExchange.CreatePortfolio(portfelj1);

		}

		[Test()]
		public void Test_CreatePortfolio_SimilarNameAlreadyExists()
		{
			// Dodaje se portfelj sličnog imena 

			string portfelj1 = "portfelj1";
			_stockExchange.CreatePortfolio(portfelj1);
			string portfelj2 = "Portfelj1";
			_stockExchange.CreatePortfolio(portfelj2);

			Assert.True(_stockExchange.PortfolioExists(portfelj1));
			Assert.True(_stockExchange.PortfolioExists(portfelj2));
			Assert.AreEqual(2, _stockExchange.NumberOfPortfolios());

		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Test_DelistStock_EmptyStockExchange()
		{
			// Pokušaj micanja dionice s burze koja nema niti jednu dionicu 

			string dionica1 = "Dionica1";
			_stockExchange.DelistStock(dionica1);

			// ak se ne varam, već bi kod dodavanja trebalo baciti exception
		}
		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Test_DelistStock_EmptyStockExchange_A()
		{
			// Pokušaj micanja dionice s burze koja nema niti jednu dionicu 

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 0, 10m, DateTime.Now);

			// ak se ne varam, već bi kod dodavanja trebalo baciti exception
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Test_DelistStock_NotExist()
		{
			// Pokušaj micanja nepostojeće dionice s burze

			_stockExchange.DelistStock("nepostojecaDionica");
		}

		[Test()]
		public void Test_DelistStock_Simple()
		{
			// Postojeća dionica miče se s burze

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000000, 10m, DateTime.Now);

			Assert.True(_stockExchange.StockExists(dionica1));
			Assert.AreEqual(1, _stockExchange.NumberOfStocks());

			_stockExchange.DelistStock(dionica1);

			Assert.False(_stockExchange.StockExists(dionica1));
			Assert.AreEqual(0, _stockExchange.NumberOfStocks());
		}

		[Test()]
		public void Test_GetIndexValue_AfterDelistingStock()
		{
			// Provjera izračuna vrijednosti portfelja nakon brisanja dionice s burze

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 100, 10m, DateTime.Now);
			string dionica2 = "Dionica2";
			_stockExchange.ListStock(dionica2, 100, 20m, DateTime.Now);


			string portfelj1 = "portfelj1";
			_stockExchange.CreatePortfolio(portfelj1);
			_stockExchange.AddStockToPortfolio(portfelj1, dionica1, 10);
			_stockExchange.AddStockToPortfolio(portfelj1, dionica2, 10);

			Assert.AreEqual(300, _stockExchange.GetPortfolioValue(portfelj1, DateTime.Now));

			_stockExchange.DelistStock(dionica1);

			Assert.AreEqual(200, _stockExchange.GetPortfolioValue(portfelj1, DateTime.Now));
		}

		[Test()]
		public void Test_GetIndexValue_Average()
		{
			// Provjera izračuna AverageIndexa 

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000, 100m, DateTime.Now);
			System.Threading.Thread.Sleep(10);

			string dionica2 = "Dionica2";
			_stockExchange.ListStock(dionica2, 1000, 200m, DateTime.Now);
			System.Threading.Thread.Sleep(10);

			string indeks1 = "indeks1";
			_stockExchange.CreateIndex(indeks1, IndexTypes.AVERAGE);

			_stockExchange.AddStockToIndex(indeks1, dionica1);
			_stockExchange.AddStockToIndex(indeks1, dionica2);

			Assert.AreEqual(150, _stockExchange.GetIndexValue(indeks1, DateTime.Now));

		}

		[Test()]
		public void Test_GetIndexValue_AverageAfterDelistingStock()
		{
			// Provjera izračuna AverageIndeksa nakon brisanja dionice s burze

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000, 100m, DateTime.Now);
			System.Threading.Thread.Sleep(10);

			string dionica2 = "Dionica2";
			_stockExchange.ListStock(dionica2, 1000, 200m, DateTime.Now);
			System.Threading.Thread.Sleep(10);

			string indeks1 = "indeks1";
			_stockExchange.CreateIndex(indeks1, IndexTypes.AVERAGE);

			_stockExchange.AddStockToIndex(indeks1, dionica1);
			_stockExchange.AddStockToIndex(indeks1, dionica2);

			Assert.AreEqual(150, _stockExchange.GetIndexValue(indeks1, DateTime.Now));
			System.Threading.Thread.Sleep(10);

			_stockExchange.DelistStock(dionica2);

			Assert.AreEqual(100, _stockExchange.GetIndexValue(indeks1, DateTime.Now));
		}

		[Test()]
		public void Test_GetIndexValue_AverageAfterPriceChange()
		{
			// Provjera izračuna AverageIndexa nakon promijene cijene

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000, 100m, new DateTime(2014, 1, 1, 0, 0, 0));
			string dionica2 = "Dionica2";
			_stockExchange.ListStock(dionica2, 1000, 200m, new DateTime(2014, 1, 1, 0, 0, 0));

			string indeks1 = "indeks1";
			_stockExchange.CreateIndex(indeks1, IndexTypes.AVERAGE);

			_stockExchange.AddStockToIndex(indeks1, dionica1);
			_stockExchange.AddStockToIndex(indeks1, dionica2);

			Assert.AreEqual(150, _stockExchange.GetIndexValue(indeks1, new DateTime(2014, 1, 2, 0, 0, 0)));

			_stockExchange.SetStockPrice(dionica2, new DateTime(2014, 1, 3, 0, 0, 0), 300m);

			Assert.AreEqual(200, _stockExchange.GetIndexValue(indeks1, new DateTime(2014, 1, 4, 0, 0, 0)));
		}

		[Test()]
		public void Test_GetIndexValue_AverageAfterRemovingStock()
		{
			// Provjera izračuna AverageIndeksa nakon brisanja dionice iz indeksa 

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000, 100m, DateTime.Now);
			System.Threading.Thread.Sleep(10);

			string dionica2 = "Dionica2";
			_stockExchange.ListStock(dionica2, 1000, 200m, DateTime.Now);
			System.Threading.Thread.Sleep(10);

			string indeks1 = "indeks1";
			_stockExchange.CreateIndex(indeks1, IndexTypes.AVERAGE);

			_stockExchange.AddStockToIndex(indeks1, dionica1);
			_stockExchange.AddStockToIndex(indeks1, dionica2);

			Assert.AreEqual(150, _stockExchange.GetIndexValue(indeks1, DateTime.Now));

			_stockExchange.RemoveStockFromIndex(indeks1, dionica2);

			Assert.AreEqual(100, _stockExchange.GetIndexValue(indeks1, DateTime.Now));
		}





		// Test_GetIndexValue_Begining 
		[Test()]
		public void Test_GetIndexValue_Begining()
		{
			string Index = "Index";

			_stockExchange.CreateIndex (Index, IndexTypes.AVERAGE);
			Assert.AreEqual(0, _stockExchange.GetIndexValue(Index, DateTime.Now));
		}





		[Test()]
		public void Test_GetIndexValue_Weighted_A()
		{
			// Provjera izračuna WeightedIndeksa 

			string firstStockName = "IBM";
			_stockExchange.ListStock(firstStockName, 1, 100m, new DateTime(2012, 1, 11, 14, 10, 00, 00));
			string secondStockName = "MSFT";
			_stockExchange.ListStock(secondStockName, 2, 200m, new DateTime(2012, 1, 11, 14, 10, 00, 00));

			string indexName = "DOW JONES";
			_stockExchange.CreateIndex(indexName, IndexTypes.WEIGHTED);

			_stockExchange.AddStockToIndex(indexName, firstStockName);
			_stockExchange.AddStockToIndex(indexName, secondStockName);

			Assert.AreEqual(180m, _stockExchange.GetIndexValue(indexName, new DateTime(2012, 1, 11, 14, 11, 00, 00)));
		}

		[Test()]
		public void Test_GetIndexValue_WeightedDecimal()
		{
			// Provjera izračuna WeightedIndeksa decimalno

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1, 110m, new DateTime(2014, 1, 1, 0, 0, 0));
			string dionica2 = "Dionica2";
			_stockExchange.ListStock(dionica2, 2, 200m, new DateTime(2014, 1, 1, 0, 0, 0));

			string indeks1 = "indeks1";
			_stockExchange.CreateIndex(indeks1, IndexTypes.WEIGHTED);

			_stockExchange.AddStockToIndex(indeks1, dionica1);
			_stockExchange.AddStockToIndex(indeks1, dionica2);

			Assert.AreEqual(180.588m, _stockExchange.GetIndexValue(indeks1, new DateTime(2014, 1, 2, 0, 0, 0 )));
		}

		[Test()]
		public void Test_GetInitialStockPrice_RandomPrices()
		{
			// Postavljanje cijena u trenucima i dohvaćanje početne

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000, 100m, new DateTime(2013, 1, 1, 1, 0, 0, 0));

			_stockExchange.SetStockPrice(dionica1, new DateTime(2014, 1, 1, 1, 0, 0, 0), 200m);      // cijena nakon
			_stockExchange.SetStockPrice(dionica1, new DateTime(2012, 1, 1, 1, 0, 0, 0), 300m);      // cijena prije - trebala bi biti inicijalna jer je najstarija

			Assert.AreEqual(300m, _stockExchange.GetInitialStockPrice(dionica1));
		}

		[Test()]
		public void Test_GetLastStockPrice_RandomPrices()
		{
			// Postavljanje cijena u trenucima i dohvaćanje zadnje

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000, 100m, new DateTime(2013, 1, 1, 1, 0, 0, 0));

			_stockExchange.SetStockPrice(dionica1, new DateTime(2014, 1, 1, 1, 0, 0, 0), 200m);      // cijena nakon - najnovija/zadnja
			_stockExchange.SetStockPrice(dionica1, new DateTime(2012, 1, 1, 1, 0, 0, 0), 300m);      // cijena prije

			Assert.AreEqual(200m, _stockExchange.GetLastStockPrice(dionica1));
		}





			// Test_GetPortfolioPercentChangeInValueForMonth_Complicated
		[Test()]
		public void Test_GetPortfolioPercentChangeInValueForMonth_Complicated()
		{
			// provjera izračuna postotka mjesečne promjene

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000, 100m, new DateTime(2014, 1, 1, 0, 0, 0, 0));       // 1.1.2014. 0:00 100kn

			_stockExchange.SetStockPrice(dionica1, new DateTime(2014, 1, 31, 23, 59, 59), 150);        // 31.1.2014. 0:00 150kn (+50%)

			string portfolio1 = "portfolio1";
			_stockExchange.CreatePortfolio(portfolio1);

			_stockExchange.AddStockToPortfolio(portfolio1, dionica1, 100);

			Assert.AreEqual(50, _stockExchange.GetPortfolioPercentChangeInValueForMonth(portfolio1, 2014, 1));  // 1. mjesec 2014.
		}
		

		// Test_GetPortfolioPercentChangeInValueForMonth_InitialPrices
			[Test()]
			public void Test_GetPortfolioPercentChangeInValueForMonth_InitialPrices()
			{
				// provjera izračuna postotka mjesečne promjene

				string dionica1 = "Dionica1";
				_stockExchange.ListStock(dionica1, 1000, 100m, new DateTime(2014, 1, 1, 0, 0, 0, 0));       // 1.1.2014. 0:00 100kn
				_stockExchange.ListStock("dionica2", 1000, 100m, new DateTime(2014, 1, 2, 0, 0, 0, 0));
			
				string portfolio1 = "portfolio1";
				_stockExchange.CreatePortfolio(portfolio1);

				_stockExchange.AddStockToPortfolio(portfolio1, dionica1, 100);
				_stockExchange.AddStockToPortfolio(portfolio1, "Dionica2", 100);

				Assert.AreEqual(0, _stockExchange.GetPortfolioPercentChangeInValueForMonth(portfolio1, 2015, 4));  // 1. mjesec 2014.
			}


		// Test_GetPortfolioPercentChangeInValueForMonth_PriceChanges
			[Test()]
			public void Test_GetPortfolioPercentChangeInValueForMonth_PriceChanges()
			{
				// provjera izračuna postotka mjesečne promjene

				string dionica1 = "Dionica1";
				_stockExchange.ListStock(dionica1, 1000, 100, new DateTime(2014, 1, 1, 0, 0, 0, 0));       // 1.1.2014. 0:00 100kn
				string dionica2 = "Dionica12";
				_stockExchange.ListStock(dionica2, 1000, 100, new DateTime(2014, 1, 1, 0, 0, 0, 0)); 
				string dionica3 = "Dionica13";
				_stockExchange.ListStock(dionica3, 1000, 100, new DateTime(2014, 1, 1, 0, 0, 0, 0)); 
				string dionica4 = "Dionica14";
				_stockExchange.ListStock(dionica4, 1000, 100, new DateTime(2014, 1, 1, 0, 0, 0, 0)); 

				_stockExchange.SetStockPrice(dionica1, new DateTime(2014, 1, 30, 23, 59, 59), 150);        // 31.1.2014. 0:00 150kn (+50%)
				_stockExchange.SetStockPrice(dionica2, new DateTime(2014, 1, 30, 23, 59, 59), 150);        // 31.1.2014. 0:00 150kn (+50%)
				_stockExchange.SetStockPrice(dionica3, new DateTime(2014, 1, 30, 23, 59, 59), 150);        // 31.1.2014. 0:00 150kn (+50%)
				_stockExchange.SetStockPrice(dionica4, new DateTime(2014, 1, 30, 23, 59, 59), 150);        // 31.1.2014. 0:00 150kn (+50%)


				string portfolio1 = "portfolio1";
				_stockExchange.CreatePortfolio(portfolio1);

				_stockExchange.AddStockToPortfolio(portfolio1, dionica1, 1);
				_stockExchange.AddStockToPortfolio(portfolio1, dionica1, 1);
				_stockExchange.AddStockToPortfolio(portfolio1, dionica2, 1);
				_stockExchange.AddStockToPortfolio(portfolio1, dionica3, 1);
				_stockExchange.AddStockToPortfolio(portfolio1, dionica4, 1);

				Assert.AreEqual(50, _stockExchange.GetPortfolioPercentChangeInValueForMonth(portfolio1, 2014, 1));  // 1. mjesec 2014.
			}




		[Test()]
		public void Test_GetPortfolioPercentChangeInValueForMonth_Simple()
		{
			// provjera izračuna postotka mjesečne promjene

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000, 100m, new DateTime(2014, 1, 1, 0, 0, 0, 0));       // 1.1.2014. 0:00 100kn

			_stockExchange.SetStockPrice(dionica1, new DateTime(2014, 1, 31, 0, 0, 0, 0), 150);        // 15.1.2014. 0:00 150kn (+50%)

			string portfolio1 = "portfolio1";
			_stockExchange.CreatePortfolio(portfolio1);

			_stockExchange.AddStockToPortfolio(portfolio1, dionica1, 100);

			Assert.AreEqual(50, _stockExchange.GetPortfolioPercentChangeInValueForMonth(portfolio1, 2014, 1));  // 1. mjesec 2014.
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Test_GetPortfolioPercentChangeInValueForMonth_WrongDate()
		{
			// provjera izračuna postotka mjesečne promjene uz pogrešnu vrijednost datuma 

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000, 100m, new DateTime(2014, 1, 1, 0, 0, 0, 0));       // 1.1.2014. 0:00 100kn

			_stockExchange.SetStockPrice(dionica1, new DateTime(2014, 1, 31, 0, 0, 0, 0), 150);        // 15.1.2014. 0:00 150kn (+50%)

			string portfolio1 = "portfolio1";
			_stockExchange.CreatePortfolio(portfolio1);

			_stockExchange.AddStockToPortfolio(portfolio1, dionica1, 100);

			Assert.AreEqual(50, _stockExchange.GetPortfolioPercentChangeInValueForMonth(portfolio1, 2014, 20));  // 20. mjesec 2014. (krivi datum)
		}

		[Test()]
		public void Test_GetPortfolioValue_AfterPriceChange()
		{
			// Provjera izračuna vrijednosti portfelja nakon promijene cijene 

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000, 100m, new DateTime(2014, 1, 1, 0, 0, 0, 0));       // 1.1.2014. 0:00 100kn

			string portfolio1 = "portfolio1";
			_stockExchange.CreatePortfolio(portfolio1);

			_stockExchange.AddStockToPortfolio(portfolio1, dionica1, 10);

			Assert.AreEqual(1000, _stockExchange.GetPortfolioValue(portfolio1, new DateTime(2014, 3, 1, 0, 0, 0, 0)));  // 1.3.2014. 0:00   PROVJERA

			_stockExchange.SetStockPrice(dionica1, new DateTime(2014, 1, 31, 0, 0, 0, 0), 200);                         // 1.2.2014. 0:00 200kn

			Assert.AreEqual(2000, _stockExchange.GetPortfolioValue(portfolio1, new DateTime(2014, 3, 1, 0, 0, 0, 0)));  // 1.3.2014. 0:00
		}

		[Test()]
		public void Test_GetPortfolioValue_AfterRemovingStock()
		{
			// Provjera izračuna vrijednosta portfelja nakon brisanja dionice iz portfolia 

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000, 100m, new DateTime(2014, 1, 1, 0, 0, 0, 0));       // 1.1.2014. 0:00 100kn
			string dionica2 = "Dionica2";
			_stockExchange.ListStock(dionica2, 1000, 100m, new DateTime(2014, 1, 1, 0, 0, 0, 0));       // 1.1.2014. 0:00 100kn

			string portfolio1 = "portfolio1";
			_stockExchange.CreatePortfolio(portfolio1);

			_stockExchange.AddStockToPortfolio(portfolio1, dionica1, 10);
			_stockExchange.AddStockToPortfolio(portfolio1, dionica2, 10);

			Assert.AreEqual(2000, _stockExchange.GetPortfolioValue(portfolio1, new DateTime(2014, 3, 1, 0, 0, 0, 0)));  // 1.3.2014. 0:00   PROVJERA

			_stockExchange.RemoveStockFromPortfolio(portfolio1, dionica2);

			Assert.AreEqual(1000, _stockExchange.GetPortfolioValue(portfolio1, new DateTime(2014, 3, 1, 0, 0, 0, 0)));  // 1.3.2014. 0:00
		}

		[Test()]
		public void Test_GetPortfolioValue_Begining()
		{
			// Provjera izračuna vrijednosti portfelja na početku

			string portfolio1 = "portfolio1";
			_stockExchange.CreatePortfolio(portfolio1);

			Assert.AreEqual(0, _stockExchange.GetPortfolioValue(portfolio1, DateTime.Now));
		}

		[Test()]
		public void Test_GetPortfolioValue_Sum()
		{
			// Provjera izračuna vrijednosti portfelja na početku

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000, 100m, new DateTime(2014, 1, 1, 0, 0, 0, 0));       // 1.1.2014. 0:00 100kn
			string dionica2 = "Dionica2";
			_stockExchange.ListStock(dionica2, 1000, 100m, new DateTime(2014, 1, 1, 0, 0, 0, 0));       // 1.1.2014. 0:00 100kn

			string portfolio1 = "portfolio1";
			_stockExchange.CreatePortfolio(portfolio1);

			_stockExchange.AddStockToPortfolio(portfolio1, dionica1, 10);
			_stockExchange.AddStockToPortfolio(portfolio1, dionica2, 10);

			Assert.AreEqual(2000, _stockExchange.GetPortfolioValue(portfolio1, new DateTime(2014, 3, 1, 0, 0, 0, 0)));  // 1.3.2014. 0:00   PROVJERA
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Test_GetStockPrice_BeforeAlPrices()
		{
			// Postavlja više cijena i dohvaćanje cijenu prije svih trenutaka 

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000000, 10m, new DateTime(2014, 1, 1, 0, 0, 0, 0));     // 1.1.2014.
			_stockExchange.SetStockPrice(dionica1, new DateTime(2014, 1, 2, 0, 0, 0, 0), 100m);         // 2.1.2014.

			_stockExchange.GetStockPrice(dionica1, new DateTime(2013, 1, 1, 0, 0, 0, 0));               // 1.1.2013.    dionica tada nije ni postojala - exception
		}




		// Test_GetStockPrice_BeforeInitialPrice        // nisam siguran na kaj se misli
		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Test_GetStockPrice_BeforeAlPrices_A()
		{
				string dionica1 = "Dionica1";
				_stockExchange.ListStock(dionica1, 1000000, 10m, new DateTime(2014, 1, 1, 0, 0, 0, 0));  
				_stockExchange.GetStockPrice(dionica1, new DateTime(2013, 1, 1, 0, 0, 0, 0));
		}

		// Test_GetStockPrice_InitialPrice
			[Test()]
			public void Test_GetStockPrice_InitialPrice()
			{
				string dionica1 = "Dionica1";
				_stockExchange.ListStock(dionica1, 1000000, 10m, new DateTime(2014, 1, 1, 0, 0, 0, 0));  
				Assert.AreEqual( 10m, _stockExchange.GetInitialStockPrice(dionica1));
			}

		[Test()]
		public void Test_GetStockPrice_LastPrice()
		{
			// Postavljanje više cijena dionice i provjera vrijednosti zadnje

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000000, 100m, new DateTime(2014, 1, 1, 0, 0, 0, 0));     // 1.1.2014.
			_stockExchange.SetStockPrice(dionica1, new DateTime(2014, 1, 2, 0, 0, 0, 0), 200m);         // 2.1.2014.
			_stockExchange.SetStockPrice(dionica1, new DateTime(2014, 1, 3, 0, 0, 0, 0), 300m);         // 3.1.2014.

			Assert.AreEqual(300m, _stockExchange.GetLastStockPrice(dionica1));
		}

		[Test()]
		public void Test_GetStockPrice_MorePrices()
		{
			// Postavljanje više cijena dionica i provjera za određene trenutke

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000000, 100m, new DateTime(2014, 1, 1, 0, 0, 0, 0));    // 1.1.2014.     100
			_stockExchange.SetStockPrice(dionica1, new DateTime(2014, 1, 10, 0, 0, 0, 0), 200m);        // 10.1.2014.    200
			_stockExchange.SetStockPrice(dionica1, new DateTime(2014, 1, 20, 0, 0, 0, 0), 300m);        // 20.1.2014.    300

			Assert.AreEqual(100m, _stockExchange.GetStockPrice(dionica1, new DateTime(2014, 1, 1, 10, 0, 0, 0)));   // 1.1.2014.    10:00   100
			Assert.AreEqual(200m, _stockExchange.GetStockPrice(dionica1, new DateTime(2014, 1, 14, 0, 1, 0, 0)));   // 14.1.2014.   00:01   200
			Assert.AreEqual(300m, _stockExchange.GetStockPrice(dionica1, new DateTime(2014, 2, 25, 0, 0, 0, 0)));   // 25.2.2014.   00:00   300
		}




		// Test_GetStockPrice_RandomPrices

			[Test()]
			public void Test_GetStockPrice_MorePrices_A()
			{
				// Postavljanje više cijena dionica i provjera za određene trenutke

				string dionica1 = "Dionica1";
				_stockExchange.ListStock(dionica1, 1000000, 100m, new DateTime(2014, 1, 1, 0, 0, 0, 0));    // 1.1.2014.     100
				_stockExchange.SetStockPrice(dionica1, new DateTime(2014, 1, 10, 0, 0, 0, 0), 200m);        // 10.1.2014.    200
				_stockExchange.SetStockPrice(dionica1, new DateTime(2014, 1, 20, 0, 0, 0, 0), 300m);        // 20.1.2014.    300

				Assert.AreEqual(200m, _stockExchange.GetStockPrice(dionica1, new DateTime(2014, 1, 11, 10, 0, 0, 0)));   // 1.1.2014.    10:00   100
				Assert.AreEqual(200m, _stockExchange.GetStockPrice(dionica1, new DateTime(2014, 1, 14, 0, 1, 0, 0)));   // 14.1.2014.   00:01   200
				Assert.AreEqual(300m, _stockExchange.GetStockPrice(dionica1, new DateTime(2014, 2, 25, 0, 0, 0, 0)));   // 25.2.2014.   00:00   300
			}




		[Test()]
		public void Test_GetStockPrice_SimilarName()
		{
			// Dohvaćanje početnu cijenu dionicu - upit sa sličnim imenom 

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000000, 300m, new DateTime(2014, 1, 20, 0, 0, 0, 0));   // 20.1.2014.   300
			_stockExchange.SetStockPrice(dionica1, new DateTime(2014, 1, 1, 0, 0, 0, 0), 100m);         // 1.1.2014.    100
				_stockExchange.SetStockPrice(dionica1, new DateTime(2014, 1, 10, 0, 0, 0, 0), 200m);        // 10.1.2014.   200

			Assert.AreEqual(100m, _stockExchange.GetInitialStockPrice("DIOnicA1"));
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Test_ListStock_IllegalNumberOfSharesNegative()
		{
			// Dodaje se dionica s negativnim brojem dionica

			_stockExchange.ListStock("IBM", -10, 10m, DateTime.Now);
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Test_ListStock_IllegalNumberOfSharesNull()
		{
			// Dodaje se dionica s 0 dionica

			_stockExchange.ListStock("IBM", (long )(0), 10m, DateTime.Now);
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Test_ListStock_IllegalPriceNegative_A()
		{
			_stockExchange.ListStock("IBM", 1000000, -10m, DateTime.Now);
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Test_ListStock_IllegalPriceNull()
		{
			_stockExchange.ListStock("IBM", 1000000, 0, DateTime.Now);
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Test_ListStock_SameName()
		{
			// Dodaje se dionica koja ima isto ime kao neka koja već postoji na burzi 

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000000, 100m, new DateTime(2014, 1, 1, 0, 0, 0, 0));

			_stockExchange.ListStock(dionica1, 1000000, 100m, new DateTime(2014, 1, 1, 0, 0, 0, 0));
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Test_ListStock_SameNameAlreadyExists_A()
		{
			_stockExchange.ListStock("IBM", 1000000, 10m, DateTime.Now);
			_stockExchange.ListStock("IBM", 1000000, 10m, DateTime.Now);
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Test_ListStock_SimilarNameAlreadyExists()
		{
			// Dodaje se dionica koja već postoji na burzi ali s promjenom velika/mala slova u imenu

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000000, 100m, new DateTime(2014, 1, 1, 0, 0, 0, 0));

			_stockExchange.ListStock("DiOnIcA1", 1000000, 100m, new DateTime(2014, 1, 1, 0, 0, 0, 0));
		}

		[Test()]
		public void Test_ListStock_Simple_A()
		{
			Assert.AreEqual(0, _stockExchange.NumberOfStocks());
			string firstStockName = "IBM";
			_stockExchange.ListStock(firstStockName, 1000000, 10m, DateTime.Now);

			Assert.AreEqual(1, _stockExchange.NumberOfStocks());
			Assert.True(_stockExchange.StockExists(firstStockName));
			Assert.False(_stockExchange.StockExists("Bezveze"));

			string secondStockName = "MSFT";
			_stockExchange.ListStock(secondStockName, 100000, 15m, DateTime.Now);
			Assert.AreEqual(2, _stockExchange.NumberOfStocks());
			Assert.True(_stockExchange.StockExists(secondStockName));
		}

		[Test()]
		public void Test_NumberOfStocksInIndex_NoStocks()
		{
			// Ispitivanje broja dionica za indeks kojem nisu pridijeljene dionice

			string indeks1 = "indeks1";
			_stockExchange.CreateIndex(indeks1, IndexTypes.WEIGHTED);
			Assert.AreEqual(0, _stockExchange.NumberOfStocksInIndex(indeks1));

			string indeks2 = "indeks2";
			_stockExchange.CreateIndex(indeks2, IndexTypes.AVERAGE);
			Assert.AreEqual(0, _stockExchange.NumberOfStocksInIndex(indeks2));
		}

		[Test()]
		public void Test_NumberOfStocksInPortfolio_NoStocks()
		{
			// Ispitivanje broja dionica za portfelj kojem nisu pridijeljene dionice

			string portfelj1 = "portfelj1";
			_stockExchange.CreatePortfolio(portfelj1);

			Assert.AreEqual(0, _stockExchange.NumberOfStocksInPortfolio(portfelj1));
		}






		// Test_RemoveStockFromIndex_MoreIndices

			[Test()]
			[ExpectedException(typeof(StockExchangeException))]
			public void Test_RemoveStockFromIndex_MoreIndices()
			{

				string dionica1 = "Dionica1";
				string Index1 = "index1";
			string Index2 = "index2";
				_stockExchange.ListStock(dionica1, 1000000, 10m, DateTime.Now);

				_stockExchange.AddStockToIndex(dionica1, Index1);
				_stockExchange.AddStockToIndex(dionica1, Index2);

				_stockExchange.RemoveStockFromIndex(Index2, dionica1);

				Assert.AreEqual(1, _stockExchange.NumberOfStocksInIndex(Index1));

			}



		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Test_RemoveStockFromIndex_NonExistingIndex()
		{
			// Briše se dionica iz nepostojećeg indeksa

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000000, 10m, DateTime.Now);

			_stockExchange.RemoveStockFromIndex("nepostojeciIndeks", dionica1);
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Test_RemoveStockFromIndex_NonExistingStock()
		{
			// Briše se dionica koja ne postoji iz indeksa koji postoji

			string indeks1 = "indeks1";
			_stockExchange.CreateIndex(indeks1, IndexTypes.WEIGHTED);

			_stockExchange.RemoveStockFromIndex(indeks1, "NepostojecaDionica");     // treba baciti exception
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Test_RemoveStockFromIndex_Twice()
		{
			// 2 puta se briše ista dionica

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000000, 10m, DateTime.Now);

			string indeks1 = "indeks1";
			_stockExchange.CreateIndex(indeks1, IndexTypes.WEIGHTED);

			_stockExchange.AddStockToIndex(indeks1, dionica1);

			_stockExchange.RemoveStockFromIndex(indeks1, dionica1);
			_stockExchange.RemoveStockFromIndex(indeks1, dionica1);     // treba baciti exception
		}




		// Test_RemoveStockFromPortfolio_All
		[Test()]
			public void Test_RemoveStockFromPortfolio_All()
			{
				string dionica1 = "Dionica1";
				_stockExchange.ListStock(dionica1, 1000000, 10m, DateTime.Now);
				string port = "port";

			_stockExchange.CreatePortfolio (port);
			_stockExchange.AddStockToPortfolio(port, dionica1, 100);
			_stockExchange.RemoveStockFromPortfolio(port, dionica1, 100);

			Assert.AreEqual( 0, _stockExchange.NumberOfStocksInPortfolio(port));

			}

		// Test_RemoveStockFromPortfolio_AllTwice
			// Test_RemoveStockFromPortfolio_All
			[Test()]
			[ExpectedException(typeof(StockExchangeException))]
			public void Test_RemoveStockFromPortfolio_AllTwice()
			{
				string dionica1 = "Dionica1";
				_stockExchange.ListStock(dionica1, 1000000, 10m, DateTime.Now);
				string Index = "index";

				_stockExchange.AddStockToPortfolio(Index, dionica1, 100);
			_stockExchange.RemoveStockFromPortfolio(Index, dionica1);
			_stockExchange.RemoveStockFromPortfolio(Index, dionica1);


			}



		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Test_RemoveStockFromPortfolio_NonExistingPortfolio()
		{
			// Briše se dionica iz nepostojećeg portfelja 

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000000, 10m, DateTime.Now);

			_stockExchange.RemoveStockFromPortfolio("nepostojeciPortfelj", dionica1);
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Test_RemoveStockFromPortfolio_NonExistingStock()
		{
			// Briše se dionica koja ne postoji iz portfelja koji postoji

			string portfelj1 = "portfelj1";
			_stockExchange.CreatePortfolio(portfelj1);

			_stockExchange.RemoveStockFromPortfolio(portfelj1, "nepostojecaDionica");
		}

		[Test()]
		public void Test_RemoveStockFromPortfolio_NumOfShares_A()
		{
			// Briše se određeni broj dionica iz portfelja

			string firstStockName = "IBM";
			_stockExchange.ListStock(firstStockName, 5, 100m, DateTime.Now);
			string secondStockName = "MSFT";
			_stockExchange.ListStock(secondStockName, 5, 200m, DateTime.Now);

			string portfolioID = "P1";
			_stockExchange.CreatePortfolio(portfolioID);
			_stockExchange.AddStockToPortfolio(portfolioID, firstStockName, 4);
			_stockExchange.AddStockToPortfolio(portfolioID, secondStockName, 1);

			_stockExchange.RemoveStockFromPortfolio(portfolioID, firstStockName, 2);

			Assert.True(_stockExchange.IsStockPartOfPortfolio(portfolioID, firstStockName));
			Assert.True(_stockExchange.IsStockPartOfPortfolio(portfolioID, secondStockName));
			Assert.AreEqual(2, _stockExchange.NumberOfStocksInPortfolio(portfolioID));
			Assert.AreEqual(2, _stockExchange.NumberOfSharesOfStockInPortfolio(portfolioID, firstStockName));
			Assert.AreEqual(1, _stockExchange.NumberOfSharesOfStockInPortfolio(portfolioID, secondStockName));
		}

		[Test()]
		public void Test_RemoveStockFromPortfolio_Simple()
		{
			// Briše se dionica iz portfelja

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 100, 10m, DateTime.Now);

			string portfelj1 = "portfelj1";
			_stockExchange.CreatePortfolio(portfelj1);
			_stockExchange.AddStockToPortfolio(portfelj1, dionica1, 100);

			Assert.True(_stockExchange.IsStockPartOfPortfolio(portfelj1, dionica1));
			Assert.AreEqual(1, _stockExchange.NumberOfStocksInPortfolio(portfelj1));

			_stockExchange.RemoveStockFromPortfolio(portfelj1, dionica1);

			Assert.False(_stockExchange.IsStockPartOfPortfolio(portfelj1, dionica1));
			Assert.AreEqual(0, _stockExchange.NumberOfStocksInPortfolio(portfelj1));
		}

		[Test()]
		public void Test_RemoveStockFromPortfolio_Twice()
		{
			// 2 puta briše se određeni broj iste dionice

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 100, 10m, DateTime.Now);

			string portfelj1 = "portfelj1";
			_stockExchange.CreatePortfolio(portfelj1);
			_stockExchange.AddStockToPortfolio(portfelj1, dionica1, 100);

			Assert.True(_stockExchange.IsStockPartOfPortfolio(portfelj1, dionica1));
			Assert.AreEqual(1, _stockExchange.NumberOfStocksInPortfolio(portfelj1));
			Assert.AreEqual(100, _stockExchange.NumberOfSharesOfStockInPortfolio(portfelj1, dionica1));     // dodaj 100

			_stockExchange.RemoveStockFromPortfolio(portfelj1, dionica1, 50);                               // izbriši 50

			Assert.True(_stockExchange.IsStockPartOfPortfolio(portfelj1, dionica1));
			Assert.AreEqual(1, _stockExchange.NumberOfStocksInPortfolio(portfelj1));
			Assert.AreEqual(50, _stockExchange.NumberOfSharesOfStockInPortfolio(portfelj1, dionica1));      // treba ih biti 50


			_stockExchange.RemoveStockFromPortfolio(portfelj1, dionica1, 50);                               // izbriši još 50

			Assert.False(_stockExchange.IsStockPartOfPortfolio(portfelj1, dionica1));
			Assert.AreEqual(0, _stockExchange.NumberOfStocksInPortfolio(portfelj1));
			Assert.AreEqual(0, _stockExchange.NumberOfSharesOfStockInPortfolio(portfelj1, dionica1));       // treba ih biti 0
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Test_SetStockPrice_DifferentName()
		{
			// postavljanje cijene dionice sa drugačijim imenom 

			decimal oldPrice = 10m;
			_stockExchange.ListStock("IBM", 1000000, oldPrice, new DateTime(2012, 1, 10, 15, 22, 00));

			decimal newPrice = 20m;
			_stockExchange.SetStockPrice("KRIVO_IME", new DateTime(2012, 1, 10, 15, 40, 00), newPrice);
		}

		[Test()]
		public void Test_SetStockPrice_NewPrice_A()
		{
			// Postavljanje cijene dionice i provjera vrijednosti za datume

			string stockName = "IBM";
			decimal oldPrice = 10m;
			_stockExchange.ListStock(stockName, 1000000, oldPrice, new DateTime(2012, 1, 10, 15, 22, 00));
			decimal newPrice = 20m;
			_stockExchange.SetStockPrice(stockName, new DateTime(2012, 1, 10, 15, 40, 00), newPrice);

			Assert.AreEqual(newPrice, _stockExchange.GetStockPrice(stockName, new DateTime(2012, 1, 10, 15, 50, 0, 0)));
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Test_SetStockPrice_SameTimeStamp()
		{
			// Pokušaj dodavanja cijene za trenutak koji već postoji

			_stockExchange.ListStock("IBM", 1000000, 10m, DateTime.Now);

			decimal cijena = 10m;
			_stockExchange.SetStockPrice("IBM", new DateTime(2012, 1, 10, 15, 40, 00), cijena);
			_stockExchange.SetStockPrice("IBM", new DateTime(2012, 1, 10, 15, 40, 00), cijena);
		}

		[Test()]
		public void Test_SetStockPrice_SimilarName()
		{
			// Postavljanje cijene dionice sa sličnim imenom

			_stockExchange.ListStock("IBM", 1000000, 10m, DateTime.Now);

			decimal novaCijena = 20m;
			_stockExchange.SetStockPrice("IbM", new DateTime(2012, 1, 10, 15, 40, 00), novaCijena);
			Assert.AreEqual(novaCijena, _stockExchange.GetStockPrice("IBM", new DateTime(2012, 1, 10, 15, 40, 00)));
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Test_SetStockPrice_StockNotExists()
		{
			// Pokušaj postavljanja cijene dionici koja ne postoji
			_stockExchange.ListStock("IBM", 1000000, 10m, DateTime.Now);

			decimal cijena = 10m;
			_stockExchange.SetStockPrice("KRIVO_IME", new DateTime(2012, 1, 10, 15, 40, 00), cijena);
		}

		[Test()]
		public void Test_StockExchangeAtTheBeginig_A()
		{
			// provjera početnih vrijednosti na burzi

			Assert.AreEqual(0, _stockExchange.NumberOfStocks());
			Assert.AreEqual(0, _stockExchange.NumberOfIndices());
			Assert.AreEqual(0, _stockExchange.NumberOfPortfolios());
		}

		
		[Test()]
		public void Ankica_TestStockExchangeAtTheBeginig()
		{
			_stockExchange = Factory.CreateStockExchange();
			Assert.AreEqual(0, _stockExchange.NumberOfStocks());
			Assert.AreEqual(0, _stockExchange.NumberOfIndices());
			Assert.AreEqual(0, _stockExchange.NumberOfPortfolios());
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Ankica_TestListStock_SameNameAlreadyExists()
		{
			_stockExchange = Factory.CreateStockExchange();
			_stockExchange.ListStock("IBM", 1000000, 10m, DateTime.Now);
			_stockExchange.ListStock("IBM", 1000000, 10m, DateTime.Now);
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Ankica_TestListStock_IllegalPriceNegative()
		{
			_stockExchange = Factory.CreateStockExchange();
			_stockExchange.ListStock("IBM", 1000000, -10m, DateTime.Now);
		}

		[Test()]
		public void Ankica_TestSetStockPrice_NewPrice()
		{
			_stockExchange = Factory.CreateStockExchange();
			string stockName = "IBM";
			decimal oldPrice = 10m;
			_stockExchange.ListStock(stockName, 1000000, oldPrice, new DateTime(2012, 1, 10, 15, 22, 00));
			decimal newPrice = 20m;
			_stockExchange.SetStockPrice(stockName, new DateTime(2012, 1, 10, 15, 40, 00), newPrice);

			Assert.AreEqual(newPrice, _stockExchange.GetStockPrice(stockName, new DateTime(2012, 1, 10, 15, 50, 0, 0)));
		}

		[Test()]
		public void Ankica_TestCreateIndex_Simple()
		{
			_stockExchange = Factory.CreateStockExchange();
			string firstIndexName = "DOW JONES";
			_stockExchange.CreateIndex(firstIndexName, IndexTypes.AVERAGE);
			string secondIndexName = "S&P";
			_stockExchange.CreateIndex(secondIndexName, IndexTypes.WEIGHTED);
			Assert.AreEqual(2, _stockExchange.NumberOfIndices());
			Assert.IsTrue(_stockExchange.IndexExists(firstIndexName));
			Assert.IsTrue(_stockExchange.IndexExists(secondIndexName));
			Assert.IsFalse(_stockExchange.IndexExists("AB"));
		}

		[Test()]
		public void Ankica_TestGetIndexValue_Weighted()
		{
			_stockExchange = Factory.CreateStockExchange();
			string firstStockName = "IBM";
			_stockExchange.ListStock(firstStockName, 1, 100m, new DateTime(2012, 1, 11, 14, 10, 00, 00));
			string secondStockName = "MSFT";
			_stockExchange.ListStock(secondStockName, 2, 200m, new DateTime(2012, 1, 11, 14, 10, 00, 00));

			string indexName = "DOW JONES";
			_stockExchange.CreateIndex(indexName, IndexTypes.WEIGHTED);

			_stockExchange.AddStockToIndex(indexName, firstStockName);
			Assert.AreEqual(100m, _stockExchange.GetIndexValue(indexName, new DateTime(2012, 1, 11, 14, 11, 00, 00)));
			_stockExchange.AddStockToIndex(indexName, secondStockName);
			Assert.AreEqual(180m, _stockExchange.GetIndexValue(indexName, new DateTime(2012, 1, 11, 14, 11, 00, 00)));

			Assert.AreEqual(180m, _stockExchange.GetIndexValue(indexName, new DateTime(2012, 1, 11, 14, 11, 00, 00)));
		}

		[Test()]
		public void Ankica_TestAddStockToPortfolio_SameStock()
		{
			_stockExchange = Factory.CreateStockExchange();
			string stockName = "IBM";
			_stockExchange.ListStock(stockName, 5, 100m, DateTime.Now);

			string portfolioID = "P1";
			_stockExchange.CreatePortfolio(portfolioID);

			_stockExchange.AddStockToPortfolio(portfolioID, stockName, 1);
			_stockExchange.AddStockToPortfolio(portfolioID, stockName, 2);

			Assert.IsTrue(_stockExchange.IsStockPartOfPortfolio(portfolioID, stockName));
			Assert.AreEqual(1, _stockExchange.NumberOfStocksInPortfolio(portfolioID));
			Assert.AreEqual(3, _stockExchange.NumberOfSharesOfStockInPortfolio(portfolioID, stockName));
		}

		[Test()]
		public void Ankica_TestRemoveStockFromPortfolio_NumOfShares()
		{
			_stockExchange = Factory.CreateStockExchange();
			string firstStockName = "IBM";
			_stockExchange.ListStock(firstStockName, 5, 100m, DateTime.Now);
			string secondStockName = "MSFT";
			_stockExchange.ListStock(secondStockName, 5, 200m, DateTime.Now);

			string portfolioID = "P1";
			_stockExchange.CreatePortfolio(portfolioID);
			_stockExchange.AddStockToPortfolio(portfolioID, firstStockName, 4);
			_stockExchange.AddStockToPortfolio(portfolioID, secondStockName, 1);

			_stockExchange.RemoveStockFromPortfolio(portfolioID, firstStockName, 2);

			Assert.IsTrue(_stockExchange.IsStockPartOfPortfolio(portfolioID, firstStockName));
			Assert.IsTrue(_stockExchange.IsStockPartOfPortfolio(portfolioID, secondStockName));
			Assert.AreEqual(2, _stockExchange.NumberOfStocksInPortfolio(portfolioID));
			Assert.AreEqual(2, _stockExchange.NumberOfSharesOfStockInPortfolio(portfolioID, firstStockName));
			Assert.AreEqual(1, _stockExchange.NumberOfSharesOfStockInPortfolio(portfolioID, secondStockName));
		}

		[Test()]
		public void GIGATEST()
		{
			_stockExchange = Factory.CreateStockExchange();
			Assert.AreEqual(0, _stockExchange.NumberOfStocks());
			Assert.AreEqual(0, _stockExchange.NumberOfIndices());
			Assert.AreEqual(0, _stockExchange.NumberOfPortfolios());

			string stock1 = "Dionica1";
			string stock2 = "Dionica2";
			string stock3 = "Dionica3";
			string indeks1 = "Indeks1";
			string indeks2 = "Indeks2";

			_stockExchange.ListStock(stock1, 10, 100m, new DateTime(2013, 1, 1, 10, 00, 00));
			_stockExchange.ListStock(stock2, 20, 200m, new DateTime(2013, 1, 1, 10, 00, 00));
			//_stockExchange.ListStock("DionICa2", 30, 1m, new DateTime(2013, 1, 1, 10, 00, 00));
			//_stockExchange.ListStock(stock3, 30, 0m, new DateTime(2013, 1, 1, 10, 00, 00));
			//_stockExchange.ListStock(stock3, 30, -50m, new DateTime(2013, 1, 1, 10, 00, 00));
			//_stockExchange.ListStock(stock3, 0, 300m, new DateTime(2013, 1, 1, 10, 00, 00)); 
			//_stockExchange.ListStock(stock3, -30, 300m, new DateTime(2013, 1, 1, 10, 00, 00)); 
			//_stockExchange.ListStock(stock1, 10, 200m, new DateTime(2013, 1, 1, 10, 00, 00)); //ova dionica vec postoji
			_stockExchange.ListStock(stock3, 30, 300m, new DateTime(2013, 1, 1, 10, 00, 00));
			Assert.IsTrue(_stockExchange.StockExists("dioniCA1"));
			_stockExchange.SetStockPrice(stock1, new DateTime(2015, 1, 22, 10, 0, 0), 500m);
			//_stockExchange.SetStockPrice("DionicaNePostoji", new DateTime(2013, 1, 22, 10, 0, 0), 500m);
			//_stockExchange.SetStockPrice(stock1, new DateTime(2013, 1, 22, 10, 0, 0), 0m);
			Assert.AreEqual(100m, _stockExchange.GetStockPrice(stock1, DateTime.Now));
			Assert.AreEqual(100m, _stockExchange.GetInitialStockPrice(stock1));
			Assert.AreEqual(500m, _stockExchange.GetLastStockPrice(stock1));
			Assert.AreEqual(_stockExchange.NumberOfStocks(), 3);
			_stockExchange.DelistStock(stock1);
			Assert.AreEqual(_stockExchange.NumberOfStocks(), 2);
			Assert.IsFalse(_stockExchange.StockExists(stock1));
			//_stockExchange.SetStockPrice(stock1, new DateTime(2013, 1, 22, 10, 0, 0), 500m);
			_stockExchange.ListStock(stock1, 10, 100m, new DateTime(2013, 1, 1, 10, 00, 00));
			Assert.AreEqual(_stockExchange.NumberOfStocks(), 3);

			_stockExchange.CreateIndex(indeks1, IndexTypes.AVERAGE);
			_stockExchange.CreateIndex(indeks2, IndexTypes.WEIGHTED);
			//_stockExchange.CreateIndex("INDEKS1", IndexTypes.AVERAGE); //ovaj postoji vec
			Assert.AreEqual(_stockExchange.NumberOfIndices(), 2);
			Assert.AreEqual(0, _stockExchange.GetIndexValue(indeks1, DateTime.Now));
			Assert.IsTrue(_stockExchange.IndexExists(indeks2));
			Assert.IsTrue(_stockExchange.IndexExists("indeKS2"));
			_stockExchange.AddStockToIndex(indeks1, stock1);
			_stockExchange.AddStockToIndex(indeks2, stock2);
			_stockExchange.AddStockToIndex(indeks2, stock3);
			_stockExchange.AddStockToIndex(indeks2, stock1); // stock1 je u indeksu1

			Assert.IsTrue(_stockExchange.IsStockPartOfIndex(indeks1, stock1));
			Assert.IsTrue(_stockExchange.IsStockPartOfIndex(indeks2, stock2));
			Assert.IsTrue(_stockExchange.IsStockPartOfIndex(indeks2, stock3));
			Assert.IsTrue(_stockExchange.IsStockPartOfIndex(indeks2, stock1));
			Assert.IsFalse(_stockExchange.IsStockPartOfIndex(indeks1, stock3));

			_stockExchange.RemoveStockFromIndex(indeks2, stock2);
			_stockExchange.RemoveStockFromIndex(indeks2, stock1);
			//_stockExchange.RemoveStockFromIndex(indeks2, "bla");
			Assert.IsFalse(_stockExchange.IsStockPartOfIndex(indeks2, stock2));
			_stockExchange.AddStockToIndex(indeks1, stock2);
			Assert.IsTrue(_stockExchange.IsStockPartOfIndex(indeks1, stock2));


			Assert.IsTrue(_stockExchange.IsStockPartOfIndex(indeks1, stock1));
			Assert.IsTrue(_stockExchange.IsStockPartOfIndex(indeks1, stock2));
			Assert.IsFalse(_stockExchange.IsStockPartOfIndex(indeks1, stock3));

			Assert.IsFalse(_stockExchange.IsStockPartOfIndex(indeks2, stock1));
			Assert.IsFalse(_stockExchange.IsStockPartOfIndex(indeks2, stock2));
			Assert.IsTrue(_stockExchange.IsStockPartOfIndex(indeks2, stock3));

			Assert.AreEqual(_stockExchange.GetIndexValue(indeks1, DateTime.Now), 150m);
			Assert.AreEqual(_stockExchange.GetIndexValue(indeks2, DateTime.Now), 300m); // tu pada
			_stockExchange.SetStockPrice(stock1, DateTime.Now, 1000m);
			Assert.AreEqual(_stockExchange.GetIndexValue(indeks1, DateTime.Now), 600m);
			_stockExchange.RemoveStockFromIndex(indeks1, stock2);
			Assert.AreEqual(_stockExchange.GetIndexValue(indeks1, DateTime.Now), 1000m);

			Assert.IsTrue(_stockExchange.IsStockPartOfIndex(indeks1, stock1));
			_stockExchange.RemoveStockFromIndex(indeks1, stock1);

			_stockExchange.DelistStock(stock1);
			//_stockExchange.DelistStock("nekatamo");
			Assert.IsFalse(_stockExchange.IsStockPartOfIndex(indeks1, stock2));
			//Assert.IsFalse(_stockExchange.IsStockPartOfIndex(indeks1, stock1));
			Assert.IsFalse(_stockExchange.IsStockPartOfIndex(indeks1, stock3));
			Assert.AreEqual(0, _stockExchange.NumberOfStocksInIndex(indeks1));

			Assert.AreEqual(_stockExchange.GetIndexValue(indeks1, DateTime.Now), 0m);

			Assert.AreEqual(_stockExchange.NumberOfStocksInIndex(indeks1), 0);
			Assert.AreEqual(_stockExchange.NumberOfStocksInIndex(indeks2), 1);
			//Assert.AreEqual(_stockExchange.NumberOfStocksInIndex("ASDAD"), 0);

			_stockExchange.DelistStock(stock2);
			_stockExchange.DelistStock(stock3);
			_stockExchange.ListStock(stock1, 10, 100m, new DateTime(2013, 1, 1, 20, 00, 00));
			_stockExchange.ListStock(stock2, 20, 200m, new DateTime(2013, 1, 1, 20, 00, 00));
			_stockExchange.ListStock(stock3, 30, 300m, new DateTime(2013, 1, 1, 20, 00, 00));

			string portfelj1 = "Portfelj1";
			string portfelj2 = "Portfelj2";

			_stockExchange.CreatePortfolio(portfelj1);
			_stockExchange.CreatePortfolio(portfelj2);
			//_stockExchange.CreatePortfolio("Portfelj2");
			Assert.AreEqual(_stockExchange.NumberOfPortfolios(), 2);
			Assert.IsTrue(_stockExchange.PortfolioExists(portfelj2));
			Assert.IsTrue(_stockExchange.PortfolioExists("Portfelj1"));
			Assert.IsFalse(_stockExchange.PortfolioExists("portfelj1"));
			Assert.IsFalse(_stockExchange.PortfolioExists("NekiTamo"));
			_stockExchange.AddStockToPortfolio(portfelj1, stock1, 5);
			//_stockExchange.AddStockToPortfolio("Nema", stock1, 4);
			//_stockExchange.AddStockToPortfolio(portfelj1, "Nema", 4);
			//_stockExchange.AddStockToPortfolio(portfelj1, stock1, 0);
			//_stockExchange.AddStockToPortfolio(portfelj1, stock1, -4);
			_stockExchange.AddStockToPortfolio(portfelj1, stock1, 4);
			//_stockExchange.AddStockToPortfolio(portfelj1, stock1, 3);//nema vise slobodnih dionica
			_stockExchange.AddStockToPortfolio(portfelj2, stock1, 1);
			_stockExchange.AddStockToPortfolio(portfelj2, stock2, 10);
			_stockExchange.AddStockToPortfolio(portfelj2, stock3, 20);
			Assert.AreEqual(_stockExchange.NumberOfStocksInPortfolio(portfelj1), 1);
			//Assert.AreEqual(_stockExchange.NumberOfStocksInPortfolio("portfelj1"), 0);//"portfelj1" ne postoji
			Assert.AreEqual(_stockExchange.NumberOfStocksInPortfolio(portfelj2), 3);
			Assert.AreEqual(_stockExchange.NumberOfSharesOfStockInPortfolio(portfelj1, stock1), 9);
			//Assert.AreEqual(_stockExchange.NumberOfSharesOfStockInPortfolio(portfelj1, stock2), 0);
			Assert.AreEqual(_stockExchange.NumberOfSharesOfStockInPortfolio(portfelj2, stock1), 1);
			Assert.AreEqual(_stockExchange.NumberOfSharesOfStockInPortfolio(portfelj2, stock3), 20);
			//Assert.AreEqual(_stockExchange.NumberOfSharesOfStockInPortfolio(portfelj2, "nope"), 0);
			//Assert.AreEqual(_stockExchange.NumberOfSharesOfStockInPortfolio("aaa", stock3), 0);

			Assert.AreEqual(_stockExchange.GetPortfolioValue(portfelj2, DateTime.Now), 8100);
			_stockExchange.SetStockPrice(stock1, DateTime.Now, 101m);
			Assert.AreEqual(_stockExchange.GetPortfolioValue(portfelj2, DateTime.Now), 8101);
			_stockExchange.RemoveStockFromPortfolio(portfelj2, stock1, 1);
			//_stockExchange.RemoveStockFromPortfolio("poRTfelj2", stock1, 1);
			//_stockExchange.RemoveStockFromPortfolio(portfelj2, "nedamiseovoraditaaaaaaa", 1);
			Assert.AreEqual(_stockExchange.NumberOfStocksInPortfolio(portfelj2), 2);
			Assert.IsFalse(_stockExchange.IsStockPartOfPortfolio(portfelj2, stock1));
			//Assert.IsFalse(_stockExchange.IsStockPartOfPortfolio(portfelj2, "nepostoji"));
			//Assert.IsFalse(_stockExchange.IsStockPartOfPortfolio("nema_me", stock1));
			Assert.AreEqual(_stockExchange.GetPortfolioValue(portfelj2, DateTime.Now), 8000);

			_stockExchange.RemoveStockFromPortfolio(portfelj2, stock3, 10);
			Assert.AreEqual(_stockExchange.NumberOfStocksInPortfolio(portfelj2), 2);
			Assert.IsTrue(_stockExchange.IsStockPartOfPortfolio(portfelj2, stock3));
			Assert.AreEqual(_stockExchange.GetPortfolioValue(portfelj2, DateTime.Now), 5000);

			_stockExchange.RemoveStockFromPortfolio(portfelj2, stock3);
			Assert.AreEqual(_stockExchange.NumberOfStocksInPortfolio(portfelj2), 1);
			Assert.IsFalse(_stockExchange.IsStockPartOfPortfolio(portfelj2, stock3));
			Assert.AreEqual(_stockExchange.GetPortfolioValue(portfelj2, DateTime.Now), 2000);

			_stockExchange.DelistStock(stock2);
			Assert.AreEqual(_stockExchange.NumberOfStocksInPortfolio(portfelj2), 0);
			//Assert.IsFalse(_stockExchange.IsStockPartOfPortfolio(portfelj2, stock2)); //dionica ne postoji
			Assert.AreEqual(_stockExchange.GetPortfolioValue(portfelj2, DateTime.Now), 0);


			_stockExchange.DelistStock(stock1);
			_stockExchange.DelistStock(stock3);
			_stockExchange.ListStock(stock1, 10, 100m, new DateTime(2013, 1, 1, 20, 00, 00));
			_stockExchange.ListStock(stock2, 20, 200m, new DateTime(2013, 1, 1, 20, 00, 00));
			_stockExchange.ListStock(stock3, 30, 300m, new DateTime(2013, 1, 1, 20, 00, 00));
			_stockExchange.AddStockToPortfolio(portfelj1, stock1, 10);
			_stockExchange.AddStockToPortfolio(portfelj1, stock2, 10);
			_stockExchange.AddStockToPortfolio(portfelj1, stock3, 10);
			Assert.AreEqual(_stockExchange.GetPortfolioPercentChangeInValueForMonth(portfelj2, 2013, 3), 0);

			_stockExchange.SetStockPrice(stock1, new DateTime(2013, 4, 30, 23, 59, 59, 999), 150m);
			_stockExchange.SetStockPrice(stock2, new DateTime(2013, 4, 30, 23, 59, 59, 999), 300m);
			_stockExchange.SetStockPrice(stock3, new DateTime(2013, 4, 30, 23, 59, 59, 999), 450m);

			Assert.AreEqual(50m, _stockExchange.GetPortfolioPercentChangeInValueForMonth(portfelj1, 2013, 4));
			Assert.AreEqual(0m, _stockExchange.GetPortfolioPercentChangeInValueForMonth(portfelj1, 2013, 5));

		}
		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Ankica_TestAddStockToIndex_Complicated()
		{
			// Dodaju se dionice u index, onda se jedna obriše s burze i pokuša se dohvatiti u indeksu 
			_stockExchange = Factory.CreateStockExchange();
			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000000, 10m, DateTime.Now);
			string dionica2 = "Dionica2";
			_stockExchange.ListStock(dionica2, 1000000, 10m, DateTime.Now);

			string indeks1 = "indeks1";
			_stockExchange.CreateIndex(indeks1, IndexTypes.WEIGHTED);

			_stockExchange.AddStockToIndex(indeks1, dionica1);
			_stockExchange.AddStockToIndex(indeks1, dionica2);

			Assert.IsTrue(_stockExchange.IsStockPartOfIndex(indeks1, dionica1));
			Assert.IsTrue(_stockExchange.IsStockPartOfIndex(indeks1, dionica2));
			Assert.AreEqual(2, _stockExchange.NumberOfStocksInIndex(indeks1));

			_stockExchange.DelistStock(dionica1);

			_stockExchange.RemoveStockFromIndex(indeks1, dionica1);             // treba baciti exception
		}


		[Test()]
		public void Ankica_TestAddStockToIndex_MoreIndices()
		{
			// Dodaje se ista dionica u različite indexe 
			_stockExchange = Factory.CreateStockExchange();
			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000000, 10m, DateTime.Now);

			string indeks1 = "indeks1";
			_stockExchange.CreateIndex(indeks1, IndexTypes.WEIGHTED);
			string indeks2 = "indeks2";
			_stockExchange.CreateIndex(indeks2, IndexTypes.AVERAGE);

			_stockExchange.AddStockToIndex(indeks1, dionica1);
			_stockExchange.AddStockToIndex(indeks2, dionica1);

			Assert.IsTrue(_stockExchange.IsStockPartOfIndex(indeks1, dionica1));
			Assert.IsTrue(_stockExchange.IsStockPartOfIndex(indeks2, dionica1));
			Assert.AreEqual(1, _stockExchange.NumberOfStocksInIndex(indeks1));
			Assert.AreEqual(1, _stockExchange.NumberOfStocksInIndex(indeks2));
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Ankica_TestAddStockToIndex_NoIndex()
		{
			// Dodaju se dionice u index koji ne postoji na burzi
			_stockExchange = Factory.CreateStockExchange();
			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000000, 10m, DateTime.Now);
			string dionica2 = "Dionica2";
			_stockExchange.ListStock(dionica2, 1000000, 10m, DateTime.Now);

			string indeks1 = "IndeksKojiNePostoji";

			_stockExchange.AddStockToIndex("IndeksKojiNePostoji", dionica1);
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Ankica_TestAddStockToIndex_NoStock()
		{
			// Dodaju se dionice koje ne postoje na burzi u index koji postoji
			_stockExchange = Factory.CreateStockExchange();
			string indeks1 = "indeks1";
			_stockExchange.CreateIndex(indeks1, IndexTypes.WEIGHTED);

			_stockExchange.AddStockToIndex(indeks1, "dionicaKojaNePostoji");

		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Ankica_TestAddStockToIndex_SameStock()
		{
			// Dodaje se ista dionica više puta u index
			_stockExchange = Factory.CreateStockExchange();
			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000000, 10m, DateTime.Now);

			string indeks1 = "indeks1";
			_stockExchange.CreateIndex(indeks1, IndexTypes.WEIGHTED);

			_stockExchange.AddStockToIndex(indeks1, dionica1);
			_stockExchange.AddStockToIndex(indeks1, dionica1);
		}



		[Test()]
		public void Ankica_TestAddStockToPortfolio_Complicated()
		{
			// Dodaju se dionice u portfelj, onda se jedna obriše s burze i pokuša se dohvatiti u portfelju
			_stockExchange = Factory.CreateStockExchange();
			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000000, 10m, DateTime.Now);
			string dionica2 = "Dionica2";
			_stockExchange.ListStock(dionica2, 1000000, 10m, DateTime.Now);

			string portfelj1 = "portfelj1";
			_stockExchange.CreatePortfolio(portfelj1);
			_stockExchange.AddStockToPortfolio(portfelj1, dionica1, 1);
			_stockExchange.AddStockToPortfolio(portfelj1, dionica2, 1);

			Assert.IsTrue(_stockExchange.IsStockPartOfPortfolio(portfelj1, dionica1));
			Assert.IsTrue(_stockExchange.IsStockPartOfPortfolio(portfelj1, dionica2));
			Assert.AreEqual(2, _stockExchange.NumberOfStocksInPortfolio(portfelj1));

			_stockExchange.DelistStock(dionica1);

			Assert.IsFalse(_stockExchange.IsStockPartOfPortfolio(portfelj1, dionica1));
			Assert.IsTrue(_stockExchange.IsStockPartOfPortfolio(portfelj1, dionica2));

			_stockExchange.DelistStock(dionica2);                       // dodano u test, provjera obriše li se portfelj
			Assert.IsTrue(_stockExchange.PortfolioExists(portfelj1));    //nakon što se izbrišu sve dionice iz njega (ili s burze)
		}

		[Test()]
		public void Ankica_TestAddStockToPortfolio_GreaterThenNumOfShares()
		{
			// Dodaje se ista dionica više puta u portfelj - ukupno više od postojećeg broja 
			_stockExchange = Factory.CreateStockExchange();
			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 100, 10m, DateTime.Now);

			string portfelj1 = "portfelj1";
			_stockExchange.CreatePortfolio(portfelj1);

			_stockExchange.AddStockToPortfolio(portfelj1, dionica1, 50);
			//_stockExchange.AddStockToPortfolio(portfelj1, dionica1, 150);   // previše ih dodamo, treba ih dodati još 50 (ukupno ih mora biti 100)

			Assert.IsTrue(_stockExchange.IsStockPartOfPortfolio(portfelj1, dionica1));
			Assert.AreEqual(50, _stockExchange.NumberOfSharesOfStockInPortfolio(portfelj1, dionica1));
		}

		[Test()]
		public void Ankica_TestAddStockToPortfolio_MorePortfolios()
		{
			// Dodaje se ista dionica u različiti portfelj
			_stockExchange = Factory.CreateStockExchange();
			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 100, 10m, DateTime.Now);

			string portfelj1 = "portfelj1";
			_stockExchange.CreatePortfolio(portfelj1);
			string portfelj2 = "portfelj2";
			_stockExchange.CreatePortfolio(portfelj2);

			_stockExchange.AddStockToPortfolio(portfelj1, dionica1, 50);
			_stockExchange.AddStockToPortfolio(portfelj2, dionica1, 30);

			Assert.IsTrue(_stockExchange.IsStockPartOfPortfolio(portfelj1, dionica1));
			Assert.IsTrue(_stockExchange.IsStockPartOfPortfolio(portfelj2, dionica1));

			Assert.AreEqual(50, _stockExchange.NumberOfSharesOfStockInPortfolio(portfelj1, dionica1));
			Assert.AreEqual(30, _stockExchange.NumberOfSharesOfStockInPortfolio(portfelj2, dionica1));
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Ankica_TestAddStockToPortfolio_NegativeNumberOfShares()
		{
			// dodaje se neispravan broj dionica u portfelj
			_stockExchange = Factory.CreateStockExchange();
			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 100, 10m, DateTime.Now);

			string portfelj1 = "portfelj1";
			_stockExchange.CreatePortfolio(portfelj1);

			_stockExchange.AddStockToPortfolio(portfelj1, dionica1, -100);
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Ankica_TestAddStockToPortfolio_NoPortfolio()
		{
			// Dodaju se dionice u portfelj koji ne postoji na burzi
			_stockExchange = Factory.CreateStockExchange();
			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 100, 10m, DateTime.Now);

			_stockExchange.AddStockToPortfolio("PortfeljKojiNePostoji", dionica1, 100);
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Ankica_TestAddStockToPortfolio_NoStock()
		{
			// Dodaju se dionice koje ne postoje na burzi u portfelj koji postoji
			_stockExchange = Factory.CreateStockExchange();
			string portfelj1 = "portfelj1";
			_stockExchange.CreatePortfolio(portfelj1);

			_stockExchange.AddStockToPortfolio(portfelj1, "NepostojeceDionice", 100);
		}

		[Test()]
		public void Ankica_TestAddStockToPortfolio_Simple()
		{
			// Dodaju se dionice u portfelj
			_stockExchange = Factory.CreateStockExchange();
			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 100, 10m, DateTime.Now);
			string dionica2 = "Dionica2";
			_stockExchange.ListStock(dionica2, 100, 10m, DateTime.Now);

			string portfelj1 = "portfelj1";
			_stockExchange.CreatePortfolio(portfelj1);

			_stockExchange.AddStockToPortfolio(portfelj1, dionica1, 50);
			_stockExchange.AddStockToPortfolio(portfelj1, dionica2, 50);

			Assert.IsTrue(_stockExchange.IsStockPartOfPortfolio(portfelj1, dionica1));
			Assert.IsTrue(_stockExchange.IsStockPartOfPortfolio(portfelj1, dionica2));

			Assert.AreEqual(2, _stockExchange.NumberOfStocksInPortfolio(portfelj1));
			Assert.AreEqual(50, _stockExchange.NumberOfSharesOfStockInPortfolio(portfelj1, dionica1));
			Assert.AreEqual(50, _stockExchange.NumberOfSharesOfStockInPortfolio(portfelj1, dionica2));
		}

		[Test()]
		public void Ankica_TestAddStockToPortfolios_GreaterThenNumOfShares()
		{
			// Dodaje se ista dionica u različiti portfelj - ukupno više od postojećeg broja 
			_stockExchange = Factory.CreateStockExchange();
			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 100, 10m, DateTime.Now);

			string portfelj1 = "portfelj1";
			_stockExchange.CreatePortfolio(portfelj1);
			string portfelj2 = "portfelj2";
			_stockExchange.CreatePortfolio(portfelj2);

			_stockExchange.AddStockToPortfolio(portfelj1, dionica1, 50);
			//_stockExchange.AddStockToPortfolio(portfelj2, dionica1, 150);   // OPREZ!!!!  u portfelju 2 ih treba biti samo 50 (jer ukupno u svim portfeljima mora biti 100)

			Assert.IsTrue(_stockExchange.IsStockPartOfPortfolio(portfelj1, dionica1));
			Assert.IsFalse(_stockExchange.IsStockPartOfPortfolio(portfelj2, dionica1));

			Assert.AreEqual(1, _stockExchange.NumberOfStocksInPortfolio(portfelj1));
			Assert.AreEqual(0, _stockExchange.NumberOfStocksInPortfolio(portfelj2));
			Assert.AreEqual(50, _stockExchange.NumberOfSharesOfStockInPortfolio(portfelj1, dionica1));
			Assert.AreEqual(0, _stockExchange.NumberOfSharesOfStockInPortfolio(portfelj2, dionica1));
		}




		// Ankica_TestComplicatedChanges_Values




		// Ankica_TestCreateIndex_IllegalTypeNegative





		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Ankica_TestCreateIndex_SameNameAlreadyExists()
		{
			// Dodaje se indeks imena koje već postoji na burzi, ali drugog tipa 
			_stockExchange = Factory.CreateStockExchange();
			string indeks1 = "indeks1";
			_stockExchange.CreateIndex(indeks1, IndexTypes.AVERAGE);
			_stockExchange.CreateIndex(indeks1, IndexTypes.WEIGHTED);
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Ankica_TestCreateIndex_SameNameAndTypeAlreadyExists()
		{
			// Dodaje se indeks koji već postoji na burzi
			_stockExchange = Factory.CreateStockExchange();
			string indeks1 = "indeks1";
			_stockExchange.CreateIndex(indeks1, IndexTypes.AVERAGE);
			_stockExchange.CreateIndex(indeks1, IndexTypes.AVERAGE);
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Ankica_TestCreateIndex_SimilarNameAlreadyExists()
		{
			// Dodaje se indeks koja već postoji na burzi ali s promjenom velika/mala slova u imenu 
			_stockExchange = Factory.CreateStockExchange();
			_stockExchange.CreateIndex("abc", IndexTypes.AVERAGE);
			_stockExchange.CreateIndex("ABC", IndexTypes.AVERAGE);
		}

		[Test()]
		public void Ankica_TestCreateIPortfolio_Simple()
		{
			// Dodaje se par portfelja i provjerava postoje li na burzi 
			_stockExchange = Factory.CreateStockExchange();
			string portfelj1 = "portfelj1";
			_stockExchange.CreatePortfolio(portfelj1);
			string portfelj2 = "portfelj2";
			_stockExchange.CreatePortfolio(portfelj2);

			Assert.IsTrue(_stockExchange.PortfolioExists(portfelj1));
			Assert.IsTrue(_stockExchange.PortfolioExists(portfelj2));
			Assert.AreEqual(2, _stockExchange.NumberOfPortfolios());
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Ankica_TestCreatePortfolio_SameIdAlreadyExists()
		{
			// Dodaje se portfelj koji već postoji na burzi 
			_stockExchange = Factory.CreateStockExchange();
			string portfelj1 = "portfelj1";
			_stockExchange.CreatePortfolio(portfelj1);
			_stockExchange.CreatePortfolio(portfelj1);

		}

		[Test()]
		public void Ankica_TestCreatePortfolio_SimilarNameAlreadyExists()
		{
			// Dodaje se portfelj sličnog imena 
			_stockExchange = Factory.CreateStockExchange();
			string portfelj1 = "portfelj1";
			_stockExchange.CreatePortfolio(portfelj1);
			string portfelj2 = "Portfelj1";
			_stockExchange.CreatePortfolio(portfelj2);

			Assert.IsTrue(_stockExchange.PortfolioExists(portfelj1));
			Assert.IsTrue(_stockExchange.PortfolioExists(portfelj2));
			Assert.AreEqual(2, _stockExchange.NumberOfPortfolios());

		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Ankica_TestDelistStock_EmptyStockExchange()
		{
			// Pokušaj micanja dionice s burze koja nema niti jednu dionicu 
			_stockExchange = Factory.CreateStockExchange();
			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 0, 10m, DateTime.Now);

			// ak se ne varam, već bi kod dodavanja trebalo baciti exception
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Ankica_TestDelistStock_NotExist()
		{
			// Pokušaj micanja nepostojeće dionice s burze
			_stockExchange = Factory.CreateStockExchange();
			_stockExchange.DelistStock("nepostojecaDionica");
		}

		[Test()]
		public void Ankica_TestDelistStock_Simple()
		{
			// Postojeća dionica miče se s burze
			_stockExchange = Factory.CreateStockExchange();
			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000000, 10m, DateTime.Now);

			Assert.IsTrue(_stockExchange.StockExists(dionica1));
			Assert.AreEqual(1, _stockExchange.NumberOfStocks());

			_stockExchange.DelistStock(dionica1);

			Assert.IsFalse(_stockExchange.StockExists(dionica1));
			Assert.AreEqual(0, _stockExchange.NumberOfStocks());
		}

		[Test()]
		public void Ankica_TestGetIndexValue_AfterDelistingStock()
		{
			// Provjera izračuna vrijednosti portfelja nakon brisanja dionice s burze
			_stockExchange = Factory.CreateStockExchange();
			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 100, 10m, DateTime.Now);
			string dionica2 = "Dionica2";
			_stockExchange.ListStock(dionica2, 100, 20m, DateTime.Now);


			string portfelj1 = "portfelj1";
			_stockExchange.CreatePortfolio(portfelj1);
			_stockExchange.AddStockToPortfolio(portfelj1, dionica1, 10);
			_stockExchange.AddStockToPortfolio(portfelj1, dionica2, 10);

			Assert.AreEqual(300, _stockExchange.GetPortfolioValue(portfelj1, DateTime.Now));

			_stockExchange.DelistStock(dionica1);

			Assert.AreEqual(200, _stockExchange.GetPortfolioValue(portfelj1, DateTime.Now));
		}

		[Test()]
		public void Ankica_TestGetIndexValue_Average()
		{
			// Provjera izračuna AverageIndexa 
			_stockExchange = Factory.CreateStockExchange();
			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000, 100m, DateTime.Now);
			string dionica2 = "Dionica2";
			_stockExchange.ListStock(dionica2, 1000, 200m, DateTime.Now);

			string indeks1 = "indeks1";
			_stockExchange.CreateIndex(indeks1, IndexTypes.AVERAGE);

			_stockExchange.AddStockToIndex(indeks1, dionica1);
			_stockExchange.AddStockToIndex(indeks1, dionica2);

			Assert.AreEqual(150, _stockExchange.GetIndexValue(indeks1, DateTime.Now));

		}

		[Test()]
		public void Ankica_TestGetIndexValue_AverageAfterDelistingStock()
		{
			// Provjera izračuna AverageIndeksa nakon brisanja dionice s burze
			_stockExchange = Factory.CreateStockExchange();
			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000, 100m, DateTime.Now);
			string dionica2 = "Dionica2";
			_stockExchange.ListStock(dionica2, 1000, 200m, DateTime.Now);

			string indeks1 = "indeks1";
			_stockExchange.CreateIndex(indeks1, IndexTypes.AVERAGE);

			_stockExchange.AddStockToIndex(indeks1, dionica1);
			_stockExchange.AddStockToIndex(indeks1, dionica2);

			Assert.AreEqual(150, _stockExchange.GetIndexValue(indeks1, DateTime.Now));

			_stockExchange.DelistStock(dionica2);

			Assert.AreEqual(100, _stockExchange.GetIndexValue(indeks1, DateTime.Now));
		}

		[Test()]
		public void Ankica_TestGetIndexValue_AverageAfterPriceChange()
		{
			// Provjera izračuna AverageIndexa nakon promijene cijene
			_stockExchange = Factory.CreateStockExchange();
			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000, 100m, new DateTime(2014, 1, 1, 0, 0, 0));
			string dionica2 = "Dionica2";
			_stockExchange.ListStock(dionica2, 1000, 200m, new DateTime(2014, 1, 1, 0, 0, 0));

			string indeks1 = "indeks1";
			_stockExchange.CreateIndex(indeks1, IndexTypes.AVERAGE);

			_stockExchange.AddStockToIndex(indeks1, dionica1);
			_stockExchange.AddStockToIndex(indeks1, dionica2);

			Assert.AreEqual(150, _stockExchange.GetIndexValue(indeks1, new DateTime(2014, 1, 2, 0, 0, 0)));

			_stockExchange.SetStockPrice(dionica2, new DateTime(2014, 1, 3, 0, 0, 0), 300m);

			Assert.AreEqual(200, _stockExchange.GetIndexValue(indeks1, new DateTime(2014, 1, 4, 0, 0, 0)));
		}

		[Test()]
		public void Ankica_TestGetIndexValue_AverageAfterRemovingStock()
		{
			// Provjera izračuna AverageIndeksa nakon brisanja dionice iz indeksa 
			_stockExchange = Factory.CreateStockExchange();
			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000, 100m, DateTime.Now);
			string dionica2 = "Dionica2";
			_stockExchange.ListStock(dionica2, 1000, 200m, DateTime.Now);

			string indeks1 = "indeks1";
			_stockExchange.CreateIndex(indeks1, IndexTypes.AVERAGE);

			_stockExchange.AddStockToIndex(indeks1, dionica1);
			_stockExchange.AddStockToIndex(indeks1, dionica2);

			Assert.AreEqual(150, _stockExchange.GetIndexValue(indeks1, DateTime.Now));

			_stockExchange.RemoveStockFromIndex(indeks1, dionica2);

			Assert.AreEqual(100, _stockExchange.GetIndexValue(indeks1, DateTime.Now));
		}

		// Ankica_TestGetIndexValue_Begining 
		[Test()]
		public void Ankica_TestGetIndexValue_Begining()
		{
			// Postavljanje cijena u trenucima i dohvaćanje početne
			_stockExchange = Factory.CreateStockExchange();
			_stockExchange.CreateIndex("index", IndexTypes.AVERAGE);
			Assert.AreEqual(0m, _stockExchange.GetIndexValue("index", DateTime.Now));
		}

		// Ankica_TestGetIndexValue_WeightedDecimal

		[Test()]
		public void Ankica_TestGetInitialStockPrice_RandomPrices()
		{
			// Postavljanje cijena u trenucima i dohvaćanje početne
			_stockExchange = Factory.CreateStockExchange();
			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000, 100m, new DateTime(2013, 1, 1, 1, 0, 0, 0));

			_stockExchange.SetStockPrice(dionica1, new DateTime(2014, 1, 1, 1, 0, 0, 0), 200m);      // cijena nakon
			_stockExchange.SetStockPrice(dionica1, new DateTime(2012, 1, 1, 1, 0, 0, 0), 300m);      // cijena prije - trebala bi biti inicijalna jer je najstarija

			Assert.AreEqual(300m, _stockExchange.GetInitialStockPrice(dionica1));
		}

		[Test()]
		public void Ankica_TestGetLastStockPrice_RandomPrices()
		{
			// Postavljanje cijena u trenucima i dohvaćanje zadnje
			_stockExchange = Factory.CreateStockExchange();
			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000, 100m, new DateTime(2013, 1, 1, 1, 0, 0, 0));

			_stockExchange.SetStockPrice(dionica1, new DateTime(2014, 1, 1, 1, 0, 0, 0), 200m);      // cijena nakon - najnovija/zadnja
			_stockExchange.SetStockPrice(dionica1, new DateTime(2012, 1, 1, 1, 0, 0, 0), 300m);      // cijena prije

			Assert.AreEqual(200m, _stockExchange.GetLastStockPrice(dionica1));
		}

		// Ankica_TestGetPortfolioPercentChangeInValueForMonth_Complicated - rijeseno  u golemom

		// Ankica_TestGetPortfolioPercentChangeInValueForMonth_InitialPrices
		[Test()]
		public void Ankica_TestGetPortfolioPercentChangeInValueForMonth_InitialPrices()
		{
			// provjera izračuna postotka mjesečne promjene
			_stockExchange = Factory.CreateStockExchange();
			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000, 100m, new DateTime(2014, 1, 1, 0, 0, 0, 0));       // 1.1.2014. 0:00 100kn

			string portfolio1 = "portfolio1";
			_stockExchange.CreatePortfolio(portfolio1);

			_stockExchange.AddStockToPortfolio(portfolio1, dionica1, 100);

			Assert.AreEqual(0, _stockExchange.GetPortfolioPercentChangeInValueForMonth(portfolio1, 2014, 1));  // 1. mjesec 2014.
		}

		// Ankica_TestGetPortfolioPercentChangeInValueForMonth_PriceChanges - rijeseno u golemom






		[Test()]
		public void Ankica_TestGetPortfolioPercentChangeInValueForMonth_Simple()
		{
			// provjera izračuna postotka mjesečne promjene
			_stockExchange = Factory.CreateStockExchange();
			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000, 100m, new DateTime(2014, 1, 1, 0, 0, 0, 0));       // 1.1.2014. 0:00 100kn

			_stockExchange.SetStockPrice(dionica1, new DateTime(2014, 1, 31, 0, 0, 0, 0), 150);        // 15.1.2014. 0:00 150kn (+50%)

			string portfolio1 = "portfolio1";
			_stockExchange.CreatePortfolio(portfolio1);

			_stockExchange.AddStockToPortfolio(portfolio1, dionica1, 100);

			Assert.AreEqual(50, _stockExchange.GetPortfolioPercentChangeInValueForMonth(portfolio1, 2014, 1));  // 1. mjesec 2014.
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Ankica_TestGetPortfolioPercentChangeInValueForMonth_WrongDate()
		{
			// provjera izračuna postotka mjesečne promjene uz pogrešnu vrijednost datuma 
			_stockExchange = Factory.CreateStockExchange();
			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000, 100m, new DateTime(2014, 1, 1, 0, 0, 0, 0));       // 1.1.2014. 0:00 100kn

			_stockExchange.SetStockPrice(dionica1, new DateTime(2014, 1, 31, 0, 0, 0, 0), 150);        // 15.1.2014. 0:00 150kn (+50%)

			string portfolio1 = "portfolio1";
			_stockExchange.CreatePortfolio(portfolio1);

			_stockExchange.AddStockToPortfolio(portfolio1, dionica1, 100);

			Assert.AreEqual(50, _stockExchange.GetPortfolioPercentChangeInValueForMonth(portfolio1, 2014, 20));  // 20. mjesec 2014. (krivi datum)
		}

		[Test()]
		public void Ankica_TestGetPortfolioValue_AfterPriceChange()
		{
			// Provjera izračuna vrijednosti portfelja nakon promijene cijene 
			_stockExchange = Factory.CreateStockExchange();
			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000, 100m, new DateTime(2014, 1, 1, 0, 0, 0, 0));       // 1.1.2014. 0:00 100kn

			string portfolio1 = "portfolio1";
			_stockExchange.CreatePortfolio(portfolio1);

			_stockExchange.AddStockToPortfolio(portfolio1, dionica1, 10);

			Assert.AreEqual(1000, _stockExchange.GetPortfolioValue(portfolio1, new DateTime(2014, 3, 1, 0, 0, 0, 0)));  // 1.3.2014. 0:00   PROVJERA

			_stockExchange.SetStockPrice(dionica1, new DateTime(2014, 1, 31, 0, 0, 0, 0), 200);                         // 1.2.2014. 0:00 200kn

			Assert.AreEqual(2000, _stockExchange.GetPortfolioValue(portfolio1, new DateTime(2014, 3, 1, 0, 0, 0, 0)));  // 1.3.2014. 0:00
		}

		[Test()]
		public void Ankica_TestGetPortfolioValue_AfterRemovingStock()
		{
			_stockExchange = Factory.CreateStockExchange();
			// Provjera izračuna vrijednosta portfelja nakon brisanja dionice iz portfolia 

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000, 100m, new DateTime(2014, 1, 1, 0, 0, 0, 0));       // 1.1.2014. 0:00 100kn
			string dionica2 = "Dionica2";
			_stockExchange.ListStock(dionica2, 1000, 100m, new DateTime(2014, 1, 1, 0, 0, 0, 0));       // 1.1.2014. 0:00 100kn

			string portfolio1 = "portfolio1";
			_stockExchange.CreatePortfolio(portfolio1);

			_stockExchange.AddStockToPortfolio(portfolio1, dionica1, 10);
			_stockExchange.AddStockToPortfolio(portfolio1, dionica2, 10);

			Assert.AreEqual(2000, _stockExchange.GetPortfolioValue(portfolio1, new DateTime(2014, 3, 1, 0, 0, 0, 0)));  // 1.3.2014. 0:00   PROVJERA

			_stockExchange.RemoveStockFromPortfolio(portfolio1, dionica2);

			Assert.AreEqual(1000, _stockExchange.GetPortfolioValue(portfolio1, new DateTime(2014, 3, 1, 0, 0, 0, 0)));  // 1.3.2014. 0:00
		}

		[Test()]
		public void Ankica_TestGetPortfolioValue_Begining()
		{
			// Provjera izračuna vrijednosti portfelja na početku
			_stockExchange = Factory.CreateStockExchange();
			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000, 100m, new DateTime(2014, 1, 1, 0, 0, 0, 0));       // 1.1.2014. 0:00 100kn
			string dionica2 = "Dionica2";
			_stockExchange.ListStock(dionica2, 1000, 100m, new DateTime(2014, 1, 1, 0, 0, 0, 0));       // 1.1.2014. 0:00 100kn

			string portfolio1 = "portfolio1";
			_stockExchange.CreatePortfolio(portfolio1);

			_stockExchange.AddStockToPortfolio(portfolio1, dionica1, 10);
			_stockExchange.AddStockToPortfolio(portfolio1, dionica2, 10);

			Assert.AreEqual(2000, _stockExchange.GetPortfolioValue(portfolio1, new DateTime(2014, 3, 1, 0, 0, 0, 0)));  // 1.3.2014. 0:00   PROVJERA
		}







		// Ankica_TestGetPortfolioValue_Begining - rijeseno 
		[Test()]
		public void Ankica_TestGetPortfolioValue_BeginingNull()
		{
			_stockExchange = Factory.CreateStockExchange();
			// Briše se dionica iz nepostojećeg portfelja 
			string stock1 = "Dionica1";
			_stockExchange.ListStock(stock1, 10, 100m, new DateTime(2013, 1, 1, 10, 00, 00));

			string portfelj1 = "Portfelj1";
			_stockExchange.CreatePortfolio(portfelj1);
			Assert.AreEqual(_stockExchange.GetPortfolioValue(portfelj1, DateTime.Now), 0);
		}

		// Ankica_TestGetPortfolioValue_Sum - rijeseno

		// Ankica_TestGetStockPrice_BeforeAlPrices - acc dolje xD

		// Ankica_TestGetStockPrice_BeforeInitialPrice
		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Ankica_TestGetStockPrice_BeforeInitialPrice()
		{
			// Dodaje se dionica s negativnim brojem dionica
			_stockExchange = Factory.CreateStockExchange();
			_stockExchange.ListStock("IBM", 10, 10m, DateTime.Now);
			Assert.AreEqual(_stockExchange.GetStockPrice("ibM", DateTime.Now), 10m);
			_stockExchange.SetStockPrice("IBM", new DateTime(2014, 12, 14, 22, 22, 22, 999), 100m);
			Assert.AreEqual(_stockExchange.GetStockPrice("ibM", new DateTime(2014, 12, 14, 22, 22, 22, 999)), 100m);
			_stockExchange.SetStockPrice("IBM", new DateTime(2014, 12, 14, 21, 22, 22, 999), 200m);
			Assert.AreEqual(_stockExchange.GetStockPrice("IBM",new DateTime(2013,1,1,1,1,1,111)), 10m);
		}


		// Ankica_TestGetStockPrice_InitialPrice
		[Test()]
		public void Ankica_TestGetStockPrice_InitialPrice()
		{
			// Dodaje se dionica s negativnim brojem dionica
			_stockExchange = Factory.CreateStockExchange();
			_stockExchange.ListStock("IBM", 10, 10m, DateTime.Now);
			Assert.AreEqual(_stockExchange.GetStockPrice("ibM", DateTime.Now), 10m);
			_stockExchange.SetStockPrice("IBM", new DateTime(2015, 12, 14, 22, 22, 22, 999), 100m);
			Assert.AreEqual(_stockExchange.GetStockPrice("ibM", new DateTime(2015, 12, 14, 22, 22, 22, 999)), 100m);
			_stockExchange.SetStockPrice("IBM", new DateTime(2015, 12, 14, 21, 22, 22, 999), 200m);
			Assert.AreEqual(_stockExchange.GetInitialStockPrice("IBM"), 10m);
		}

		// Ankica_TestGetStockPrice_MorePrices
		// Ankica_TestGetStockPrice_LastPrice
		[Test()]
		public void Ankica_TestGetStockPrice_LastPrice()
		{
			// Dodaje se dionica s negativnim brojem dionica
			_stockExchange = Factory.CreateStockExchange();
			_stockExchange.ListStock("IBM", 10, 10m, DateTime.Now);
			Assert.AreEqual(_stockExchange.GetStockPrice("ibM", DateTime.Now), 10m);
			_stockExchange.SetStockPrice("IBM", new DateTime(2015, 12, 14, 22, 22, 22, 999), 100m);
			Assert.AreEqual(_stockExchange.GetStockPrice("ibM", new DateTime(2015, 12, 14, 22, 22, 22, 999)), 100m);
			_stockExchange.SetStockPrice("IBM", new DateTime(2015, 12, 14, 21, 22, 22, 999), 200m);
			Assert.AreEqual(_stockExchange.GetLastStockPrice("IBM"), 100m);
		}

		// Ankica_TestGetStockPrice_RandomPrices
		[Test()]
		public void Ankica_TestGetStockPrice_RandomPrices()
		{
			// Dodaje se dionica s negativnim brojem dionica
			_stockExchange = Factory.CreateStockExchange();
			_stockExchange.ListStock("IBM", 10, 10m, DateTime.Now);
			Assert.AreEqual(_stockExchange.GetStockPrice("ibM", DateTime.Now), 10m);
			_stockExchange.SetStockPrice("IBM", new DateTime(2014,12,14,22,22,22,999), 100m);
			Assert.AreEqual(_stockExchange.GetStockPrice("ibM", new DateTime(2014, 12, 14, 22, 22, 22, 999)), 100m);
		}

		// Ankica_TestGetStockPrice_SimilarName
		[Test()]
		public void Ankica_TestGetStockPrice_SimilarName()
		{
			// Dodaje se dionica s negativnim brojem dionica
			_stockExchange = Factory.CreateStockExchange();
			_stockExchange.ListStock("IBM", 10, 10m, DateTime.Now);
			Assert.AreEqual(_stockExchange.GetStockPrice("ibM", DateTime.Now), 10m);
		}


		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Ankica_TestListStock_IllegalNumberOfSharesNegative()
		{
			// Dodaje se dionica s negativnim brojem dionica
			_stockExchange = Factory.CreateStockExchange();
			_stockExchange.ListStock("IBM", -10, 10m, DateTime.Now);
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Ankica_TestListStock_IllegalNumberOfSharesNull()
		{
			// Dodaje se dionica s 0 dionica
			_stockExchange = Factory.CreateStockExchange();
			_stockExchange.ListStock("IBM", 0, 10m, DateTime.Now);
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Ankica_TestListStock_IllegalPriceNull()
		{
			_stockExchange = Factory.CreateStockExchange();
			_stockExchange.ListStock("IBM", 1000000, 0m, DateTime.Now);
		}

		// Ankica_TestListStock_SimilarNameAlreadyExists - već u golemom :D

		[Test()]
		public void Ankica_TestListStock_Simple()
		{
			_stockExchange = Factory.CreateStockExchange();
			Assert.AreEqual(0, _stockExchange.NumberOfStocks());
			string firstStockName = "IBM";
			_stockExchange.ListStock(firstStockName, 1000000, 10m, DateTime.Now);

			Assert.AreEqual(1, _stockExchange.NumberOfStocks());
			Assert.IsTrue(_stockExchange.StockExists(firstStockName));
			Assert.IsFalse(_stockExchange.StockExists("Bezveze"));

			string secondStockName = "MSFT";
			_stockExchange.ListStock(secondStockName, 100000, 15m, DateTime.Now);
			Assert.AreEqual(2, _stockExchange.NumberOfStocks());
			Assert.IsTrue(_stockExchange.StockExists(secondStockName));
		}

		// Ankica_TestNumberOfStocksInIndex_NoStocks - već u golemom


		// Ankica_TestNumberOfStocksInPortfolio_NoStocks - već u golemom


		// Ankica_TestRemoveStockFromIndex_MoreIndices - već u golemom




		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Ankica_TestRemoveStockFromIndex_NonExistingIndex()
		{
			_stockExchange = Factory.CreateStockExchange();
			// Briše se dionica iz nepostojećeg indeksa

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000000, 10m, DateTime.Now);

			_stockExchange.RemoveStockFromIndex("nepostojeciIndeks", dionica1);
		}

		// Ankica_TestRemoveStockFromIndex_NonExistingStock
		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Ankica_TestRemoveStockFromIndex_NonExistingStock()
		{
			_stockExchange = Factory.CreateStockExchange();
			// Briše se dionica iz nepostojećeg portfelja 
			string stock1 = "Dionica1";
			_stockExchange.ListStock(stock1, 10, 100m, new DateTime(2013, 1, 1, 10, 00, 00));

			string index = "Portfelj1";
			_stockExchange.CreateIndex(index, IndexTypes.AVERAGE);
			_stockExchange.AddStockToIndex(index, stock1);
			_stockExchange.RemoveStockFromIndex(index, "bla");
		}

		// Ankica_TestRemoveStockFromIndex_Twice
		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Ankica_TestRemoveStockFromIndex_Twice()
		{
			_stockExchange = Factory.CreateStockExchange();
			// Briše se dionica iz nepostojećeg portfelja 
			string stock1 = "Dionica1";
			_stockExchange.ListStock(stock1, 10, 100m, new DateTime(2013, 1, 1, 10, 00, 00));

			string index = "Portfelj1";
			_stockExchange.CreateIndex(index, IndexTypes.AVERAGE);
			_stockExchange.AddStockToIndex(index, stock1);
			_stockExchange.RemoveStockFromIndex(index, stock1);
			_stockExchange.RemoveStockFromIndex(index, stock1);
		}
		// Ankica_TestRemoveStockFromPortfolio_All
		[Test()]
		public void Ankica_TestRemoveStockFromPortfolio_All()
		{
			_stockExchange = Factory.CreateStockExchange();
			// Briše se dionica iz nepostojećeg portfelja 
			string stock1 = "Dionica1";
			_stockExchange.ListStock(stock1, 10, 100m, new DateTime(2013, 1, 1, 10, 00, 00));

			string portfelj1 = "Portfelj1";
			_stockExchange.CreatePortfolio(portfelj1);
			_stockExchange.AddStockToPortfolio(portfelj1, stock1, 1);
			_stockExchange.RemoveStockFromPortfolio(portfelj1, stock1, 1);
			Assert.AreEqual(_stockExchange.NumberOfSharesOfStockInPortfolio(portfelj1, stock1), 0);
		}

		// Ankica_TestRemoveStockFromPortfolio_AllTwice
		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Ankica_TestRemoveStockFromPortfolio_AllTwice()
		{
			_stockExchange = Factory.CreateStockExchange();
			// Briše se dionica iz nepostojećeg portfelja 
			string stock1 = "Dionica1";
			_stockExchange.ListStock(stock1, 10, 100m, new DateTime(2013, 1, 1, 10, 00, 00));

			string portfelj1 = "Portfelj1";
			_stockExchange.CreatePortfolio(portfelj1);
			_stockExchange.AddStockToPortfolio(portfelj1, stock1, 1);
			_stockExchange.RemoveStockFromPortfolio(portfelj1, stock1, 1);
			_stockExchange.RemoveStockFromPortfolio(portfelj1, stock1, 1);
		}




		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Ankica_TestRemoveStockFromPortfolio_NonExistingPortfolio()
		{
			_stockExchange = Factory.CreateStockExchange();
			// Briše se dionica iz nepostojećeg portfelja 

			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000000, 10m, DateTime.Now);

			_stockExchange.RemoveStockFromPortfolio("nepostojeciPortfelj", dionica1);
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Ankica_TestSetStockPrice_DifferentName()
		{
			_stockExchange = Factory.CreateStockExchange();
			// postavljanje cijene dionice sa drugačijim imenom 

			decimal oldPrice = 10m;
			_stockExchange.ListStock("IBM", 1000000, oldPrice, new DateTime(2012, 1, 10, 15, 22, 00));

			decimal newPrice = 20m;
			_stockExchange.SetStockPrice("KRIVO_IME", new DateTime(2012, 1, 10, 15, 40, 00), newPrice);
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Ankica_TestSetStockPrice_SameTimeStamp()
		{
			_stockExchange = Factory.CreateStockExchange();
			// Pokušaj dodavanja cijene za trenutak koji već postoji

			_stockExchange.ListStock("IBM", 1000000, 10m, DateTime.Now);

			decimal cijena = 10m;
			_stockExchange.SetStockPrice("IBM", new DateTime(2012, 1, 10, 15, 40, 00), cijena);
			_stockExchange.SetStockPrice("IBM", new DateTime(2012, 1, 10, 15, 40, 00), cijena);
		}

		[Test()]
		public void Ankica_TestSetStockPrice_SimilarName()
		{
			_stockExchange = Factory.CreateStockExchange();
			// Postavljanje cijene dionice sa sličnim imenom

			_stockExchange.ListStock("IBM", 1000000, 10m, DateTime.Now);

			decimal novaCijena = 20m;
			_stockExchange.SetStockPrice("IbM", new DateTime(2012, 1, 10, 15, 40, 00), novaCijena);
			Assert.AreEqual(novaCijena, _stockExchange.GetStockPrice("IBM", new DateTime(2012, 1, 10, 15, 40, 00)));
		}

		[Test()]
		[ExpectedException(typeof(StockExchangeException))]
		public void Ankica_TestSetStockPrice_StockNotExists()
		{
			_stockExchange = Factory.CreateStockExchange();
			// Pokušaj postavljanja cijene dionici koja ne postoji
			_stockExchange.ListStock("IBM", 1000000, 10m, DateTime.Now);

			decimal cijena = 10m;
			_stockExchange.SetStockPrice("KRIVO_IME", new DateTime(2012, 1, 10, 15, 40, 00), cijena);
		}

		[Test()]
		public void Ankica_TestGetPortfolioPercentChangeInValueForMonth_PriceChanges()
		{
			_stockExchange = Factory.CreateStockExchange();
			// provjera izračuna postotka mjesečne promjene
			string dionica1 = "Dionica1";
			_stockExchange.ListStock(dionica1, 1000, 100, new DateTime(2014, 1, 1, 0, 0, 0, 0));       // 1.1.2014. 0:00 100kn
			string dionica2 = "Dionica12";
			_stockExchange.ListStock(dionica2, 1000, 100, new DateTime(2014, 1, 1, 0, 0, 0, 0));
			string dionica3 = "Dionica13";
			_stockExchange.ListStock(dionica3, 1000, 100, new DateTime(2014, 1, 1, 0, 0, 0, 0));
			string dionica4 = "Dionica14";
			_stockExchange.ListStock(dionica4, 1000, 100, new DateTime(2014, 1, 1, 0, 0, 0, 0));
			_stockExchange.SetStockPrice(dionica1, new DateTime(2014, 1, 30, 23, 59, 59), 150);        // 31.1.2014. 0:00 150kn (+50%)
			_stockExchange.SetStockPrice(dionica2, new DateTime(2014, 1, 30, 23, 59, 59), 150);        // 31.1.2014. 0:00 150kn (+50%)
			_stockExchange.SetStockPrice(dionica3, new DateTime(2014, 1, 30, 23, 59, 59), 150);        // 31.1.2014. 0:00 150kn (+50%)
			_stockExchange.SetStockPrice(dionica4, new DateTime(2014, 1, 30, 23, 59, 59), 150);        // 31.1.2014. 0:00 150kn (+50%)
			string portfolio1 = "portfolio1";
			_stockExchange.CreatePortfolio(portfolio1);
			_stockExchange.AddStockToPortfolio(portfolio1, dionica1, 1);
			_stockExchange.AddStockToPortfolio(portfolio1, dionica1, 1);
			_stockExchange.AddStockToPortfolio(portfolio1, dionica2, 1);
			_stockExchange.AddStockToPortfolio(portfolio1, dionica3, 1);
			_stockExchange.AddStockToPortfolio(portfolio1, dionica4, 1);
			Assert.AreEqual(50, _stockExchange.GetPortfolioPercentChangeInValueForMonth(portfolio1, 2014, 1));  // 1. mjesec 2014.
		}


	}
}