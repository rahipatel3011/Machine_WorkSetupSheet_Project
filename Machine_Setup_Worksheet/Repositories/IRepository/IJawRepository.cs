using Machine_Setup_Worksheet.Models;

namespace Machine_Setup_Worksheet.Repositories.IRepository
{
    public interface IJawRepository
    {
       
        Task<IEnumerable<Jaw>> GetAll();
        Task<Jaw> GetById(Guid id);
        Task<Jaw> Create(Jaw jaw);
        Task<Jaw> Update(Jaw jaw);
        Task<int> Delete(Guid id);
    }
}
