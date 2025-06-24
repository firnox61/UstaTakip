using AutoMapper;

using UstaTakip.Application.Interfaces.Services.Contracts;
using UstaTakip.Core.Utilities.Results;
using UstaTakip.Application.DTOs.Users;
using UstaTakip.Domain.Entities;
using UstaTakip.Application.Repositories;

namespace UstaTakip.Application.Services.Managers
{
    public class UserOperationClaimManager : IUserOperationClaimService
    {
        private readonly IUserOperationClaimDal _userOperationClaimDal;
        private readonly IMapper _mapper;

        public UserOperationClaimManager(IUserOperationClaimDal userOperationClaimDal, IMapper mapper)
        {
            _userOperationClaimDal = userOperationClaimDal;
            _mapper = mapper;
        }

        //[ValidationAspect(typeof(UserOperationClaimCreateDtoValidator))]
        public async Task<IResult> AddAsync(UserOperationClaimCreateDto dto)
        {
            var entity = _mapper.Map<UserOperationClaim>(dto);
            await _userOperationClaimDal.AddAsync(entity);
            return new SuccessResult("Kullanıcıya rol atandı.");
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var entity = await _userOperationClaimDal.GetAsync(x => x.Id == id);
            if (entity == null)
                return new ErrorResult("Rol ataması bulunamadı.");

            await _userOperationClaimDal.DeleteAsync(entity);
            return new SuccessResult("Rol ataması silindi.");
        }

        public async Task<IDataResult<List<UserOperationClaimListDto>>> GetByUserIdAsync(int userId)
        {
            var list = await _userOperationClaimDal.GetWithDetailsByUserIdAsync(userId);

            var dtoList = list.Select(x => new UserOperationClaimListDto
            {
                Id = x.Id,
                UserFullName = $"{x.User.FirstName} {x.User.LastName}",
                RoleName = x.OperationClaim.Name
            }).ToList();

            return new SuccessDataResult<List<UserOperationClaimListDto>>(dtoList);
        }
    }
}
