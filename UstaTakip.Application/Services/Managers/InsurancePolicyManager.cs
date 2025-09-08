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
        var entity = _mapper.Map<InsurancePolicy>(dto);
        await _insurancePolicyDal.UpdateAsync(entity);
        return new SuccessResult("Sigorta poliçesi güncellendi.");
    }

    [TransactionScopeAspect]
    public async Task<IResult> DeleteAsync(Guid id)
    {
        await _insurancePolicyDal.DeleteAsync(new InsurancePolicy { Id = id });
        return new SuccessResult("Sigorta poliçesi silindi.");
    }

    public async Task<IDataResult<List<InsurancePolicyListDto>>> GetAllAsync()
    {
        var list = await _insurancePolicyDal.GetAllWithDetailsAsync();
        var dto = _mapper.Map<List<InsurancePolicyListDto>>(list);
        return new SuccessDataResult<List<InsurancePolicyListDto>>(dto);
    }

    public async Task<IDataResult<InsurancePolicyListDto>> GetByIdAsync(Guid id)
    {
        var entity = await _insurancePolicyDal.GetByIdWithDetailsAsync(id);
        if (entity == null)
            return new ErrorDataResult<InsurancePolicyListDto>("Poliçe bulunamadı.");
        
        var dto = _mapper.Map<InsurancePolicyListDto>(entity);
        return new SuccessDataResult<InsurancePolicyListDto>(dto);
    }

    public async Task<IDataResult<List<InsurancePolicyListDto>>> GetByVehicleIdAsync(Guid vehicleId)
    {
        var list = await _insurancePolicyDal.GetByVehicleIdWithDetailsAsync(vehicleId);
        var dto = _mapper.Map<List<InsurancePolicyListDto>>(list);
        return new SuccessDataResult<List<InsurancePolicyListDto>>(dto);
    }
        [CacheAspect]
        public async Task<IDataResult<List<InsurancePolicyListDto>>> GetExpiringAsync(int days, int take)
        {
            days = Math.Clamp(days, 1, 365);
            take = Math.Clamp(take, 1, 100);

            var list = await _insurancePolicyDal.GetExpiringAsync(DateTime.UtcNow, days, take);

            // Map (AutoMapper kullanmıyorsan manuel doldur)
            var dto = list.Select(p => new InsurancePolicyListDto
            {
                Id = p.Id,
                CompanyName = p.CompanyName,
                PolicyNumber = p.PolicyNumber,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                CoverageAmount = p.CoverageAmount,
                VehicleId = p.VehicleId
            }).ToList();

            return new SuccessDataResult<List<InsurancePolicyListDto>>(dto);
        }

        [CacheAspect]
        public async Task<IDataResult<int>> GetActiveCountAsync()
        {
            var count = await _insurancePolicyDal.GetActiveCountAsync();
            return new SuccessDataResult<int>(count);
        }
    }


}
