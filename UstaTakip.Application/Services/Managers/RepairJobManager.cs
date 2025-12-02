using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Application.DTOs.RepairJobs;
using UstaTakip.Application.Interfaces.Services.Contracts;
using UstaTakip.Application.Repositories;
using UstaTakip.Application.Validators.RepairJobs;
using UstaTakip.Application.Validators.Vehicles;
using UstaTakip.Core.Aspects.Caching;
using UstaTakip.Core.Aspects.Transaction;
using UstaTakip.Core.Aspects.Validation;
using UstaTakip.Core.Utilities.Results;
using UstaTakip.Domain.Entities;

namespace UstaTakip.Application.Services.Managers
{
    public class RepairJobManager : IRepairJobService
    {
        private readonly IRepairJobDal _repairJobDal;
        private readonly IMapper _mapper;

        public RepairJobManager(IRepairJobDal repairJobDal, IMapper mapper)
        {
            _repairJobDal = repairJobDal;
            _mapper = mapper;
        }

        [ValidationAspect(typeof(RepairJobCreateDtoValidator))]
        [TransactionScopeAspect]
        [CacheRemoveAspect("IRepairJobService.Get*")]
        public async Task<IResult> AddAsync(RepairJobCreateDto dto)
        {
            var job = _mapper.Map<RepairJob>(dto);
            await _repairJobDal.AddAsync(job);
            return new SuccessResult("İşlem kaydedildi.");
        }

        [TransactionScopeAspect]
        [CacheRemoveAspect("IRepairJobService.Get*")]
        public async Task<IResult> UpdateAsync(RepairJobUpdateDto dto)
        {
            var job = _mapper.Map<RepairJob>(dto);
            await _repairJobDal.UpdateAsync(job);
            return new SuccessResult("İşlem güncellendi.");
        }

        [CacheRemoveAspect("IRepairJobService.Get*")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _repairJobDal.DeleteAsync(new RepairJob { Id = id });
            return new SuccessResult("İşlem silindi.");
        }

        [CacheAspect]
        public async Task<IDataResult<List<RepairJobListDto>>> GetAllAsync()
        {
            var jobs = await _repairJobDal.GetAllWithVehicleAsync();
            var dto = _mapper.Map<List<RepairJobListDto>>(jobs);
            return new SuccessDataResult<List<RepairJobListDto>>(dto);
        }

        [CacheAspect]
        public async Task<IDataResult<RepairJobListDto>> GetByIdAsync(Guid id)
        {
            var job = await _repairJobDal.GetByIdWithVehicleAsync(id);
            if (job == null)
                return new ErrorDataResult<RepairJobListDto>("İşlem bulunamadı.");

            var dto = _mapper.Map<RepairJobListDto>(job);
            return new SuccessDataResult<RepairJobListDto>(dto);
        }

        [CacheAspect]
        public async Task<IDataResult<List<RepairJobListDto>>> GetByVehicleIdAsync(Guid vehicleId)
        {
            var jobs = await _repairJobDal.GetAllAsync(x => x.VehicleId == vehicleId);
            var dto = _mapper.Map<List<RepairJobListDto>>(jobs);
            return new SuccessDataResult<List<RepairJobListDto>>(dto);
        }
        [CacheAspect]
        public async Task<IDataResult<List<RepairJobListDto>>> GetRecentAsync(int take)
        {
            take = Math.Clamp(take, 1, 100);
            var entities = await _repairJobDal.GetRecentWithVehicleAsync(take);
            var dto = _mapper.Map<List<RepairJobListDto>>(entities);
            return new SuccessDataResult<List<RepairJobListDto>>(dto);
        }

        [CacheAspect]
        public async Task<IDataResult<List<MonthlyRepairJobDto>>> GetMonthlyStatsAsync()
        {
            var data = await _repairJobDal.GetMonthlyStatsAsync();
            return new SuccessDataResult<List<MonthlyRepairJobDto>>(data);
        }

    }


}
