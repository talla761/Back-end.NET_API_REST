using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
    [TestClass]
    public class CurveControllerTests
    {
        private Mock<IGenericRepository<CurvePoint>> _mockRepository;
        private Mock<IMapper> _mockMapper;
        private Mock<ILogger<CurveController>> _mockLogger;
        private CurveController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockRepository = new Mock<IGenericRepository<CurvePoint>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<CurveController>>();
            _controller = new CurveController(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [TestMethod]
        public async Task GetAll_ShouldReturnOk_WhenCurvePointsExist()
        {
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

            var result = await _controller.GetAll();
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOfType(okResult.Value, typeof(List<CurvePointDTO>));
            var returnValue = okResult.Value as List<CurvePointDTO>;
            Assert.AreEqual(2, returnValue.Count);
        }

        [TestMethod]
        public async Task GetById_ShouldReturnOk_WhenCurvePointExists()
        {
            var curvePoint = new CurvePoint { Id = 1, CurveId = 1, Term = 5.0, CurvePointValue = 100.0 };
            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(curvePoint);
            _mockMapper.Setup(m => m.Map<CurvePointDTO>(It.IsAny<CurvePoint>()))
                .Returns(new CurvePointDTO { Id = 1, CurveId = 1, Term = 5.0, CurvePointValue = 100.0 });

            var result = await _controller.GetById(1);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOfType(okResult.Value, typeof(CurvePointDTO));
            var returnValue = okResult.Value as CurvePointDTO;
            Assert.AreEqual(1, returnValue.Id);
        }

        [TestMethod]
        public async Task Create_ShouldReturnCreatedAtAction_WhenCurvePointIsCreated()
        {
            var curvePointDto = new CurvePointDTO { CurveId = 1, Term = 5.0, CurvePointValue = 100.0 };
            var curvePoint = new CurvePoint { Id = 1, CurveId = 1, Term = 5.0, CurvePointValue = 100.0 };

            _mockMapper.Setup(m => m.Map<CurvePoint>(It.IsAny<CurvePointDTO>())).Returns(curvePoint);
            _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<CurvePoint>())).ReturnsAsync(curvePoint);
            _mockMapper.Setup(m => m.Map<CurvePointDTO>(It.IsAny<CurvePoint>())).Returns(new CurvePointDTO { Id = 1, CurveId = 1, Term = 5.0, CurvePointValue = 100.0 });

            var result = await _controller.Create(curvePointDto);
            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual("GetById", createdAtActionResult.ActionName);
            Assert.IsInstanceOfType(createdAtActionResult.Value, typeof(CurvePointDTO));
            var returnValue = createdAtActionResult.Value as CurvePointDTO;
            Assert.AreEqual(1, returnValue.Id);
        }

        [TestMethod]
        public async Task Update_ShouldReturnNoContent_WhenCurvePointIsUpdated()
        {
            var curvePointDto = new CurvePointDTO { Id = 1, CurveId = 1, Term = 6.0, CurvePointValue = 110.0 };
            var curvePoint = new CurvePoint { Id = 1, CurveId = 1, Term = 5.0, CurvePointValue = 100.0 };

            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(curvePoint);
            _mockMapper.Setup(m => m.Map(It.IsAny<CurvePointDTO>(), It.IsAny<CurvePoint>()));
            _mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<CurvePoint>())).Returns(Task.CompletedTask);

            var result = await _controller.Update(1, curvePointDto);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task Delete_ShouldReturnNoContent_WhenCurvePointIsDeleted()
        {
            var curvePoint = new CurvePoint { Id = 1, CurveId = 1, Term = 5.0, CurvePointValue = 100.0 };
            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(curvePoint);
            _mockRepository.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _controller.Delete(1);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }
    }
}
