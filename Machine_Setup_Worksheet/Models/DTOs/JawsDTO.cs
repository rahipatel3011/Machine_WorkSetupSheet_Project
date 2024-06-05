using System.ComponentModel.DataAnnotations;

namespace Machine_Setup_Worksheet.Models.DTOs
{
    public class JawsDTO
    {
        public Guid JawId { get; set; }

        [Required(ErrorMessage = "Jaws name can not be blank")]
        public string JawName { get; set; }
    }
}
