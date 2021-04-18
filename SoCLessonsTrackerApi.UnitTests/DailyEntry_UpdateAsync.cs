using System;
using Xunit;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace SoCLessonsTrackerApi.UnitTests
{
    public class DailyEntry_UpdateAsync
    {
        [Fact]
        public async void WhenUpdateFunctionIsCalled_IfValidId_ReturnsOkStatus()
        {
            string entryId = "test";
            var testEntry = new DailyEntry() {};
            var mockRepository = new Mock<IRepository<DailyEntry>>();
            mockRepository.Setup(repo => repo.UpdateAsync(entryId, testEntry)).Returns(Task.FromResult<DailyEntry>(testEntry));;

            var controller = new DailyEntryController(mockRepository.Object);

            var result = await controller.Update(entryId, testEntry);

            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = viewResult.Value;
            
            Assert.Equal(200, viewResult.StatusCode);
            Assert.Equal(model, $"The daily entry with id {entryId} has been updated");
        }
        
        [Fact]
        public async void WhenUpdateFunctionIsCalled_IfExceptionThrown_ReturnsBadRequestStatus()
        {
            string entryId = "test";
            var testEntry = new DailyEntry() {};

            var mockRepository = new Mock<IRepository<DailyEntry>>();
            mockRepository.Setup(repo => repo.UpdateAsync(entryId, testEntry)).Throws(new Exception());;

            var controller = new DailyEntryController(mockRepository.Object);

            var result = await controller.Update(entryId, testEntry);

            var viewResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal(400, viewResult.StatusCode);
        }

    }
}
