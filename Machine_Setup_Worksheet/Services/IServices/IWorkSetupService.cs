using Machine_Setup_Worksheet.Models.DTOs;

namespace Machine_Setup_Worksheet.Services.IServices
{
    public interface IWorkSetupService
    {

        Task<IEnumerable<WorkSetupDTO>> GetAllWorkSetupAsync();
        Task<WorkSetupDTO> GetWorkSetupByIdAsync(Guid id);
        Task<IEnumerable<WorkSetupDTO>> GetBySearchAsync(string searchTerm);
        Task<WorkSetupDTO> SaveWorkSetup(WorkSetupDTO workSetup);
        Task<int> DeleteWorkSetup(Guid id);

    }
}
