using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Application.DTOs.Vehicles;
using UstaTakip.Application.Interfaces.Services.Contracts;
using UstaTakip.Application.Repositories;
using UstaTakip.Application.Validators.Vehicles;
using UstaTakip.Core.Aspects.Caching;
using UstaTakip.Core.Aspects.Transaction;
using UstaTakip.Core.Aspects.Validation;
using UstaTakip.Core.Utilities.Results;
using UstaTakip.Domain.Entities;

namespace UstaTakip.Application.Services.Managers
{
    public class VehicleManager : IVehicleService
    {
        private readonly IVehicleDal _vehicleDal;
        private readonly IMapper _mapper;

        public VehicleManager(IVehicleDal vehicleDal, IMapper mapper)
        {
            _vehicleDal = vehicleDal;
            _mapper = mapper;
        }

        [TransactionScopeAspect]
        [ValidationAspect(typeof(VehicleCreateDtoValidator))]
        [CacheRemoveAspect("IVehicleService.Get*")]
        public async Task<IResult> AddAsync(VehicleCreateDto dto)
        {
            var vehicle = _mapper.Map<Vehicle>(dto);
            await _vehicleDal.AddAsync(vehicle);
            return new SuccessResult("Araç başarıyla eklendi.");
        }

        [TransactionScopeAspect]
        [CacheRemoveAspect("IVehicleService.Get*")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _vehicleDal.DeleteAsync(new Vehicle { Id = id });
            return new SuccessResult("Araç silindi.");
        }

        [CacheAspect]
        public async Task<IDataResult<List<VehicleListDto>>> GetAllAsync()
        {
            var vehicles = await _vehicleDal.GetAllAsync();
            var dto = _mapper.Map<List<VehicleListDto>>(vehicles);
            return new SuccessDataResult<List<VehicleListDto>>(dto);
        }

        [CacheAspect]
        public async Task<IDataResult<VehicleListDto>> GetByIdAsync(Guid id)
        {
            var vehicle = await _vehicleDal.GetByIdWithDetailsAsync(id);
            if (vehicle == null)
                return new ErrorDataResult<VehicleListDto>("Araç bulunamadı.");

            var dto = _mapper.Map<VehicleListDto>(vehicle);
            return new SuccessDataResult<VehicleListDto>(dto);
        }

        [CacheAspect]
        public async Task<IDataResult<List<VehicleListDto>>> GetByCustomerIdAsync(Guid customerId)
        {
            var vehicles = await _vehicleDal.GetByCustomerIdWithDetailsAsync(customerId);
            var dto = _mapper.Map<List<VehicleListDto>>(vehicles);
            return new SuccessDataResult<List<VehicleListDto>>(dto);
        }

        [TransactionScopeAspect]
        [ValidationAspect(typeof(VehicleUpdateDtoValidator))]
        [CacheRemoveAspect("IVehicleService.Get*")]
        public async Task<IResult> UpdateAsync(VehicleUpdateDto dto)
        {
            var vehicle = _mapper.Map<Vehicle>(dto);
            await _vehicleDal.UpdateAsync(vehicle);
            return new SuccessResult("Araç güncellendi.");
        }

        [CacheAspect]
        public async Task<IDataResult<List<VehicleListDto>>> GetListAsync()
        {
            var vehicles = await _vehicleDal.GetAllWithDetailsAsync();
            var dtos = _mapper.Map<List<VehicleListDto>>(vehicles);
            return new SuccessDataResult<List<VehicleListDto>>(dtos);
        }
    }


}
