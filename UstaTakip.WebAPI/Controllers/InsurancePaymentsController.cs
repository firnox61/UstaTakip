using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UstaTakip.Application.DTOs.InsurancePayments;
using UstaTakip.Application.Interfaces.Services.Contracts;

namespace UstaTakip.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsurancePaymentsController : ControllerBase
    {
        private readonly IInsurancePaymentService _insurancePaymentService;

        public InsurancePaymentsController(IInsurancePaymentService insurancePaymentService)
        {
            _insurancePaymentService = insurancePaymentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _insurancePaymentService.GetAllAsync();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _insurancePaymentService.GetByIdAsync(id);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }

        [HttpGet("policy/{policyId}")]
        public async Task<IActionResult> GetByPolicyId(Guid policyId)
        {
            var result = await _insurancePaymentService.GetByPolicyIdAsync(policyId);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }

        [HttpGet("repair-job/{repairJobId}")]
        public async Task<IActionResult> GetByRepairJobId(Guid repairJobId)
        {
            var result = await _insurancePaymentService.GetByRepairJobIdAsync(repairJobId);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] InsurancePaymentCreateDto dto)
        {
            var result = await _insurancePaymentService.AddAsync(dto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] InsurancePaymentUpdateDto dto)
        {
            var result = await _insurancePaymentService.UpdateAsync(dto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _insurancePaymentService.DeleteAsync(id);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
