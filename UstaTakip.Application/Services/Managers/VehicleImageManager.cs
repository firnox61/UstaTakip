using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Application.DTOs.VehicleImages;
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
    public class VehicleImageManager : IVehicleImageService
    {
        private readonly IVehicleImageDal _vehicleImageDal;
        private readonly IMapper _mapper;

        public VehicleImageManager(IVehicleImageDal vehicleImageDal, IMapper mapper)
        {
            _vehicleImageDal = vehicleImageDal;
            _mapper = mapper;
        }

        [TransactionScopeAspect]
        [ValidationAspect(typeof(VehicleImageCreateDtoValidator))]
        [CacheRemoveAspect("IVehicleImageService.Get*")]
        public async Task<IResult> AddAsync(VehicleImage entity)
        {
            await _vehicleImageDal.AddAsync(entity);
            return new SuccessResult("Araç resmi başarıyla eklendi.");
        }

        [TransactionScopeAspect]
        [CacheRemoveAspect("IVehicleImageService.Get*")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = await _vehicleImageDal.GetAsync(x => x.Id == id);
            if (entity == null) return new ErrorResult("Resim bulunamadı.");

            await _vehicleImageDal.DeleteAsync(entity);
            return new SuccessResult("Araç resmi silindi.");
        }

        [CacheAspect]
        public async Task<IDataResult<List<VehicleImageListDto>>> GetByVehicleIdAsync(Guid vehicleId)
        {
            var images = await _vehicleImageDal.GetAllAsync(x => x.VehicleId == vehicleId);
            var dto = _mapper.Map<List<VehicleImageListDto>>(images);
            return new SuccessDataResult<List<VehicleImageListDto>>(dto);
        }

        [CacheAspect]
        public async Task<IDataResult<VehicleImageListDto>> GetByIdAsync(Guid id)
        {
            var image = await _vehicleImageDal.GetAsync(x => x.Id == id);
            if (image == null) return new ErrorDataResult<VehicleImageListDto>("Resim bulunamadı.");

            var dto = _mapper.Map<VehicleImageListDto>(image);
            return new SuccessDataResult<VehicleImageListDto>(dto);
        }
    }



}
