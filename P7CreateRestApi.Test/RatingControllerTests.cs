using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    [TestClass]
    public class RatingControllerTests
    {
        private Mock<IGenericRepository<Rating>> _mockRepository;
        private Mock<IMapper> _mockMapper;
        private Mock<ILogger<RatingController>> _mockLogger;
        private RatingController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockRepository = new Mock<IGenericRepository<Rating>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<RatingController>>();
            _controller = new RatingController(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [TestMethod]
        public async Task GetAll_ShouldReturnOk_WhenRatingsExist()
        {
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

            var result = await _controller.GetAll();

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnValue = okResult.Value as List<RatingDTO>;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual(2, returnValue.Count);
        }

        [TestMethod]
        public async Task GetById_ShouldReturnOk_WhenRatingExists()
        {
            var rating = new Rating { Id = 1, MoodysRating = "A", SandPRating = "A", FitchRating = "A", OrderNumber = 1 };
            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(rating);
            _mockMapper.Setup(m => m.Map<RatingDTO>(It.IsAny<Rating>()))
                .Returns(new RatingDTO { Id = 1, MoodysRating = "A", SandPRating = "A", FitchRating = "A", OrderNumber = 1 });

            var result = await _controller.GetById(1);

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnValue = okResult.Value as RatingDTO;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual(1, returnValue.Id);
        }

        [TestMethod]
        public async Task Create_ShouldReturnCreatedAtAction_WhenRatingIsCreated()
        {
            var ratingDto = new RatingDTO { MoodysRating = "A", SandPRating = "A", FitchRating = "A", OrderNumber = 1 };
            var rating = new Rating { Id = 1, MoodysRating = "A", SandPRating = "A", FitchRating = "A", OrderNumber = 1 };

            _mockMapper.Setup(m => m.Map<Rating>(It.IsAny<RatingDTO>())).Returns(rating);
            _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Rating>())).ReturnsAsync(rating);
            _mockMapper.Setup(m => m.Map<RatingDTO>(It.IsAny<Rating>())).Returns(new RatingDTO { Id = 1, MoodysRating = "A", SandPRating = "A", FitchRating = "A", OrderNumber = 1 });

            var result = await _controller.Create(ratingDto);

            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual("GetById", createdAtActionResult.ActionName);
            var returnValue = createdAtActionResult.Value as RatingDTO;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual(1, returnValue.Id);
        }

        [TestMethod]
        public async Task Update_ShouldReturnNoContent_WhenRatingIsUpdated()
        {
            var ratingDto = new RatingDTO { Id = 1, MoodysRating = "B", SandPRating = "B", FitchRating = "B", OrderNumber = 2 };
            var rating = new Rating { Id = 1, MoodysRating = "A", SandPRating = "A", FitchRating = "A", OrderNumber = 1 };

            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(rating);
            _mockMapper.Setup(m => m.Map(It.IsAny<RatingDTO>(), It.IsAny<Rating>()));
            _mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Rating>())).Returns(Task.CompletedTask);

            var result = await _controller.Update(1, ratingDto);

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task Delete_ShouldReturnNoContent_WhenRatingIsDeleted()
        {
            var rating = new Rating { Id = 1, MoodysRating = "A", SandPRating = "A", FitchRating = "A", OrderNumber = 1 };
            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(rating);
            _mockRepository.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _controller.Delete(1);

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }
    }
}
