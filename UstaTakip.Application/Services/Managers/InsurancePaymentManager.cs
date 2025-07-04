using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Application.DTOs.InsurancePayments;
using UstaTakip.Application.Interfaces.Services.Contracts;
using UstaTakip.Application.Repositories;
using UstaTakip.Application.Validators.InsurancePayments;
using UstaTakip.Core.Aspects.Transaction;
using UstaTakip.Core.Aspects.Validation;
using UstaTakip.Core.Utilities.Results;
using UstaTakip.Domain.Entities;

namespace UstaTakip.Application.Services.Managers
{
    public class InsurancePaymentManager : IInsurancePaymentService
    {
        private readonly IInsurancePaymentDal _insurancePaymentDal;
        private readonly IMapper _mapper;

        public InsurancePaymentManager(IInsurancePaymentDal insurancePaymentDal, IMapper mapper)
        {
            _insurancePaymentDal = insurancePaymentDal;
            _mapper = mapper;
        }

        [ValidationAspect(typeof(InsurancePaymentCreateDtoValidator))]
        [TransactionScopeAspect]
        public async Task<IResult> AddAsync(InsurancePaymentCreateDto dto)
        {
            var entity = _mapper.Map<InsurancePayment>(dto);
            await _insurancePaymentDal.AddAsync(entity);
            return new SuccessResult("Sigorta ödemesi eklendi.");
        }

        [ValidationAspect(typeof(InsurancePaymentUpdateDtoValidator))]
        [TransactionScopeAspect]
        public async Task<IResult> UpdateAsync(InsurancePaymentUpdateDto dto)
        {
            var entity = _mapper.Map<InsurancePayment>(dto);
            await _insurancePaymentDal.UpdateAsync(entity);
            return new SuccessResult("Sigorta ödemesi güncellendi.");
        }

        [TransactionScopeAspect]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _insurancePaymentDal.DeleteAsync(new InsurancePayment { Id = id });
            return new SuccessResult("Sigorta ödemesi silindi.");
        }

        public async Task<IDataResult<List<InsurancePaymentListDto>>> GetAllAsync()
        {
            var list = await _insurancePaymentDal.GetAllWithDetailsAsync();
            var dto = _mapper.Map<List<InsurancePaymentListDto>>(list);
            return new SuccessDataResult<List<InsurancePaymentListDto>>(dto);
        }

        public async Task<IDataResult<InsurancePaymentListDto>> GetByIdAsync(Guid id)
        {
            var entity = await _insurancePaymentDal.GetByIdWithDetailsAsync(id);
            if (entity == null)
                return new ErrorDataResult<InsurancePaymentListDto>("Sigorta ödemesi bulunamadı.");

            var dto = _mapper.Map<InsurancePaymentListDto>(entity);
            return new SuccessDataResult<InsurancePaymentListDto>(dto);
        }

        public async Task<IDataResult<InsurancePaymentListDto>> GetByRepairJobIdAsync(Guid repairJobId)
        {
            var entity = await _insurancePaymentDal.GetByRepairJobIdAsync(repairJobId);
            if (entity == null)
                return new ErrorDataResult<InsurancePaymentListDto>("Sigorta ödemesi bulunamadı.");

            var dto = _mapper.Map<InsurancePaymentListDto>(entity);
            return new SuccessDataResult<InsurancePaymentListDto>(dto);
        }

        public async Task<IDataResult<List<InsurancePaymentListDto>>> GetByPolicyIdAsync(Guid insurancePolicyId)
        {
            var list = await _insurancePaymentDal.GetByPolicyIdWithDetailsAsync(insurancePolicyId);
            var dto = _mapper.Map<List<InsurancePaymentListDto>>(list);
            return new SuccessDataResult<List<InsurancePaymentListDto>>(dto);
        }
    }


}
