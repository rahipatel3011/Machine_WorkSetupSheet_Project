using Machine_Setup_Worksheet.Models.DTOs;

namespace Machine_Setup_Worksheet.Services.IServices
{
    public interface IJawService
    {
        Task<IEnumerable<JawsDTO>> GetAllJaws();
        Task<JawsDTO> GetJawById(Guid? jawId);
        Task<JawsDTO> SaveJaw(JawsDTO jawsDTO);
        Task<int> DeleteJaw(Guid jawId);
    }
}
