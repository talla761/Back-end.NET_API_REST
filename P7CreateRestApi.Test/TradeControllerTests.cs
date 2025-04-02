using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using P7CreateRestApi.DTOs;
using P7CreateRestApi.Repositories.Interfaces;
using Dot.Net.WebApi.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dot.Net.WebApi.Controllers;

namespace P7CreateRestApi.Tests
{
    [TestClass]
    public class TradeControllerTests
    {
        private Mock<IGenericRepository<Trade>> _mockRepository;
        private Mock<IMapper> _mockMapper;
        private Mock<ILogger<TradeController>> _mockLogger;
        private TradeController _controller;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new Mock<IGenericRepository<Trade>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<TradeController>>();
            _controller = new TradeController(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
        }

        // Test pour la méthode GET (GetAll)
        [TestMethod]
        public async Task GetAll_ShouldReturnOk_WhenTradesExist()
        {
            // Arrange
            var trades = new List<Trade>
            {
                new Trade { TradeId = 1, Account = "A1", AccountType = "Type1", BuyQuantity = 10, SellQuantity = 5, BuyPrice = 100, SellPrice = 105, TradeDate = DateTime.Now, TradeSecurity = "Security1", TradeStatus = "Status1", Trader = "Trader1", Benchmark = "Benchmark1", Book = "Book1", CreationName = "Creator1", CreationDate = DateTime.Now, RevisionName = "Rev1", RevisionDate = DateTime.Now, DealName = "Deal1", DealType = "Type1", SourceListId = "List1", Side = "Buy" },
                new Trade { TradeId = 2, Account = "A2", AccountType = "Type2", BuyQuantity = 20, SellQuantity = 10, BuyPrice = 200, SellPrice = 210, TradeDate = DateTime.Now, TradeSecurity = "Security2", TradeStatus = "Status2", Trader = "Trader2", Benchmark = "Benchmark2", Book = "Book2", CreationName = "Creator2", CreationDate = DateTime.Now, RevisionName = "Rev2", RevisionDate = DateTime.Now, DealName = "Deal2", DealType = "Type2", SourceListId = "List2", Side = "Sell" }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(trades);
            _mockMapper.Setup(m => m.Map<IEnumerable<TradeDTO>>(It.IsAny<IEnumerable<Trade>>()))
                .Returns(new List<TradeDTO>
                {
                    new TradeDTO { TradeId = 1, Account = "A1", AccountType = "Type1", BuyQuantity = 10, SellQuantity = 5, BuyPrice = 100, SellPrice = 105, TradeDate = DateTime.Now, TradeSecurity = "Security1", TradeStatus = "Status1", Trader = "Trader1", Benchmark = "Benchmark1", Book = "Book1", CreationName = "Creator1", CreationDate = DateTime.Now, RevisionName = "Rev1", RevisionDate = DateTime.Now, DealName = "Deal1", DealType = "Type1", SourceListId = "List1", Side = "Buy" },
                    new TradeDTO { TradeId = 2, Account = "A2", AccountType = "Type2", BuyQuantity = 20, SellQuantity = 10, BuyPrice = 200, SellPrice = 210, TradeDate = DateTime.Now, TradeSecurity = "Security2", TradeStatus = "Status2", Trader = "Trader2", Benchmark = "Benchmark2", Book = "Book2", CreationName = "Creator2", CreationDate = DateTime.Now, RevisionName = "Rev2", RevisionDate = DateTime.Now, DealName = "Deal2", DealType = "Type2", SourceListId = "List2", Side = "Sell" }
                });

            // Act
            var result = await _controller.GetAll();

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOfType(okResult.Value, typeof(List<TradeDTO>));
            var returnValue = okResult.Value as List<TradeDTO>;
            Assert.AreEqual(2, returnValue.Count);
        }

        // Test pour la méthode GET (GetById)
        [TestMethod]
        public async Task GetById_ShouldReturnOk_WhenTradeExists()
        {
            // Arrange
            var trade = new Trade { TradeId = 1, Account = "A1", AccountType = "Type1", BuyQuantity = 10, SellQuantity = 5, BuyPrice = 100, SellPrice = 105, TradeDate = DateTime.Now, TradeSecurity = "Security1", TradeStatus = "Status1", Trader = "Trader1", Benchmark = "Benchmark1", Book = "Book1", CreationName = "Creator1", CreationDate = DateTime.Now, RevisionName = "Rev1", RevisionDate = DateTime.Now, DealName = "Deal1", DealType = "Type1", SourceListId = "List1", Side = "Buy" };

            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(trade);
            _mockMapper.Setup(m => m.Map<TradeDTO>(It.IsAny<Trade>()))
                .Returns(new TradeDTO { TradeId = 1, Account = "A1", AccountType = "Type1", BuyQuantity = 10, SellQuantity = 5, BuyPrice = 100, SellPrice = 105, TradeDate = DateTime.Now, TradeSecurity = "Security1", TradeStatus = "Status1", Trader = "Trader1", Benchmark = "Benchmark1", Book = "Book1", CreationName = "Creator1", CreationDate = DateTime.Now, RevisionName = "Rev1", RevisionDate = DateTime.Now, DealName = "Deal1", DealType = "Type1", SourceListId = "List1", Side = "Buy" });

            // Act
            var result = await _controller.GetById(1);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOfType(okResult.Value, typeof(TradeDTO));
            var returnValue = okResult.Value as TradeDTO;
            Assert.AreEqual(1, returnValue.TradeId);
        }

        // Test pour la méthode POST (Create)
        [TestMethod]
        public async Task Create_ShouldReturnCreatedAtAction_WhenTradeIsCreated()
        {
            // Arrange
            var tradeDto = new TradeDTO
            {
                Account = "A1",
                AccountType = "Type1",
                BuyQuantity = 10,
                SellQuantity = 5,
                BuyPrice = 100,
                SellPrice = 105,
                TradeDate = DateTime.Now,
                TradeSecurity = "Security1",
                TradeStatus = "Status1",
                Trader = "Trader1",
                Benchmark = "Benchmark1",
                Book = "Book1",
                CreationName = "Creator1",
                CreationDate = DateTime.Now,
                RevisionName = "Rev1",
                RevisionDate = DateTime.Now,
                DealName = "Deal1",
                DealType = "Type1",
                SourceListId = "List1",
                Side = "Buy"
            };

            var trade = new Trade
            {
                TradeId = 1,
                Account = "A1",
                AccountType = "Type1",
                BuyQuantity = 10,
                SellQuantity = 5,
                BuyPrice = 100,
                SellPrice = 105,
                TradeDate = DateTime.Now,
                TradeSecurity = "Security1",
                TradeStatus = "Status1",
                Trader = "Trader1",
                Benchmark = "Benchmark1",
                Book = "Book1",
                CreationName = "Creator1",
                CreationDate = DateTime.Now,
                RevisionName = "Rev1",
                RevisionDate = DateTime.Now,
                DealName = "Deal1",
                DealType = "Type1",
                SourceListId = "List1",
                Side = "Buy"
            };

            // Setup du mapping et du repository mocké
            _mockMapper.Setup(m => m.Map<Trade>(It.IsAny<TradeDTO>())).Returns(trade);
            _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Trade>())).ReturnsAsync(trade);
            _mockMapper.Setup(m => m.Map<TradeDTO>(It.IsAny<Trade>())).Returns(new TradeDTO
            {
                TradeId = 1,
                Account = "A1",
                AccountType = "Type1",
                BuyQuantity = 10,
                SellQuantity = 5,
                BuyPrice = 100,
                SellPrice = 105,
                TradeDate = DateTime.Now,
                TradeSecurity = "Security1",
                TradeStatus = "Status1",
                Trader = "Trader1",
                Benchmark = "Benchmark1",
                Book = "Book1",
                CreationName = "Creator1",
                CreationDate = DateTime.Now,
                RevisionName = "Rev1",
                RevisionDate = DateTime.Now,
                DealName = "Deal1",
                DealType = "Type1",
                SourceListId = "List1",
                Side = "Buy"
            });

            // Act
            var result = await _controller.Create(tradeDto);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual("GetById", createdAtActionResult.ActionName);
            Assert.IsInstanceOfType(createdAtActionResult.Value, typeof(TradeDTO));
            var returnValue = createdAtActionResult.Value as TradeDTO;
            Assert.AreEqual(1, returnValue.TradeId);
        }


        // Test pour la méthode PUT (Update)
        [TestMethod]
        public async Task Update_ShouldReturnNoContent_WhenTradeIsUpdated()
        {
            // Arrange
            var tradeDto = new TradeDTO { TradeId = 1, Account = "A1", AccountType = "Type1", BuyQuantity = 10, SellQuantity = 5, BuyPrice = 100, SellPrice = 105, TradeDate = DateTime.Now, TradeSecurity = "Security1", TradeStatus = "Status1", Trader = "Trader1", Benchmark = "Benchmark1", Book = "Book1", CreationName = "Creator1", CreationDate = DateTime.Now, RevisionName = "Rev1", RevisionDate = DateTime.Now, DealName = "Deal1", DealType = "Type1", SourceListId = "List1", Side = "Buy" };

            var trade = new Trade { TradeId = 1, Account = "A1", AccountType = "Type1", BuyQuantity = 10, SellQuantity = 5, BuyPrice = 100, SellPrice = 105, TradeDate = DateTime.Now, TradeSecurity = "Security1", TradeStatus = "Status1", Trader = "Trader1", Benchmark = "Benchmark1", Book = "Book1", CreationName = "Creator1", CreationDate = DateTime.Now, RevisionName = "Rev1", RevisionDate = DateTime.Now, DealName = "Deal1", DealType = "Type1", SourceListId = "List1", Side = "Buy" };

            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(trade);
            _mockMapper.Setup(m => m.Map(It.IsAny<TradeDTO>(), It.IsAny<Trade>()));

            // Act
            var result = await _controller.Update(1, tradeDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        // Test pour la méthode DELETE (Delete)
        [TestMethod]
        public async Task Delete_ShouldReturnNoContent_WhenTradeIsDeleted()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }
    }
}
