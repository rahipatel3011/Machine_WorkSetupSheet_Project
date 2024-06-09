using AutoMapper;
using Machine_Setup_Worksheet.Models;
using Machine_Setup_Worksheet.Models.DTOs;

namespace Machine_Setup_Worksheet.MapperProfiles
{
    public class JawMapper: Profile
    {
        public JawMapper() { 
            CreateMap<JawsDTO,Jaw>().ReverseMap();
            CreateMap<MachineDTO,Machine>().ReverseMap();
        }
    }
}
