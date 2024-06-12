﻿using System.ComponentModel.DataAnnotations;

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

    }
}
