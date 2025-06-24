using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Application.DTOs.RepairJobs;
using UstaTakip.Application.Interfaces.Services.Contracts;
using UstaTakip.Application.Repositories;
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

        public async Task<IResult> AddAsync(RepairJobCreateDto dto)
        {
            var job = _mapper.Map<RepairJob>(dto);
            await _repairJobDal.AddAsync(job);
            return new SuccessResult("İşlem kaydedildi.");
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _repairJobDal.DeleteAsync(new RepairJob { Id = id });
            return new SuccessResult("İşlem silindi.");
        }

        public async Task<IDataResult<List<RepairJobListDto>>> GetAllAsync()
        {
            var jobs = await _repairJobDal.GetAllAsync();
            var dto = _mapper.Map<List<RepairJobListDto>>(jobs);
            return new SuccessDataResult<List<RepairJobListDto>>(dto);
        }

        public async Task<IDataResult<RepairJobListDto>> GetByIdAsync(Guid id)
        {
            var job = await _repairJobDal.GetAsync(x => x.Id == id);
            if (job == null)
                return new ErrorDataResult<RepairJobListDto>("İşlem bulunamadı.");

            var dto = _mapper.Map<RepairJobListDto>(job);
            return new SuccessDataResult<RepairJobListDto>(dto);
        }

        public async Task<IDataResult<List<RepairJobListDto>>> GetByVehicleIdAsync(Guid vehicleId)
        {
            var jobs = await _repairJobDal.GetAllAsync(x => x.VehicleId == vehicleId);
            var dto = _mapper.Map<List<RepairJobListDto>>(jobs);
            return new SuccessDataResult<List<RepairJobListDto>>(dto);
        }

        public async Task<IResult> UpdateAsync(RepairJobUpdateDto dto)
        {
            var job = _mapper.Map<RepairJob>(dto);
            await _repairJobDal.UpdateAsync(job);
            return new SuccessResult("İşlem güncellendi.");
        }
    }

}
