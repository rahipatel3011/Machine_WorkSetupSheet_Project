using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Machine_Setup_Worksheet.Models
{
    public class Setup
    {
        [Key]
        public Guid SetupId { get; set; }
        public int SetupNumber { get; set; }

        public Guid JawId { get; set; }

        [ForeignKey("JawId")]
        public Jaw Jaw { get; set; }
        [Required]
        public string Toothinfo { get; set; }

        public string SetupImage { get; set; }
        public double MaterialSize { get; set; }

        public Guid WorkSetupId { get; set; }

        [ForeignKey("WorkSetupId")]
        public WorkSetup WorkSetup { get; set; }


    }
}
