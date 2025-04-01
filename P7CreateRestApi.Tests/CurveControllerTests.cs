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
    public class CurveControllerTests
    {
        private readonly Mock<IGenericRepository<CurvePoint>> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<CurveController>> _mockLogger;
        private readonly CurveController _controller;

        public CurveControllerTests()
        {
            _mockRepository = new Mock<IGenericRepository<CurvePoint>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<CurveController>>();
            _controller = new CurveController(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
        }

        // Test de la méthode GET (GetAll)
        [Fact]
        public async Task GetAll_ShouldReturnOk_WhenCurvePointsExist()
        {
            // Arrange
            var curvePoints = new List<CurvePoint>
            {
                new CurvePoint { Id = 1, CurveId = 1, Term = 5.0, CurvePointValue = 100.0 },
                new CurvePoint { Id = 2, CurveId = 2, Term = 10.0, CurvePointValue = 150.0 }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(curvePoints);
            _mockMapper.Setup(m => m.Map<IEnumerable<CurvePointDTO>>(It.IsAny<IEnumerable<CurvePoint>>()))
                .Returns(new List<CurvePointDTO>
                {
                    new CurvePointDTO { Id = 1, CurveId = 1, Term = 5.0, CurvePointValue = 100.0 },
                    new CurvePointDTO { Id = 2, CurveId = 2, Term = 10.0, CurvePointValue = 150.0 }
                });

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<CurvePointDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        // Test de la méthode GET (GetById)
        [Fact]
        public async Task GetById_ShouldReturnOk_WhenCurvePointExists()
        {
            // Arrange
            var curvePoint = new CurvePoint { Id = 1, CurveId = 1, Term = 5.0, CurvePointValue = 100.0 };
            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(curvePoint);
            _mockMapper.Setup(m => m.Map<CurvePointDTO>(It.IsAny<CurvePoint>()))
                .Returns(new CurvePointDTO { Id = 1, CurveId = 1, Term = 5.0, CurvePointValue = 100.0 });

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<CurvePointDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        // Test de la méthode POST (Create)
        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction_WhenCurvePointIsCreated()
        {
            // Arrange
            var curvePointDto = new CurvePointDTO { CurveId = 1, Term = 5.0, CurvePointValue = 100.0 };
            var curvePoint = new CurvePoint { Id = 1, CurveId = 1, Term = 5.0, CurvePointValue = 100.0 };

            _mockMapper.Setup(m => m.Map<CurvePoint>(It.IsAny<CurvePointDTO>())).Returns(curvePoint);
            _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<CurvePoint>())).ReturnsAsync(curvePoint);
            _mockMapper.Setup(m => m.Map<CurvePointDTO>(It.IsAny<CurvePoint>())).Returns(new CurvePointDTO { Id = 1, CurveId = 1, Term = 5.0, CurvePointValue = 100.0 });

            // Act
            var result = await _controller.Create(curvePointDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("GetById", createdAtActionResult.ActionName);
            var returnValue = Assert.IsType<CurvePointDTO>(createdAtActionResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        // Test de la méthode PUT (Update)
        [Fact]
        public async Task Update_ShouldReturnNoContent_WhenCurvePointIsUpdated()
        {
            // Arrange
            var curvePointDto = new CurvePointDTO { Id = 1, CurveId = 1, Term = 6.0, CurvePointValue = 110.0 };
            var curvePoint = new CurvePoint { Id = 1, CurveId = 1, Term = 5.0, CurvePointValue = 100.0 };

            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(curvePoint);
            _mockMapper.Setup(m => m.Map(It.IsAny<CurvePointDTO>(), It.IsAny<CurvePoint>()));
            _mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<CurvePoint>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(1, curvePointDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        // Test de la méthode DELETE (Delete)
        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenCurvePointIsDeleted()
        {
            // Arrange
            var curvePoint = new CurvePoint { Id = 1, CurveId = 1, Term = 5.0, CurvePointValue = 100.0 };
            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(curvePoint);
            _mockRepository.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
