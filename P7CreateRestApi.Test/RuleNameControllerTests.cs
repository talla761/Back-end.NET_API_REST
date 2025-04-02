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
    public class RuleNameControllerTests
    {
        private Mock<IGenericRepository<RuleName>> _mockRepository;
        private Mock<IMapper> _mockMapper;
        private Mock<ILogger<RuleNameController>> _mockLogger;
        private RuleNameController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockRepository = new Mock<IGenericRepository<RuleName>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<RuleNameController>>();
            _controller = new RuleNameController(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [TestMethod]
        public async Task GetAll_ShouldReturnOk_WhenRuleNamesExist()
        {
            var ruleNames = new List<RuleName>
            {
                new RuleName { Id = 1, Name = "Rule1", Description = "Description1", Json = "{}", Template = "Template1", SqlStr = "SQL1", SqlPart = "Part1" },
                new RuleName { Id = 2, Name = "Rule2", Description = "Description2", Json = "{}", Template = "Template2", SqlStr = "SQL2", SqlPart = "Part2" }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(ruleNames);
            _mockMapper.Setup(m => m.Map<IEnumerable<RuleNameDTO>>(ruleNames))
                .Returns(new List<RuleNameDTO>
                {
                    new RuleNameDTO { Id = 1, Name = "Rule1", Description = "Description1", Json = "{}", Template = "Template1", SqlStr = "SQL1", SqlPart = "Part1" },
                    new RuleNameDTO { Id = 2, Name = "Rule2", Description = "Description2", Json = "{}", Template = "Template2", SqlStr = "SQL2", SqlPart = "Part2" }
                });

            var result = await _controller.GetAll();

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnValue = okResult.Value as List<RuleNameDTO>;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual(2, returnValue.Count);
        }

        [TestMethod]
        public async Task GetById_ShouldReturnOk_WhenRuleNameExists()
        {
            var ruleName = new RuleName { Id = 1, Name = "Rule1", Description = "Description1", Json = "{}", Template = "Template1", SqlStr = "SQL1", SqlPart = "Part1" };
            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(ruleName);
            _mockMapper.Setup(m => m.Map<RuleNameDTO>(ruleName))
                .Returns(new RuleNameDTO { Id = 1, Name = "Rule1", Description = "Description1", Json = "{}", Template = "Template1", SqlStr = "SQL1", SqlPart = "Part1" });

            var result = await _controller.GetById(1);

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnValue = okResult.Value as RuleNameDTO;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual(1, returnValue.Id);
        }

        [TestMethod]
        public async Task Create_ShouldReturnCreatedAtAction_WhenRuleNameIsCreated()
        {
            var ruleNameDto = new RuleNameDTO { Name = "Rule1", Description = "Description1", Json = "{}", Template = "Template1", SqlStr = "SQL1", SqlPart = "Part1" };
            var ruleName = new RuleName { Id = 1, Name = "Rule1", Description = "Description1", Json = "{}", Template = "Template1", SqlStr = "SQL1", SqlPart = "Part1" };

            _mockMapper.Setup(m => m.Map<RuleName>(ruleNameDto)).Returns(ruleName);
            _mockRepository.Setup(repo => repo.AddAsync(ruleName)).ReturnsAsync(ruleName);
            _mockMapper.Setup(m => m.Map<RuleNameDTO>(ruleName)).Returns(new RuleNameDTO { Id = 1, Name = "Rule1", Description = "Description1", Json = "{}", Template = "Template1", SqlStr = "SQL1", SqlPart = "Part1" });

            var result = await _controller.Create(ruleNameDto);

            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual("GetById", createdAtActionResult.ActionName);
            var returnValue = createdAtActionResult.Value as RuleNameDTO;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual(1, returnValue.Id);
        }

        [TestMethod]
        public async Task Update_ShouldReturnNoContent_WhenRuleNameIsUpdated()
        {
            var ruleNameDto = new RuleNameDTO { Id = 1, Name = "UpdatedRule", Description = "UpdatedDescription", Json = "{}", Template = "UpdatedTemplate", SqlStr = "UpdatedSQL", SqlPart = "UpdatedPart" };
            var ruleName = new RuleName { Id = 1, Name = "Rule1", Description = "Description1", Json = "{}", Template = "Template1", SqlStr = "SQL1", SqlPart = "Part1" };

            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(ruleName);
            _mockMapper.Setup(m => m.Map(ruleNameDto, ruleName));
            _mockRepository.Setup(repo => repo.UpdateAsync(ruleName)).Returns(Task.CompletedTask);

            var result = await _controller.Update(1, ruleNameDto);

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task Delete_ShouldReturnNoContent_WhenRuleNameIsDeleted()
        {
            var ruleName = new RuleName { Id = 1, Name = "Rule1", Description = "Description1", Json = "{}", Template = "Template1", SqlStr = "SQL1", SqlPart = "Part1" };
            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(ruleName);
            _mockRepository.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _controller.Delete(1);

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }
    }
}
