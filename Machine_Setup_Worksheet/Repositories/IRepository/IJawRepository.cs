using Machine_Setup_Worksheet.Models;

namespace Machine_Setup_Worksheet.Repositories.IRepository
{
    public interface IJawRepository
    {
        Task<IEnumerable<Jaw>> getAll();
        Task<Jaw> getById(Guid? id);
    }
}
