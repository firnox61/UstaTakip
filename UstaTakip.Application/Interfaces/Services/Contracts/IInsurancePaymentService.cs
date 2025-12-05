using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Application.DTOs.InsurancePayments;
using UstaTakip.Core.Utilities.Results;

namespace UstaTakip.Application.Interfaces.Services.Contracts
{
    public interface IInsurancePaymentService
    {
        Task<IDataResult<List<InsurancePaymentListDto>>> GetAllAsync();
        Task<IDataResult<InsurancePaymentListDto>> GetByIdAsync(Guid id);

        Task<IDataResult<List<InsurancePaymentListDto>>> GetByRepairJobIdAsync(Guid repairJobId);
        Task<IDataResult<List<InsurancePaymentListDto>>> GetByPolicyIdAsync(Guid insurancePolicyId);

        Task<IResult> AddAsync(InsurancePaymentCreateDto dto);
        Task<IResult> UpdateAsync(InsurancePaymentUpdateDto dto);
        Task<IResult> DeleteAsync(Guid id);
    }


}
