using Machine_Setup_Worksheet.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Machine_Setup_Worksheet.Services.IServices
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUserAsync(RegisterDTO registerDTO);
        Task<bool> LoginUserAsync(LoginDTO loginDTO);
        Task LogoutUserAsync();
    }
}
