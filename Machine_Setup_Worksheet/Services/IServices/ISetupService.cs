using Machine_Setup_Worksheet.Models.DTOs;

namespace Machine_Setup_Worksheet.Services.IServices
{
    public interface ISetupService
    {
        Task<IEnumerable<SetupDTO>> GetAllSetupsAsync(Guid WorkSetupId);
        Task<SetupDTO> GetBySetupIdAsync(Guid id);
        Task<SetupDTO> SaveSetup(SetupDTO setup);
        Task<int> DeleteSetup(Guid id);
    }
}
