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
            var job = await _repairJobDal.GetAsync(x => x.Id == dto.Id);
            if (job == null)
                return new ErrorResult("İşlem bulunamadı.");

            _mapper.Map(dto, job);
            await _repairJobDal.UpdateAsync(job);

            return new SuccessResult("İşlem güncellendi.");
        }

        [CacheRemoveAspect("IRepairJobService.Get*")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var job = await _repairJobDal.GetAsync(x => x.Id == id);
            if (job == null)
                return new ErrorResult("İşlem bulunamadı.");

            await _repairJobDal.DeleteAsync(job);
            return new SuccessResult("İşlem silindi.");
        }

        [CacheAspect]
        public async Task<IDataResult<List<RepairJobListDto>>> GetAllAsync()
        {
            var jobs = await _repairJobDal.GetAllWithVehicleAsync();
            return new SuccessDataResult<List<RepairJobListDto>>(
                _mapper.Map<List<RepairJobListDto>>(jobs)
            );
        }

        [CacheAspect]
        public async Task<IDataResult<RepairJobListDto>> GetByIdAsync(Guid id)
        {
            var job = await _repairJobDal.GetByIdWithVehicleAsync(id);
            if (job == null)
                return new ErrorDataResult<RepairJobListDto>("İşlem bulunamadı.");

            return new SuccessDataResult<RepairJobListDto>(
                _mapper.Map<RepairJobListDto>(job)
            );
        }
        public async Task<IDataResult<RepairJobUpdateDto>> GetUpdateDtoAsync(Guid id)
        {
            var job = await _repairJobDal.GetByIdWithVehicleAndPolicyAsync(id);
            if (job == null)
                return new ErrorDataResult<RepairJobUpdateDto>("İşlem bulunamadı.");

            return new SuccessDataResult<RepairJobUpdateDto>(
                _mapper.Map<RepairJobUpdateDto>(job)
            );
        }

        [CacheAspect]
        public async Task<IDataResult<List<RepairJobListDto>>> GetByVehicleIdAsync(Guid vehicleId)
        {
            var jobs = await _repairJobDal.GetAllWithVehicleAsync(); // daha doğru Include
            var filtered = jobs.Where(j => j.VehicleId == vehicleId).ToList();

            return new SuccessDataResult<List<RepairJobListDto>>(
                _mapper.Map<List<RepairJobListDto>>(filtered)
            );
        }

        [CacheAspect]
        public async Task<IDataResult<List<RepairJobListDto>>> GetRecentAsync(int take)
        {
            var entities = await _repairJobDal.GetRecentWithVehicleAsync(take);
            return new SuccessDataResult<List<RepairJobListDto>>(
                _mapper.Map<List<RepairJobListDto>>(entities)
            );
        }

        [CacheAspect]
        public async Task<IDataResult<List<MonthlyRepairJobStatsDto>>> GetMonthlyStatsAsync()
        {
            var data = await _repairJobDal.GetMonthlyStatsAsync();
            return new SuccessDataResult<List<MonthlyRepairJobStatsDto>>(data);
        }

    }



}
