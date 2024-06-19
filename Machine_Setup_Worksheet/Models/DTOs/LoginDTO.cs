using System.ComponentModel.DataAnnotations;

namespace Machine_Setup_Worksheet.Models.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email can't be empty")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password can't be empty")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
