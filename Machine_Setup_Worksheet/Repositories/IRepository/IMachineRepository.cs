using Machine_Setup_Worksheet.Models;

namespace Machine_Setup_Worksheet.Repositories.IRepository
{
    public interface IMachineRepository
    {
       
        Task<IEnumerable<Machine>> GetAll();
        Task<Machine> GetById(Guid id);
        Task<Machine> Create(Machine machine);
        Task<Machine> Update(Machine machine);
        Task<int> Delete(Guid id);
    }
}
