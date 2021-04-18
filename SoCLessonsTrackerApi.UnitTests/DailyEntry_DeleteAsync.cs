using System;
using Xunit;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace SoCLessonsTrackerApi.UnitTests
{
    public class DailyEntry_DeleteAsync
    {
        [Fact]
        public void WhenDeleteFunctionIsCalled_IfValidId_ReturnsOkStatus()
        {
            string entryId = "test";
            var mockRepository = new Mock<IRepository<DailyEntry>>();
            mockRepository.Setup(repo => repo.DeleteAsync(entryId));

            var controller = new DailyEntryController(mockRepository.Object);
            
            var result = controller.Delete(entryId);
            
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = viewResult.Value;

            mockRepository.Verify(repo => repo.DeleteAsync(entryId), Times.Once);
            Assert.Equal(200, viewResult.StatusCode);
            Assert.Equal(model, $"The daily entry with id {entryId} has been deleted");
        }
        
        [Fact]
        public void WhenDeleteFunctionIsCalled_IfExceptionThrown_ReturnsNotFoundStatus()
        {
            string entryId = "test";
            var mockRepository = new Mock<IRepository<DailyEntry>>();
            mockRepository.Setup(repo => repo.DeleteAsync(entryId)).Throws(new Exception());

            var controller = new DailyEntryController(mockRepository.Object);

            var result = controller.Delete(entryId);

            var viewResult = Assert.IsType<NotFoundObjectResult>(result);
            var model = viewResult.Value;

            Assert.Equal(404, viewResult.StatusCode);
            Assert.Equal(model, $"There is no daily entry with id {entryId}");
        }

    }
}
