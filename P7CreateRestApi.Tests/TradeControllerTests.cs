using Moq;
using Xunit;
using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Domain;
using P7CreateRestApi.DTOs;
using P7CreateRestApi.Repositories.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace P7CreateRestApi.Tests
{
    public class TradeControllerTests
    {
        private readonly Mock<IGenericRepository<Trade>> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<TradeController>> _mockLogger;
        private readonly TradeController _controller;

        public TradeControllerTests()
        {
            _mockRepository = new Mock<IGenericRepository<Trade>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<TradeController>>();
            _controller = new TradeController(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
        }

        // Test de la méthode GET (GetAll)
        [Fact]
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
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<TradeDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        // Test de la méthode GET (GetById)
        [Fact]
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
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<TradeDTO>(okResult.Value);
            Assert.Equal(1, returnValue.TradeId);
        }

        // Test de la méthode POST (Create)
        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction_WhenTradeIsCreated()
        {
            // Arrange
            var tradeDto = new TradeDTO { Account = "A1", AccountType = "Type1", BuyQuantity = 10, SellQuantity = 5, BuyPrice = 100, SellPrice = 105, TradeDate = DateTime.Now, TradeSecurity = "Security1", TradeStatus = "Status1", Trader = "Trader1", Benchmark = "Benchmark1", Book = "Book1", CreationName = "Creator1", CreationDate = DateTime.Now, RevisionName = "Rev1", RevisionDate = DateTime.Now, DealName = "Deal1", DealType = "Type1", SourceListId = "List1", Side = "Buy" };
            var trade = new Trade { TradeId = 1, Account = "A1", AccountType = "Type1", BuyQuantity = 10, SellQuantity = 5, BuyPrice = 100, SellPrice = 105, TradeDate = DateTime.Now, TradeSecurity = "Security1", TradeStatus = "Status1", Trader = "Trader1", Benchmark = "Benchmark1", Book = "Book1", CreationName = "Creator1", CreationDate = DateTime.Now, RevisionName = "Rev1", RevisionDate = DateTime.Now, DealName = "Deal1", DealType = "Type1", SourceListId = "List1", Side = "Buy" };

            _mockMapper.Setup(m => m.Map<Trade>(It.IsAny<TradeDTO>())).Returns(trade);
            _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Trade>())).ReturnsAsync(trade);
            _mockMapper.Setup(m => m.Map<TradeDTO>(It.IsAny<Trade>())).Returns(new TradeDTO { TradeId = 1, Account = "A1", AccountType = "Type1", BuyQuantity = 10, SellQuantity = 5, BuyPrice = 100, SellPrice = 105, TradeDate = DateTime.Now, TradeSecurity = "Security1", TradeStatus = "Status1", Trader = "Trader1", Benchmark = "Benchmark1", Book = "Book1", CreationName = "Creator1", CreationDate = DateTime.Now, RevisionName = "Rev1", RevisionDate = DateTime.Now, DealName = "Deal1", DealType = "Type1", SourceListId = "List1", Side = "Buy" });

            // Act
            var result = await _controller.Create(tradeDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("GetById", createdAtActionResult.ActionName);
            var returnValue = Assert.IsType<TradeDTO>(createdAtActionResult.Value);
            Assert.Equal(1, returnValue.TradeId);
        }

        // Test de la méthode PUT (Update)
        [Fact]
        public async Task Update_ShouldReturnNoContent_WhenTradeIsUpdated()
        {
            // Arrange
            var tradeDto = new TradeDTO { TradeId = 1, Account = "A1", AccountType = "Type1", BuyQuantity = 10, SellQuantity = 5, BuyPrice = 100, SellPrice = 105, TradeDate = DateTime.Now, TradeSecurity = "Security1", TradeStatus = "Status1", Trader = "Trader1", Benchmark = "Benchmark1", Book = "Book1", CreationName = "Creator1", CreationDate = DateTime.Now, RevisionName = "Rev1", RevisionDate = DateTime.Now, DealName = "Deal1", DealType = "Type1", SourceListId = "List1", Side = "Buy" };

            var trade = new Trade { TradeId = 1, Account = "A1", AccountType = "Type1", BuyQuantity = 10, SellQuantity = 5, BuyPrice = 100, SellPrice = 105, TradeDate = DateTime.Now, TradeSecurity = "Security1", TradeStatus = "Status1", Trader = "Trader1", Benchmark = "Benchmark1", Book = "Book1", CreationName = "Creator1", CreationDate = DateTime.Now, RevisionName = "Rev1", RevisionDate = DateTime.Now, DealName = "Deal1", DealType = "Type1", SourceListId = "List1", Side = "Buy" };

            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(trade);
            _mockMapper.Setup(m => m.Map(It.IsAny<TradeDTO>(), It.IsAny<Trade>())).Verifiable();
            _mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Trade>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(1, tradeDto);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
        }

        // Test de la méthode DELETE (Delete)
        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenTradeIsDeleted()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
        }
    }
}
