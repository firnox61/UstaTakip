using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UstaTakip.Application.DTOs.RepairJobs;
using UstaTakip.Application.Interfaces.Services.Contracts;

namespace UstaTakip.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepairJobsController : ControllerBase
    {
        private readonly IRepairJobService _repairJobService;

        public RepairJobsController(IRepairJobService repairJobService)
        {
            _repairJobService = repairJobService;
        }

        // GET api/repairjobs
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _repairJobService.GetAllAsync();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("update-dto/{id}")]
        public async Task<IActionResult> GetUpdateDto(Guid id)
        {
            var result = await _repairJobService.GetUpdateDtoAsync(id);
            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }

        // GET api/repairjobs/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _repairJobService.GetByIdAsync(id);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }

        // GET api/repairjobs/vehicle/{vehicleId}
        [HttpGet("vehicle/{vehicleId}")]
        public async Task<IActionResult> GetByVehicleId(Guid vehicleId)
        {
            var result = await _repairJobService.GetByVehicleIdAsync(vehicleId);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }

        // GET api/repairjobs/recent?take=10
        [HttpGet("recent")]
        public async Task<IActionResult> GetRecent([FromQuery] int take = 10)
        {
            var result = await _repairJobService.GetRecentAsync(take);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        // GET api/repairjobs/monthly-stats
        [HttpGet("monthly-stats")]
        public async Task<IActionResult> GetMonthlyStats()
        {
            var result = await _repairJobService.GetMonthlyStatsAsync();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        // POST api/repairjobs
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] RepairJobCreateDto dto)
        {
            var result = await _repairJobService.AddAsync(dto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        // PUT api/repairjobs
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] RepairJobUpdateDto dto)
        {
            var result = await _repairJobService.UpdateAsync(dto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        // DELETE api/repairjobs/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _repairJobService.DeleteAsync(id);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }

}
