using AutoMapper;
using UstaTakip.Application.DTOs.Users;
using UstaTakip.Application.Interfaces.Security;
using UstaTakip.Application.Interfaces.Services.Contracts;
using UstaTakip.Application.Repositories;
using UstaTakip.Application.Validators.Users;
using UstaTakip.Core.Aspects.Validation;
using UstaTakip.Core.Utilities.Results;
using UstaTakip.Domain.Entities;


namespace UstaTakip.Application.Services.Managers
{
    public class UserManager : IUserService
    {
        private readonly IUserDal _userDal;
        private readonly IMapper _mapper;
        private readonly IHashingService _hashingService;

        public UserManager(IUserDal userDal, IMapper mapper, IHashingService hashingService)
        {
            _userDal = userDal;
            _mapper = mapper;
            _hashingService = hashingService;
        }
        //[ValidationAspect(typeof(UserDtoValidator))]
        public async Task<IResult> EditProfil(UserDto userDto, string password)
        {
            byte[] passwordHash, passwordSalt;
            _hashingService.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            var user = _mapper.Map<User>(userDto);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _userDal.UpdateAsync(user);
            return new SuccessResult("Müşteri bilgileri güncellendi");
        }
       // [ValidationAspect(typeof(UserCreateDtoValidator))]
        public async Task<IResult> AddAsync(UserCreateDto dto)
        {
            var user = _mapper.Map<User>(dto);
            await _userDal.AddAsync(user);
            return new SuccessResult();
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var user = await _userDal.GetAsync(u => u.Id == id);
            if (user == null)
                return new ErrorResult("Kullanıcı bulunamadı.");

            await _userDal.DeleteAsync(user);
            return new SuccessResult("Kullanıcı silindi.");
        }

        public async Task<IDataResult<List<UserListDto>>> GetAllAsync()
        {
            var users = await _userDal.GetAllAsync();
            var dtoList = _mapper.Map<List<UserListDto>>(users);
            return new SuccessDataResult<List<UserListDto>>(dtoList);
        }

        public async Task<IDataResult<User>> GetByEmailAsync(string email)
        {
            var user = await _userDal.GetAsync(u => u.Email == email);
            if (user == null)
                return new ErrorDataResult<User>("Kullanıcı bulunamadı");

            return new SuccessDataResult<User>(user);
        }

        public async Task<IDataResult<UserListDto>> GetByIdAsync(int id)
        {
            var user = await _userDal.GetAsync(u => u.Id == id);
            if (user == null)
                return new ErrorDataResult<UserListDto>("Kullanıcı bulunamadı");

            var dto = _mapper.Map<UserListDto>(user);
            return new SuccessDataResult<UserListDto>(dto);
        }

        public async Task<IDataResult<List<OperationClaim>>> GetClaimsAsync(User user)
        {
            var claims = await _userDal.GetClaimsAsync(user); // DAL'de bu olmalı
            return new SuccessDataResult<List<OperationClaim>>(claims);
        }
       // [ValidationAspect(typeof(UserUpdateDtoValidator))]
        public async Task<IResult> UpdateAsync(UserUpdateDto dto)
        {
            var user = await _userDal.GetAsync(u => u.Id == dto.Id);
            if (user == null)
                return new ErrorResult("Kullanıcı bulunamadı");

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;
            user.Status = dto.Status;

            await _userDal.UpdateAsync(user);
            return new SuccessResult("Kullanıcı güncellendi");
        }
    }
}
