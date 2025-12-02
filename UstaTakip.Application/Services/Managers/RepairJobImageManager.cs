using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Application.DTOs.RepairJobImage;
using UstaTakip.Application.Interfaces.Services.Contracts;
using UstaTakip.Application.Repositories;
using UstaTakip.Core.Aspects.Caching;
using UstaTakip.Core.Aspects.Transaction;
using UstaTakip.Core.Utilities.Results;
using UstaTakip.Domain.Entities;

namespace UstaTakip.Application.Services.Managers
{
    public class RepairJobImageManager : IRepairJobImageService
    {
        private readonly IRepairJobImageDal _repairJobImageDal;
        private readonly IMapper _mapper;

        public RepairJobImageManager(IRepairJobImageDal repairJobImageDal, IMapper mapper)
        {
            _repairJobImageDal = repairJobImageDal;
            _mapper = mapper;
        }

        [TransactionScopeAspect]
        [CacheRemoveAspect("IRepairJobImageService.Get*")]
        public async Task<IResult> AddAsync(RepairJobImage entity)
        {
            await _repairJobImageDal.AddAsync(entity);
            return new SuccessResult("Tamir resmi eklendi.");
        }

        [TransactionScopeAspect]
        [CacheRemoveAspect("IRepairJobImageService.Get*")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = await _repairJobImageDal.GetAsync(x => x.Id == id);
            if (entity == null)
                return new ErrorResult("Resim bulunamadı.");

            await _repairJobImageDal.DeleteAsync(entity);
            return new SuccessResult("Tamir resmi silindi.");
        }

        [CacheAspect]
        public async Task<IDataResult<List<RepairJobImageListDto>>> GetByRepairJobIdAsync(Guid jobId)
        {
            var images = await _repairJobImageDal.GetAllAsync(x => x.RepairJobId == jobId);
            var dto = _mapper.Map<List<RepairJobImageListDto>>(images);

            return new SuccessDataResult<List<RepairJobImageListDto>>(dto);
        }

        [CacheAspect]
        public async Task<IDataResult<RepairJobImageListDto>> GetByIdAsync(Guid id)
        {
            var img = await _repairJobImageDal.GetAsync(x => x.Id == id);
            if (img == null)
                return new ErrorDataResult<RepairJobImageListDto>("Resim bulunamadı.");

            var dto = _mapper.Map<RepairJobImageListDto>(img);
            return new SuccessDataResult<RepairJobImageListDto>(dto);
        }
    }

}
