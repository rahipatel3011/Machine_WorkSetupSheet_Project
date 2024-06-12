using Machine_Setup_Worksheet.Models;

namespace Machine_Setup_Worksheet.Repositories.IRepository
{
    public interface ISetupRepository
    {
        Task<IEnumerable<Setup>> GetAllAsync(Guid WorkSetupId);
        Task<Setup> GetByIdAsync(Guid id);
        Task<Setup> Save(Setup setup);
        Task<int> Delete(Guid id);
    }
}
