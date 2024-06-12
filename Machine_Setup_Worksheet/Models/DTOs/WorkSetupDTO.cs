using System.ComponentModel.DataAnnotations;

namespace Machine_Setup_Worksheet.Models.DTOs
{
    public class WorkSetupDTO
    {
        [Key]
        public Guid WorkSetupId { get; set; }

        [Required(ErrorMessage = "Setup Name is required")]
        public string WorkSetupName { get; set; }

        public string? WorkSetupCode { get; set; }

        public string CompanyName { get; set; }


        public ICollection<SetupDTO>? Setups { get; set; }

        public string? Note { get; set; }

    }
}
