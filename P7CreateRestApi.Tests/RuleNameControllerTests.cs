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
    public class RuleNameControllerTests
    {
        private readonly Mock<IGenericRepository<RuleName>> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<RuleNameController>> _mockLogger;
        private readonly RuleNameController _controller;

        public RuleNameControllerTests()
        {
            _mockRepository = new Mock<IGenericRepository<RuleName>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<RuleNameController>>();
            _controller = new RuleNameController(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
        }

        // Test de la méthode GET (GetAll)
        [Fact]
        public async Task GetAll_ShouldReturnOk_WhenRuleNamesExist()
        {
            // Arrange
            var ruleNames = new List<RuleName>
            {
                new RuleName { Id = 1, Name = "Rule1", Description = "Description1", Json = "{}", Template = "Template1", SqlStr = "SQL1", SqlPart = "Part1" },
                new RuleName { Id = 2, Name = "Rule2", Description = "Description2", Json = "{}", Template = "Template2", SqlStr = "SQL2", SqlPart = "Part2" }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(ruleNames);
            _mockMapper.Setup(m => m.Map<IEnumerable<RuleNameDTO>>(It.IsAny<IEnumerable<RuleName>>()))
                .Returns(new List<RuleNameDTO>
                {
                    new RuleNameDTO { Id = 1, Name = "Rule1", Description = "Description1", Json = "{}", Template = "Template1", SqlStr = "SQL1", SqlPart = "Part1" },
                    new RuleNameDTO { Id = 2, Name = "Rule2", Description = "Description2", Json = "{}", Template = "Template2", SqlStr = "SQL2", SqlPart = "Part2" }
                });

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<RuleNameDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        // Test de la méthode GET (GetById)
        [Fact]
        public async Task GetById_ShouldReturnOk_WhenRuleNameExists()
        {
            // Arrange
            var ruleName = new RuleName { Id = 1, Name = "Rule1", Description = "Description1", Json = "{}", Template = "Template1", SqlStr = "SQL1", SqlPart = "Part1" };
            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(ruleName);
            _mockMapper.Setup(m => m.Map<RuleNameDTO>(It.IsAny<RuleName>()))
                .Returns(new RuleNameDTO { Id = 1, Name = "Rule1", Description = "Description1", Json = "{}", Template = "Template1", SqlStr = "SQL1", SqlPart = "Part1" });

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<RuleNameDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        // Test de la méthode POST (Create)
        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction_WhenRuleNameIsCreated()
        {
            // Arrange
            var ruleNameDto = new RuleNameDTO { Name = "Rule1", Description = "Description1", Json = "{}", Template = "Template1", SqlStr = "SQL1", SqlPart = "Part1" };
            var ruleName = new RuleName { Id = 1, Name = "Rule1", Description = "Description1", Json = "{}", Template = "Template1", SqlStr = "SQL1", SqlPart = "Part1" };

            _mockMapper.Setup(m => m.Map<RuleName>(It.IsAny<RuleNameDTO>())).Returns(ruleName);
            _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<RuleName>())).ReturnsAsync(ruleName);
            _mockMapper.Setup(m => m.Map<RuleNameDTO>(It.IsAny<RuleName>())).Returns(new RuleNameDTO { Id = 1, Name = "Rule1", Description = "Description1", Json = "{}", Template = "Template1", SqlStr = "SQL1", SqlPart = "Part1" });

            // Act
            var result = await _controller.Create(ruleNameDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("GetById", createdAtActionResult.ActionName);
            var returnValue = Assert.IsType<RuleNameDTO>(createdAtActionResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        // Test de la méthode PUT (Update)
        [Fact]
        public async Task Update_ShouldReturnNoContent_WhenRuleNameIsUpdated()
        {
            // Arrange
            var ruleNameDto = new RuleNameDTO { Id = 1, Name = "UpdatedRule", Description = "UpdatedDescription", Json = "{}", Template = "UpdatedTemplate", SqlStr = "UpdatedSQL", SqlPart = "UpdatedPart" };
            var ruleName = new RuleName { Id = 1, Name = "Rule1", Description = "Description1", Json = "{}", Template = "Template1", SqlStr = "SQL1", SqlPart = "Part1" };

            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(ruleName);
            _mockMapper.Setup(m => m.Map(It.IsAny<RuleNameDTO>(), It.IsAny<RuleName>()));
            _mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<RuleName>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(1, ruleNameDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        // Test de la méthode DELETE (Delete)
        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenRuleNameIsDeleted()
        {
            // Arrange
            var ruleName = new RuleName { Id = 1, Name = "Rule1", Description = "Description1", Json = "{}", Template = "Template1", SqlStr = "SQL1", SqlPart = "Part1" };
            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(ruleName);
            _mockRepository.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
