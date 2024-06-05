using Machine_Setup_Worksheet.Models;
using Machine_Setup_Worksheet.Repositories.IRepository;
using Machine_Setup_Worksheet.Services.IServices;

namespace Machine_Setup_Worksheet.Services
{
    public class JawService : IJawService
    {
        private readonly IJawRepository _jawRepository;

        public JawService(IJawRepository jawRepository) { 
            _jawRepository = jawRepository;
        }

        public async Task<IEnumerable<Jaw>> getAllJaws()
        {
            IEnumerable<Jaw> allJaws = await _jawRepository.getAll();
            return allJaws;
        }

        public async Task<Jaw> getJawById(Guid? jawId)
        {
            return await _jawRepository.getById(jawId);
        }
    }
}
