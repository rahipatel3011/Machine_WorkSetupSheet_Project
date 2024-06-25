using Microsoft.AspNetCore.Authorization;

namespace Machine_Setup_Worksheet.Attributes
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params Machine_Setup_Worksheet.Utility.Roles[] roles) { 
            Roles = string.Join(",", roles);
        }
    }
}
