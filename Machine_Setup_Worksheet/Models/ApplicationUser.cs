using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Machine_Setup_Worksheet.Models
{
    public class ApplicationUser: IdentityUser<Guid>
    {
        [Required]
        public string? Name {  get; set; }

    }
}
