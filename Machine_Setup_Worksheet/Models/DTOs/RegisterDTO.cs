using System.ComponentModel.DataAnnotations;

namespace Machine_Setup_Worksheet.Models.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password cannot be empty")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Confirm Password cannot be empty")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "passwords are not matching")]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please select Role")]
        public string? Role {  get; set; }

    }
}
