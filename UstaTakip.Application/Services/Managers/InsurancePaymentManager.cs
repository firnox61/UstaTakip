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

        public async Task<IDataResult<List<InsurancePaymentListDto>>> GetAllAsync()
        {
            var list = await _insurancePaymentDal.GetAllWithDetailsAsync();
            var mapped = _mapper.Map<List<InsurancePaymentListDto>>(list);

            return new SuccessDataResult<List<InsurancePaymentListDto>>(mapped);
        }

        public async Task<IDataResult<InsurancePaymentListDto>> GetByIdAsync(Guid id)
        {
            var entity = await _insurancePaymentDal.GetByIdWithDetailsAsync(id);
            if (entity == null)
                return new ErrorDataResult<InsurancePaymentListDto>("Kayıt bulunamadı.");

            return new SuccessDataResult<InsurancePaymentListDto>(
                _mapper.Map<InsurancePaymentListDto>(entity));
        }

        public async Task<IDataResult<List<InsurancePaymentListDto>>> GetByRepairJobIdAsync(Guid repairJobId)
        {
            var list = await _insurancePaymentDal.GetByRepairJobIdAsync(repairJobId);
            var mapped = _mapper.Map<List<InsurancePaymentListDto>>(list);

            return new SuccessDataResult<List<InsurancePaymentListDto>>(mapped);
        }

        public async Task<IDataResult<List<InsurancePaymentListDto>>> GetByPolicyIdAsync(Guid policyId)
        {
            var list = await _insurancePaymentDal.GetByPolicyIdWithDetailsAsync(policyId);
            var mapped = _mapper.Map<List<InsurancePaymentListDto>>(list);

            return new SuccessDataResult<List<InsurancePaymentListDto>>(mapped);
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
            var existing = await _insurancePaymentDal.GetAsync(x => x.Id == dto.Id);
            if (existing == null)
                return new ErrorResult("Kayıt bulunamadı.");

            _mapper.Map(dto, existing);

            await _insurancePaymentDal.UpdateAsync(existing);

            return new SuccessResult("Sigorta ödemesi güncellendi.");
        }

        [TransactionScopeAspect]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = await _insurancePaymentDal.GetAsync(x => x.Id == id);
            if (entity == null)
                return new ErrorResult("Kayıt bulunamadı.");

            await _insurancePaymentDal.DeleteAsync(entity);

            return new SuccessResult("Sigorta ödemesi silindi.");
        }
    }



}
