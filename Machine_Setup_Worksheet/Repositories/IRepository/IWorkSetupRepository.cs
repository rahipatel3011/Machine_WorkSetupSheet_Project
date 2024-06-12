using Machine_Setup_Worksheet.Models;

namespace Machine_Setup_Worksheet.Repositories.IRepository
{
    public interface IWorkSetupRepository
    {
        Task<IEnumerable<WorkSetup>> GetAllAsync();
        Task<WorkSetup> GetByIdAsync(Guid id);
        // Task<WorkSetup> GetByNameAsync(string name);
        // Task<WorkSetup> GetByNumberAsync(string code);
        Task<WorkSetup> Save(WorkSetup workSetup);
        Task<int> Delete(Guid id);
    }
}
