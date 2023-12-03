using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Service.AuthAPI.Data;
using Service.AuthAPI.Models;
using Service.AuthAPI.Models.Dto;
using Service.AuthAPI.Service.IService;

namespace Service.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AuthService> _logger;
        private readonly IJwtTokenGenerator _jwtToken;
        public AuthService(AppDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<AuthService> logger, IJwtTokenGenerator jwtToken)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
            _jwtToken = jwtToken;
        }
        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            try
            {
                var user = _db.ApplicationUsers.FirstOrDefault(x => x.UserName.ToLower() == loginRequestDto.UserName.ToLower());
                bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if (user == null || isValid == false)
                {
                    return new LoginResponseDto() { Token = string.Empty };
                }

                UserDto userDto = new()
                {
                    Email = user.Email,
                    ID = user.Id,
                    Name = user.Name,
                    PhoneNumber = user.PhoneNumber,
                };

                LoginResponseDto loginResponseDto = new()
                {
                    User = userDto,
                    Token = string.Empty,
                };

                loginResponseDto.Token = _jwtToken.GenerateToken(user);
                return loginResponseDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new LoginResponseDto() { Token = string.Empty };
            }
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = new()
            {
                Name = registrationRequestDto.Name,
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                PhoneNumber = registrationRequestDto.PhoneNumber,
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);
                if (result.Succeeded)
                {
                    var userToReturn = _db.ApplicationUsers.First(x => x.UserName == registrationRequestDto.Email);
                    UserDto userDto = new()
                    {
                        Email = userToReturn.Email,
                        ID = userToReturn.Id,
                        Name = userToReturn.Name,
                        PhoneNumber = userToReturn.PhoneNumber,
                    };
                    return "";
                }
                else
                {
                    return result.Errors.First().Description;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return "Internal Error";
        }
    }
}
