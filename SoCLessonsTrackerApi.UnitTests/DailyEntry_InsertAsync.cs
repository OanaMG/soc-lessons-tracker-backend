using System;
using Xunit;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

public class DailyEntry_UpdateAsync
{
    public class DailyEntry_InsertAsync
    {
        [Fact]
        public async Task WhenInsertFunctionIsCalled_IfValidId_ReturnsCreatedStatusAndInsertedEntry(){
            var entriesList = new List<DailyEntry>();
            entriesList.Add(new DailyEntry()
            {
                Id = "entryid1",
                Date = "2021-06-14",
                Topics = "Testing1, xunit1",
                RecapQuizScore = 8,
            });
            entriesList.Add(new DailyEntry()
            {
                Id = "entryid2",
                Date = "2021-06-19",
                Topics = "Testing2, xunit2",
                RecapQuizScore = 7,
            });

            DailyEntry testDailyEntry = new DailyEntry{
                Id = "entryid3",
                Date = "2021-06-24",
                Topics = "Testing3, xunit3",
                RecapQuizScore = 9,
            };

            var mockRepository = new Mock<IRepository<DailyEntry>>();

            mockRepository.Setup(repo => repo.InsertAsync(It.IsAny<DailyEntry>()))
                          .Callback(()=>entriesList.Add(testDailyEntry))
                          .Returns(()=>Task.FromResult(entriesList.LastOrDefault()));
            
            var controller = new DailyEntryController(mockRepository.Object);

            var result = await controller.Insert(testDailyEntry);

            mockRepository.Verify(x => x.InsertAsync(It.IsAny<DailyEntry>()), Times.Once); 

            var viewResult = Assert.IsType<CreatedResult>(result);
            var model = Assert.IsType<DailyEntry>(viewResult.Value);
            Assert.Equal(201, viewResult.StatusCode);
            Assert.Equal(testDailyEntry.Date, model.Date);
            Assert.Equal(testDailyEntry.RecapQuizScore, model.RecapQuizScore);
            Assert.Equal(3, entriesList.Count);
        }

        [Fact]
        public async void WhenInsertFunctionIsCalled_IfExceptionThrown_ReturnsBadRequestStatus()
        {
            var testDailyEntry = new DailyEntry() {};

            var mockRepository = new Mock<IRepository<DailyEntry>>();
            mockRepository.Setup(repo => repo.InsertAsync(testDailyEntry)).Throws(new Exception());

            var controller = new DailyEntryController(mockRepository.Object);

            var result = await controller.Insert(testDailyEntry);

            var viewResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal(400, viewResult.StatusCode);
        }
    }
}
