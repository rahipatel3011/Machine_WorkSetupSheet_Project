using Machine_Setup_Worksheet.Models;

namespace Machine_Setup_Worksheet.Services.IServices
{
    public interface IJawService
    {
        Task<IEnumerable<Jaw>> getAllJaws();
        Task<Jaw> getJawById(Guid? jawId);
    }
}
