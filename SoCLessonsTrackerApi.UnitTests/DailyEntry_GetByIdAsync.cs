using System;
using Xunit;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace SoCLessonsTrackerApi.UnitTests
{
    public class DailyEntry_GetByIdAsync
    {
        [Fact]
        public async void WhenGetByIdAsyncFunctionIsCalled_IfValidId_ReturnsOkStatusAndEntryWithThatId()
        {
            string entryId = "test";
            var mockRepository = new Mock<IRepository<DailyEntry>>();
            mockRepository.Setup(repo => repo.GetByIdAsync(entryId)).Returns(Task.FromResult<DailyEntry>(new DailyEntry {Id = entryId}));;
            var controller = new DailyEntryController(mockRepository.Object);

            var result = await controller.GetById(entryId);

            var viewResult = Assert.IsType<OkObjectResult>(result);
            
            mockRepository.Verify(repo => repo.GetByIdAsync(entryId), Times.Once);
            Assert.Equal(200, viewResult.StatusCode);
            Assert.NotNull(viewResult);
        }
        
        [Fact]
        public async void WWhenGetByIdAsyncFunctionIsCalled__IfExceptionThrown_ReturnsNotFoundStatus()
        {
            string entryId = "test";
            var mockRepository = new Mock<IRepository<DailyEntry>>();
            mockRepository.Setup(repo => repo.GetByIdAsync(entryId)).Throws(new Exception());
            var controller = new DailyEntryController(mockRepository.Object);

            var result = await controller.GetById(entryId);
            var viewResult = Assert.IsType<NotFoundObjectResult>(result);
            var model = viewResult.Value;

            Assert.Equal(404, viewResult.StatusCode);
            Assert.Equal(model, $"There is no daily entry with id {entryId}");

        }

    }
}
