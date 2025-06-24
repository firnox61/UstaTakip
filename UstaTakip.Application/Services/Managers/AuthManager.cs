using AutoMapper;
using UstaTakip.Application.DTOs.Users;
using UstaTakip.Application.Interfaces.Security;
using UstaTakip.Application.Interfaces.Services.Contracts;
using UstaTakip.Application.Repositories;
using UstaTakip.Core.Utilities.Results;
using UstaTakip.Domain.Entities;
using UstaTakip.Domain.Security;



namespace UstaTakip.Application.Services.Managers
{
    public class AuthManager : IAuthService
    {
        private readonly IUserService _userService;
        private readonly ITokenHelper _tokenHelper;
        private readonly IMapper _mapper;
        private readonly IUserDal _userDal;
        private readonly IHashingService _hashingService;
        public AuthManager(IUserService userService, ITokenHelper tokenHelper, IMapper mapper, IUserDal userDal, IHashingService hashingService)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
            _mapper = mapper;
            _userDal = userDal;
            _hashingService = hashingService;
        }

        public async Task<IDataResult<AccessToken>> CreateAccessTokenAsync(User user)
        {
            var claims = await _userService.GetClaimsAsync(user);
            var accessToken = _tokenHelper.CreateToken(user, claims.Data);
            return new SuccessDataResult<AccessToken>(accessToken);
        }

        public async Task<IDataResult<User>> LoginAsync(UserForLoginDto loginDto)
        {
            var userResult = await _userService.GetByEmailAsync(loginDto.Email);
            if (userResult.Data == null)
                return new ErrorDataResult<User>("Kullanıcı bulunamadı");

            var user = userResult.Data;

            bool isPasswordValid = _hashingService.VerifyPasswordHash(
                loginDto.Password,
                user.PasswordHash,
                user.PasswordSalt
            );

            if (!isPasswordValid)
                return new ErrorDataResult<User>("Şifre hatalı");

            return new SuccessDataResult<User>(user, "Giriş başarılı");
        }

        public async Task<IDataResult<User>> RegisterAsync(UserForRegisterDto registerDto, string password)
        {
            byte[] passwordHash, passwordSalt;
            _hashingService.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            var user = _mapper.Map<User>(registerDto);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Status = true;

            // await _userService.AddAsync(_mapper.Map<UserCreateDto>(user)); // İstersen doğrudan user da yollayabilirsin
            await _userDal.AddAsync(user); // DTO'ya dönüştürmeden direkt entity
            return new SuccessDataResult<User>(user, "Kayıt başarılı");
        }

        public async Task<IResult> UserExistsAsync(string email)
        {
            var userResult = await _userService.GetByEmailAsync(email);
            if (userResult.Data != null)
                return new ErrorResult("Kullanıcı zaten var");
            return new SuccessResult();
        }
    }
}
