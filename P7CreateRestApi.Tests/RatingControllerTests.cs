using Moq;
using Xunit;
using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;
using P7CreateRestApi.DTOs;
using P7CreateRestApi.Repositories.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace P7CreateRestApi.Tests
{
    public class RatingControllerTests
    {
        private readonly Mock<IGenericRepository<Rating>> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<RatingController>> _mockLogger;
        private readonly RatingController _controller;

        public RatingControllerTests()
        {
            _mockRepository = new Mock<IGenericRepository<Rating>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<RatingController>>();
            _controller = new RatingController(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
        }

        // Test de la méthode GET (GetAll)
        [Fact]
        public async Task GetAll_ShouldReturnOk_WhenRatingsExist()
        {
            // Arrange
            var ratings = new List<Rating>
            {
                new Rating { Id = 1, MoodysRating = "A", SandPRating = "A", FitchRating = "A", OrderNumber = 1 },
                new Rating { Id = 2, MoodysRating = "B", SandPRating = "B", FitchRating = "B", OrderNumber = 2 }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(ratings);
            _mockMapper.Setup(m => m.Map<IEnumerable<RatingDTO>>(It.IsAny<IEnumerable<Rating>>()))
                .Returns(new List<RatingDTO>
                {
                    new RatingDTO { Id = 1, MoodysRating = "A", SandPRating = "A", FitchRating = "A", OrderNumber = 1 },
                    new RatingDTO { Id = 2, MoodysRating = "B", SandPRating = "B", FitchRating = "B", OrderNumber = 2 }
                });

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<RatingDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        // Test de la méthode GET (GetById)
        [Fact]
        public async Task GetById_ShouldReturnOk_WhenRatingExists()
        {
            // Arrange
            var rating = new Rating { Id = 1, MoodysRating = "A", SandPRating = "A", FitchRating = "A", OrderNumber = 1 };
            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(rating);
            _mockMapper.Setup(m => m.Map<RatingDTO>(It.IsAny<Rating>()))
                .Returns(new RatingDTO { Id = 1, MoodysRating = "A", SandPRating = "A", FitchRating = "A", OrderNumber = 1 });

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<RatingDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        // Test de la méthode POST (Create)
        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction_WhenRatingIsCreated()
        {
            // Arrange
            var ratingDto = new RatingDTO { MoodysRating = "A", SandPRating = "A", FitchRating = "A", OrderNumber = 1 };
            var rating = new Rating { Id = 1, MoodysRating = "A", SandPRating = "A", FitchRating = "A", OrderNumber = 1 };

            _mockMapper.Setup(m => m.Map<Rating>(It.IsAny<RatingDTO>())).Returns(rating);
            _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Rating>())).ReturnsAsync(rating);
            _mockMapper.Setup(m => m.Map<RatingDTO>(It.IsAny<Rating>())).Returns(new RatingDTO { Id = 1, MoodysRating = "A", SandPRating = "A", FitchRating = "A", OrderNumber = 1 });

            // Act
            var result = await _controller.Create(ratingDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("GetById", createdAtActionResult.ActionName);
            var returnValue = Assert.IsType<RatingDTO>(createdAtActionResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        // Test de la méthode PUT (Update)
        [Fact]
        public async Task Update_ShouldReturnNoContent_WhenRatingIsUpdated()
        {
            // Arrange
            var ratingDto = new RatingDTO { Id = 1, MoodysRating = "B", SandPRating = "B", FitchRating = "B", OrderNumber = 2 };
            var rating = new Rating { Id = 1, MoodysRating = "A", SandPRating = "A", FitchRating = "A", OrderNumber = 1 };

            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(rating);
            _mockMapper.Setup(m => m.Map(It.IsAny<RatingDTO>(), It.IsAny<Rating>()));
            _mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Rating>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(1, ratingDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        // Test de la méthode DELETE (Delete)
        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenRatingIsDeleted()
        {
            // Arrange
            var rating = new Rating { Id = 1, MoodysRating = "A", SandPRating = "A", FitchRating = "A", OrderNumber = 1 };
            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(rating);
            _mockRepository.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
