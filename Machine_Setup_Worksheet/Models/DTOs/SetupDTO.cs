﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Machine_Setup_Worksheet.Models.DTOs
{
    public class SetupDTO
    {

        [Key]
        public Guid SetupId { get; set; }
        public int SetupNumber { get; set; }

        public Guid JawId { get; set; }

        [ForeignKey("JawId")]
        public JawsDTO? Jaw { get; set; }
        [Required(ErrorMessage = "Please add tooth infomation")]
        public string Toothinfo { get; set; }

        public string SetupImage { get; set; }

        public string? ImageUrl { get; set; }
        [Range(0.125,Double.MaxValue, ErrorMessage = "Min value should be {1}")]
        public double MaterialSize { get; set; }

        public string MaterialShape { get; set; } = "Round";
        public string MeasurementUnit { get; set; }
        public Guid WorkSetupId { get; set; }

        [ForeignKey("WorkSetupId")]
        [JsonIgnore]
        public WorkSetupDTO? WorkSetup { get; set; }


    }
}
