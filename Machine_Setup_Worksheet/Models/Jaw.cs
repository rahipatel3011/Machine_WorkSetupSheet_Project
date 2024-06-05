using System.ComponentModel.DataAnnotations;

namespace Machine_Setup_Worksheet.Models
{
    public class Jaw
    {
        [Key]
        public Guid JawId { get; set; }

        [Required]
        public string JawName { get; set; }
    }
}
