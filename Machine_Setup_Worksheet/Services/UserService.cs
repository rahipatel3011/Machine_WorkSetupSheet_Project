using Machine_Setup_Worksheet.Controllers;
using Machine_Setup_Worksheet.CustomExceptions;
using Machine_Setup_Worksheet.Models;
using Machine_Setup_Worksheet.Models.DTOs;
using Machine_Setup_Worksheet.Services.IServices;
using Microsoft.AspNetCore.Identity;

namespace Machine_Setup_Worksheet.Services
{
    public class UserService : IUserService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
        {

            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public async Task<IdentityResult> CreateUserAsync(RegisterDTO registerDTO)
        {
            try
            {
                ApplicationUser user = new ApplicationUser()
                {
                    Name = registerDTO.Name,
                    Email = registerDTO.Email,
                    UserName = registerDTO.Email,
                };

                IdentityResult identityResult = await _userManager.CreateAsync(user, registerDTO.Password);
                return identityResult;
            }catch(Exception ex)
            {
                throw new ServiceException("Error while creaing new user", ex);
            }
        }




        public async Task<bool> LoginUserAsync(LoginDTO loginDTO)
        {
            try
            {
                ApplicationUser? applicationUser = await _userManager.FindByEmailAsync(loginDTO.Email);
                if (applicationUser != null)
                {
                    bool isCreadentialTrue = await _userManager.CheckPasswordAsync(applicationUser, loginDTO.Password);
                    if (isCreadentialTrue)
                    {
                        await _signInManager.SignInAsync(applicationUser, false);
                        return true;
                    }
                    
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error while login a user", ex);
            }
        }

        public async Task LogoutUserAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
