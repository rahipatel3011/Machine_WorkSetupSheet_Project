using System.ComponentModel.DataAnnotations;

namespace Machine_Setup_Worksheet.Models.DTOs
{
    public class MachineDTO
    {
        
        public Guid MachineId { get; set; }

        [Required(ErrorMessage = "Machine name is required")]
        public string MachineName { get; set; }
    }
}
