using Machine_Setup_Worksheet.Models.DTOs;

namespace Machine_Setup_Worksheet.Services.IServices
{
    public interface IWorkSetupService
    {

        Task<IEnumerable<WorkSetupDTO>> GetAllWorkSetupAsync();
        Task<WorkSetupDTO> GetWorkSetupByIdAsync(Guid id);
        // Task<WorkSetup> GetByNameAsync(string name);
        // Task<WorkSetup> GetByNumberAsync(string code);
        Task<WorkSetupDTO> SaveWorkSetup(WorkSetupDTO workSetup);
        Task<int> DeleteWorkSetup(Guid id);

    }
}
