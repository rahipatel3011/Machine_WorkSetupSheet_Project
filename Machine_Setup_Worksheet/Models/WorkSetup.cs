using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Machine_Setup_Worksheet.Models
{
    public class WorkSetup
    {
        [Key]
        public Guid WorkSetupId { get; set; }

        [Required]
        public string WorkSetupName { get; set; }

        public string? WorkSetupCode { get; set; }

        public string CompanyName { get; set; }

        public ICollection<Setup>? Setups { get; set; }

        public string? Note { get; set; }
        public Guid MachineId { get; set; }
        [ForeignKey("MachineId")]
        public Machine? machine { get; set; }

    }
}
