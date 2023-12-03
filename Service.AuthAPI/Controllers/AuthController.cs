using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Service.AuthAPI.Data;
using Service.AuthAPI.Models.Dto;
using Service.AuthAPI.Service.IService;

namespace Service.AuthAPI.Controllers
{
    [ApiController]
    [Route("/v1/api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ILogger<AuthController> _logger;
        private ResponseDto _response;
        private IMapper _mapper;
        private IAuthService _authService;

        public AuthController(AppDbContext db, ILogger<AuthController> logger, IMapper mapper, IAuthService authService)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
            _authService = authService;
            _response = new();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            var errorMessage = await _authService.Register(model);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _response.IsSuccess = false;
                _response.Message = errorMessage;
                return BadRequest(_response);
            }
            return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var loginResponse = await _authService.Login(model);
            if (loginResponse.User is null)
            {
                _response.IsSuccess = false;
                _response.Message = "username or password is incorrect";
                return BadRequest(_response);
            }
            _response.Result = loginResponse;
            return Ok(_response);
        }
    }
}
