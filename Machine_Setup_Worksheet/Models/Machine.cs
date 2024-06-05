using System.ComponentModel.DataAnnotations;

namespace Machine_Setup_Worksheet.Models
{
    public class Machine
    {
        [Key]
        public Guid MachineId { get; set; }

        [Required]
        public string MachineName { get; set; }
    }
}
