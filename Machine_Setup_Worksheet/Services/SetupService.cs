using AutoMapper;
using Machine_Setup_Worksheet.Models;
using Machine_Setup_Worksheet.Models.DTOs;
using Machine_Setup_Worksheet.Repositories.IRepository;
using Machine_Setup_Worksheet.Services.IServices;

namespace Machine_Setup_Worksheet.Services
{
    public class SetupService : ISetupService
    {
        private readonly ISetupRepository _setupRepository;
        private readonly IMapper _mapper;

        public SetupService(ISetupRepository setupRepository, IMapper mapper) {
            _setupRepository = setupRepository;
            _mapper = mapper;
        }
        public async Task<int> DeleteSetup(Guid id)
        {
            if(id !=  Guid.Empty)
            {
               return  await _setupRepository.Delete(id);
            }
            return 0;
        }

        public async Task<IEnumerable<SetupDTO>> GetAllSetupsAsync(Guid WorkSetupId)
        {
            IEnumerable<Setup> setups = await _setupRepository.GetAllAsync(WorkSetupId);
            IEnumerable<SetupDTO> setupsDTOs = _mapper.Map<IEnumerable<SetupDTO>>(setups);
            return setupsDTOs;
        }

        public async Task<SetupDTO> GetBySetupIdAsync(Guid id)
        {
            Setup? setup = await _setupRepository.GetByIdAsync(id);
            if(setup != null)
            {
                return _mapper.Map<SetupDTO>(setup);
            }
            return new SetupDTO();
        }

        public async Task<SetupDTO> SaveSetup(SetupDTO setupDTO)
        {
            Setup setup = _mapper.Map<Setup>(setupDTO);
            Setup savedSetup = await _setupRepository.Save(setup);
            return _mapper.Map<SetupDTO>(savedSetup);
        }
    }
}
