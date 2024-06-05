using System.ComponentModel.DataAnnotations;

namespace Machine_Setup_Worksheet.Models
{
    public class WorkSetup
    {
        [Key]
        public Guid WorkSetupId { get; set; }

        public ICollection<Setup> Setups { get; set; }

        public string Note { get; set; }

    }
}
