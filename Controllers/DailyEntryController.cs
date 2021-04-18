using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("daily-entries")]
    public class DailyEntryController : ControllerBase
    {
        private readonly IRepository<DailyEntry> _dailyEntryRepository;
        public DailyEntryController(IRepository<DailyEntry> dailyEntryRepository)
        {
            _dailyEntryRepository = dailyEntryRepository;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<DailyEntry>>> GetAll([FromQuery] string token, [FromQuery] string date, [FromQuery] string search)
        {
            if (!String.IsNullOrEmpty(token) && !String.IsNullOrEmpty(date) && String.IsNullOrEmpty(search)){
                
                var entries = await _dailyEntryRepository.GetByDateAndUserAsync(token, date);
                return Ok(entries);

            } else if (!String.IsNullOrEmpty(token) && String.IsNullOrEmpty(date) && !String.IsNullOrEmpty(search)) {
                
                var entries = await _dailyEntryRepository.GetBySearchAndUserAsync(token, search);  
                return Ok(entries);
            
            } else if ((!String.IsNullOrEmpty(token) && String.IsNullOrEmpty(date) && String.IsNullOrEmpty(search))){
                
                var entries = await _dailyEntryRepository.GetAllByUserAsync(token); 
                return Ok(entries);
            
            } else {
                //var entries = await _dailyEntryRepository.GetAllAsync();
                return NoContent();
            }
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<DailyEntry>> GetById(string id)
        {
            var entry = await _dailyEntryRepository.GetByIdAsync(id);
            if (entry == null)
            {
                return NotFound();
            }
            return Ok(entry);
        }

        [HttpPost]
        public async Task<IActionResult> Insert(DailyEntry dailyEntry)
        {
            await _dailyEntryRepository.InsertAsync(dailyEntry);
            return Ok(dailyEntry);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, DailyEntry updatedDailyEntry)
        {
            var queriedDailyEntry = await _dailyEntryRepository.GetByIdAsync(id);
            if(queriedDailyEntry == null)
            {
                return NotFound();
            }
            await _dailyEntryRepository.UpdateAsync(id, updatedDailyEntry);
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var dailyEntry = await _dailyEntryRepository.GetByIdAsync(id);
            if (dailyEntry == null)
            {
                return NotFound();
            }
            await _dailyEntryRepository.DeleteAsync(id);
            return NoContent();
        }
    }
