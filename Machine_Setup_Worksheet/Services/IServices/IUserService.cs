using Machine_Setup_Worksheet.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Machine_Setup_Worksheet.Services.IServices
{
    public interface IUserService
    {
        /// <summary>
        /// Create Application User
        /// </summary>
        /// <param name="registerDTO">registerDTO</param>
        /// <returns>IdentityResult</returns>
        Task<IdentityResult> CreateUserAsync(RegisterDTO registerDTO);


        /// <summary>
        /// Login Application User
        /// </summary>
        /// <param name="loginDTO">loginDTO</param>
        /// <returns>True/False</returns>
        Task<bool> LoginUserAsync(LoginDTO loginDTO);


        /// <summary>
        /// Logout User
        /// </summary>
        /// <returns>void</returns>
        Task LogoutUserAsync();



        /// <summary>
        /// Assign as role to specific user with given emailId
        /// </summary>
        /// <param name="email">mail Address</param>
        /// <param name="roleName">Role Name</param>
        /// <returns>IdentityResult</returns>
        Task<IdentityResult> AssignRoleAsync(string email, string roleName);


    }
}
