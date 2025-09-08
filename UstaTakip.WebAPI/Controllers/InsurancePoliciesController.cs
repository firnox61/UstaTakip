using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UstaTakip.Application.DTOs.InsurancePolicys;
using UstaTakip.Application.Interfaces.Services.Contracts;

namespace UstaTakip.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsurancePoliciesController : ControllerBase
    {
        private readonly IInsurancePolicyService _insurancePolicyService;

        public InsurancePoliciesController(IInsurancePolicyService insurancePolicyService)
        {
            _insurancePolicyService = insurancePolicyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _insurancePolicyService.GetAllAsync();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _insurancePolicyService.GetByIdAsync(id);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }

        [HttpGet("vehicle/{vehicleId}")]
        public async Task<IActionResult> GetByVehicleId(Guid vehicleId)
        {
            var result = await _insurancePolicyService.GetByVehicleIdAsync(vehicleId);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] InsurancePolicyCreateDto dto)
        {
            var result = await _insurancePolicyService.AddAsync(dto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] InsurancePolicyUpdateDto dto)
        {
            var result = await _insurancePolicyService.UpdateAsync(dto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _insurancePolicyService.DeleteAsync(id);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("expiring")]
        public async Task<IActionResult> GetExpiring([FromQuery] int days = 30, [FromQuery] int take = 5)
        {
            var result = await _insurancePolicyService.GetExpiringAsync(days, take);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        // GET /api/InsurancePolicies/active/count
        [HttpGet("active/count")]
        public async Task<IActionResult> GetActiveCount()
        {
            var result = await _insurancePolicyService.GetActiveCountAsync();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

    }
}
