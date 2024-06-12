using AutoMapper;
using Machine_Setup_Worksheet.Models;
using Machine_Setup_Worksheet.Models.DTOs;
using Machine_Setup_Worksheet.Repositories.IRepository;
using Machine_Setup_Worksheet.Services.IServices;

namespace Machine_Setup_Worksheet.Services
{
    public class WorkSetupService : IWorkSetupService
    {
        private readonly IWorkSetupRepository _workRepository;
        private readonly IMapper _mapper;

        public WorkSetupService(IWorkSetupRepository workRepository, IMapper mapper) {
            _workRepository = workRepository;
            _mapper = mapper;
        }


        /// <summary>
        /// Insert or Update WorkSetup
        /// </summary>
        /// <param name="workSetupDTO">WorkSetupDTO</param>
        /// <returns>WorkSetupDTO</returns>
        public async Task<WorkSetupDTO> SaveWorkSetup(WorkSetupDTO workSetupDTO)
        {
            WorkSetup workSetup = _mapper.Map<WorkSetup>(workSetupDTO);
            WorkSetup savedWorkSetup = await  _workRepository.Save(workSetup);
            return _mapper.Map<WorkSetupDTO>(savedWorkSetup);
        }



        /// <summary>
        /// Delete Work Setup
        /// </summary>
        /// <param name="id">WorkSetup Id</param>
        /// <returns>int(affeceted records)</returns>
        public async Task<int> DeleteWorkSetup(Guid id)
        {
            if (id != Guid.Empty)
            {
                return await _workRepository.Delete(id);
            }
            return 0;
        }



        /// <summary>
        /// Find all WorkSetups
        /// </summary>
        /// <returns>return IEnumerable of WorkSetupDTO</returns>
        public async Task<IEnumerable<WorkSetupDTO>> GetAllWorkSetupAsync()
        {
            return _mapper.Map<IEnumerable<WorkSetupDTO>>(await _workRepository.GetAllAsync());
        }



        /// <summary>
        /// Find Work Setup using provided ID
        /// </summary>
        /// <param name="id">WorkSetupId</param>
        /// <returns>WorkSetupDTO</returns>
        public async Task<WorkSetupDTO> GetWorkSetupByIdAsync(Guid id)
        {
            if(id != Guid.Empty)
            {
                WorkSetup workSetup = await _workRepository.GetByIdAsync(id);
                if (workSetup != null)
                    workSetup.Setups = workSetup.Setups.OrderBy(s => s.SetupNumber).ToList();
                    return _mapper.Map<WorkSetupDTO>(workSetup);

            }
            return new WorkSetupDTO();
        }

    }
}
