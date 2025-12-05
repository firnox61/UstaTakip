using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Application.DTOs.InsurancePolicys;
using UstaTakip.Application.Interfaces.Services.Contracts;
using UstaTakip.Application.Repositories;
using UstaTakip.Application.Validators.InsurancePolicys;
using UstaTakip.Core.Aspects.Caching;
using UstaTakip.Core.Aspects.Transaction;
using UstaTakip.Core.Aspects.Validation;
using UstaTakip.Core.Utilities.Results;
using UstaTakip.Domain.Entities;

namespace UstaTakip.Application.Services.Managers
{
    public class InsurancePolicyManager : IInsurancePolicyService
    {
        private readonly IInsurancePolicyDal _insurancePolicyDal;
        private readonly IMapper _mapper;

        public InsurancePolicyManager(IInsurancePolicyDal insurancePolicyDal, IMapper mapper)
        {
            _insurancePolicyDal = insurancePolicyDal;
            _mapper = mapper;
        }

        [ValidationAspect(typeof(InsurancePolicyCreateDtoValidator))]
        [TransactionScopeAspect]
        public async Task<IResult> AddAsync(InsurancePolicyCreateDto dto)
        {
            var entity = _mapper.Map<InsurancePolicy>(dto);
            await _insurancePolicyDal.AddAsync(entity);
            return new SuccessResult("Sigorta poliçesi eklendi.");
        }

        [ValidationAspect(typeof(InsurancePolicyUpdateDtoValidator))]
        [TransactionScopeAspect]
        public async Task<IResult> UpdateAsync(InsurancePolicyUpdateDto dto)
        {
            var entity = await _insurancePolicyDal.GetAsync(x => x.Id == dto.Id);
            if (entity == null)
                return new ErrorResult("Poliçe bulunamadı.");

            _mapper.Map(dto, entity);
            await _insurancePolicyDal.UpdateAsync(entity);

            return new SuccessResult("Sigorta poliçesi güncellendi.");
        }

        [TransactionScopeAspect]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = await _insurancePolicyDal.GetAsync(x => x.Id == id);
            if (entity == null)
                return new ErrorResult("Poliçe bulunamadı.");

            await _insurancePolicyDal.DeleteAsync(entity);
            return new SuccessResult("Sigorta poliçesi silindi.");
        }

        public async Task<IDataResult<List<InsurancePolicyListDto>>> GetAllAsync()
        {
            var list = await _insurancePolicyDal.GetAllWithDetailsAsync();
            return new SuccessDataResult<List<InsurancePolicyListDto>>(
                _mapper.Map<List<InsurancePolicyListDto>>(list)
            );
        }

        public async Task<IDataResult<InsurancePolicyListDto>> GetByIdAsync(Guid id)
        {
            var entity = await _insurancePolicyDal.GetByIdWithDetailsAsync(id);
            if (entity == null)
                return new ErrorDataResult<InsurancePolicyListDto>("Poliçe bulunamadı.");

            return new SuccessDataResult<InsurancePolicyListDto>(
                _mapper.Map<InsurancePolicyListDto>(entity)
            );
        }

        public async Task<IDataResult<List<InsurancePolicyListDto>>> GetByVehicleIdAsync(Guid vehicleId)
        {
            var list = await _insurancePolicyDal.GetByVehicleIdWithDetailsAsync(vehicleId);
            return new SuccessDataResult<List<InsurancePolicyListDto>>(
                _mapper.Map<List<InsurancePolicyListDto>>(list)
            );
        }

        [CacheAspect]
        public async Task<IDataResult<List<InsurancePolicyListDto>>> GetExpiringAsync(int days, int take)
        {
            days = Math.Clamp(days, 1, 365);
            take = Math.Clamp(take, 1, 100);

            var list = await _insurancePolicyDal.GetExpiringAsync(DateTime.UtcNow, days, take);

            return new SuccessDataResult<List<InsurancePolicyListDto>>(
                _mapper.Map<List<InsurancePolicyListDto>>(list)
            );
        }

        [CacheAspect]
        public async Task<IDataResult<int>> GetActiveCountAsync()
        {
            return new SuccessDataResult<int>(await _insurancePolicyDal.GetActiveCountAsync());
        }
    }



}
