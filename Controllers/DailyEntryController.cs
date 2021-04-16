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
        private readonly DailyEntryService _dailyEntryService;
        public DailyEntryController(DailyEntryService service)
        {
            _dailyEntryService = service;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<DailyEntry>>> GetAll([FromQuery] string token, [FromQuery] string date, [FromQuery] string search)
        {
            if (!String.IsNullOrEmpty(token) && !String.IsNullOrEmpty(date) && String.IsNullOrEmpty(search)){
                
                var entries = await _dailyEntryService.GetByDateAndUserAsync(token, date);
                return Ok(entries);

            } else if (!String.IsNullOrEmpty(token) && String.IsNullOrEmpty(date) && !String.IsNullOrEmpty(search)) {
                
                var entries = await _dailyEntryService.GetBySearchAndUserAsync(token, search);  
                return Ok(entries);
            
            } else if ((!String.IsNullOrEmpty(token) && String.IsNullOrEmpty(date) && String.IsNullOrEmpty(search))){
                
                var entries = await _dailyEntryService.GetAllByUserAsync(token); 
                return Ok(entries);
            
            } else {
                //var entries = await _dailyEntryService.GetAllAsync();
                return NoContent();
            }
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<DailyEntry>> GetById(string id)
        {
            var entry = await _dailyEntryService.GetByIdAsync(id);
            if (entry == null)
            {
                return NotFound();
            }
            return Ok(entry);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DailyEntry dailyEntry)
        {
            await _dailyEntryService.CreateAsync(dailyEntry);
            return Ok(dailyEntry);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, DailyEntry updatedDailyEntry)
        {
            var queriedDailyEntry = await _dailyEntryService.GetByIdAsync(id);
            if(queriedDailyEntry == null)
            {
                return NotFound();
            }
            await _dailyEntryService.UpdateAsync(id, updatedDailyEntry);
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var dailyEntry = await _dailyEntryService.GetByIdAsync(id);
            if (dailyEntry == null)
            {
                return NotFound();
            }
            await _dailyEntryService.DeleteAsync(id);
            return NoContent();
        }
    }
