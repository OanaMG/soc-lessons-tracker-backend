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
        public async Task<IActionResult> GetAll([FromQuery] string token, [FromQuery] string date, [FromQuery] string search)
        {
            if (!String.IsNullOrEmpty(token) && !String.IsNullOrEmpty(date) && String.IsNullOrEmpty(search)){
                try {
                    var entries = await _dailyEntryRepository.GetByDateAndUserAsync(token, date);
                    return Ok(entries);
                }
                catch (Exception){
                    return NotFound($"The user has no entries for this date {date}");
                }

            } else if (!String.IsNullOrEmpty(token) && String.IsNullOrEmpty(date) && !String.IsNullOrEmpty(search)) {
                try {
                    var entries = await _dailyEntryRepository.GetBySearchAndUserAsync(token, search);  
                    return Ok(entries);
                }
                catch (Exception) {
                    return NotFound("There are no entries that match this search");
                }
            
            } else if ((!String.IsNullOrEmpty(token) && String.IsNullOrEmpty(date) && String.IsNullOrEmpty(search))){
                try {
                    var entries = await _dailyEntryRepository.GetAllByUserAsync(token); 
                    return Ok(entries);
                }
                catch {
                    return NotFound("There are no entries for this user");
                }
            } else {
                //var entries = await _dailyEntryRepository.GetAllAsync();
                return NoContent();
            }
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(string id)
        {
            try {
                var entry = await _dailyEntryRepository.GetByIdAsync(id);
                return Ok(entry);
            }
            catch (Exception){
                return NotFound($"There is no daily entry with id {id}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Insert(DailyEntry dailyEntry)
        {
            try {
                var newDailyEntry = await _dailyEntryRepository.InsertAsync(dailyEntry);
                return Created($"/daily-entries/{dailyEntry.Id}", newDailyEntry);
            }
            catch (Exception){
                return BadRequest();
            }
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, DailyEntry updatedDailyEntry)
        {
            try {
                await _dailyEntryRepository.UpdateAsync(id, updatedDailyEntry);
                return Ok($"The daily entry with id {id} has been updated");  
            }
            catch (Exception) {
                return BadRequest();
            }
        }
        
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try {
                _dailyEntryRepository.DeleteAsync(id);
                return Ok($"The daily entry with id {id} has been deleted");
            }
            catch (Exception) {
                return NotFound($"There is no daily entry with id {id}");
            }
        }
    }
