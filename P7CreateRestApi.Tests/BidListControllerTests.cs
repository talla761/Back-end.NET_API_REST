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
using System.Linq;

namespace P7CreateRestApi.Tests
{
    public class BidListControllerTests
    {
        private readonly Mock<IGenericRepository<BidList>> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<BidListController>> _mockLogger;
        private readonly BidListController _controller;

        public BidListControllerTests()
        {
            _mockRepository = new Mock<IGenericRepository<BidList>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<BidListController>>();
            _controller = new BidListController(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
        }

        // Test de la méthode GET (GetAll)
        [Fact]
        public async Task GetAll_ShouldReturnOk_WhenBidListsExist()
        {
            // Arrange
            var bidLists = new List<BidList>
            {
                new BidList { BidListId = 1, Account = "Account1", BidType = "Type1" },
                new BidList { BidListId = 2, Account = "Account2", BidType = "Type2" }
            };
            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(bidLists);
            _mockMapper.Setup(m => m.Map<IEnumerable<BidListDTO>>(It.IsAny<IEnumerable<BidList>>()))
                .Returns(new List<BidListDTO>
                {
                    new BidListDTO { BidListId = 1, Account = "Account1" },
                    new BidListDTO { BidListId = 2, Account = "Account2" }
                });

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<BidListDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        // Test de la méthode GET (GetById)
        [Fact]
        public async Task GetById_ShouldReturnOk_WhenBidListExists()
        {
            // Arrange
            var bidList = new BidList { BidListId = 1, Account = "Account1", BidType = "Type1" };
            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(bidList);
            _mockMapper.Setup(m => m.Map<BidListDTO>(It.IsAny<BidList>()))
                .Returns(new BidListDTO { BidListId = 1, Account = "Account1" });

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<BidListDTO>(okResult.Value);
            Assert.Equal(1, returnValue.BidListId);
        }

        // Test de la méthode POST (Create)
        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction_WhenBidListIsCreated()
        {
            // Arrange
            var bidListDto = new BidListDTO { Account = "Account1", BidType = "Type1" };
            var bidList = new BidList { BidListId = 1, Account = "Account1", BidType = "Type1" };

            _mockMapper.Setup(m => m.Map<BidList>(It.IsAny<BidListDTO>())).Returns(bidList);
            _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<BidList>())).ReturnsAsync(bidList);
            _mockMapper.Setup(m => m.Map<BidListDTO>(It.IsAny<BidList>())).Returns(new BidListDTO { BidListId = 1, Account = "Account1" });

            // Act
            var result = await _controller.Create(bidListDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("GetById", createdAtActionResult.ActionName);
            var returnValue = Assert.IsType<BidListDTO>(createdAtActionResult.Value);
            Assert.Equal(1, returnValue.BidListId);
        }

        // Test de la méthode PUT (Update)
        [Fact]
        public async Task Update_ShouldReturnNoContent_WhenBidListIsUpdated()
        {
            // Arrange
            var bidListDto = new BidListDTO { BidListId = 1, Account = "UpdatedAccount", BidType = "UpdatedType" };
            var bidList = new BidList { BidListId = 1, Account = "Account1", BidType = "Type1" };

            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(bidList);
            _mockMapper.Setup(m => m.Map(It.IsAny<BidListDTO>(), It.IsAny<BidList>()));
            _mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<BidList>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(1, bidListDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        // Test de la méthode DELETE (Delete)
        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenBidListIsDeleted()
        {
            // Arrange
            var bidList = new BidList { BidListId = 1, Account = "Account1", BidType = "Type1" };
            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(bidList);
            _mockRepository.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
