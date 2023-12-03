using Service.AuthAPI.Models;
using Service.AuthAPI.Models.Dto;

namespace Service.AuthAPI.Service.IService
{
    public interface IJwtTokenGenerator
    {
       string GenerateToken(ApplicationUser user);
    }
}
