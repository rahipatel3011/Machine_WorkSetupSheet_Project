using Machine_Setup_Worksheet.CustomExceptions;
using Machine_Setup_Worksheet.Models;
using Machine_Setup_Worksheet.Models.DTOs;
using Machine_Setup_Worksheet.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

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




        /// <summary>
        /// Create Application User
        /// </summary>
        /// <param name="registerDTO">registerDTO</param>
        /// <returns>IdentityResult</returns>
        /// <exception cref="ServiceException">ServiceException</exception>
        public async Task<IdentityResult> CreateUserAsync(RegisterDTO registerDTO)
        {
            try
            {
                ApplicationUser user = new ApplicationUser()
                {
                    Name = registerDTO.Name,
                    Email = registerDTO.Email,
                    UserName = registerDTO.Email.Split("@")[0],
                };

                IdentityResult identityResult = await _userManager.CreateAsync(user, registerDTO.Password);
                return identityResult;
            }catch(Exception ex)
            {
                throw new ServiceException("Error while creaing new user", ex);
            }
        }



        /// <summary>
        /// Login Application User
        /// </summary>
        /// <param name="loginDTO">loginDTO</param>
        /// <returns>True/False</returns>
        /// <exception cref="ServiceException">ServiceException</exception>
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



        /// <summary>
        /// Logout User
        /// </summary>
        /// <returns>void</returns>
        public async Task LogoutUserAsync()
        {
            await _signInManager.SignOutAsync();
        }




        /// <summary>
        /// Assign as role to specific user with given emailId
        /// </summary>
        /// <param name="email">mail Address</param>
        /// <param name="roleName">Role Name</param>
        /// <returns>IdentityResult</returns>
        /// <exception cref="ServiceException">ServiceException</exception>
        public async Task<IdentityResult> AssignRoleAsync(string email, string roleName)
        {
            try
            {
                ApplicationRole? applicationRole = await _roleManager.FindByNameAsync(roleName);
                if (applicationRole == null)
                {
                    //create role
                    ApplicationRole createdApplicationRole = new ApplicationRole() { Name = roleName };
                    IdentityResult identityResult = await _roleManager.CreateAsync(createdApplicationRole);
                    if (!identityResult.Succeeded)
                    {
                        return identityResult;
                    }
                }
                ApplicationUser? user = await _userManager.FindByEmailAsync(email);
                IdentityResult identityResultAfterCreatingRole = await _userManager.AddToRoleAsync(user, roleName);
                return identityResultAfterCreatingRole;
            }catch (Exception ex)
            {
                throw new ServiceException("Error while assigning a role", ex);
            }
        }


        
    }
}
