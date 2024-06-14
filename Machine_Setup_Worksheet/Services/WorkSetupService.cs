using AutoMapper;
using Machine_Setup_Worksheet.CustomExceptions;
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
            try
            {
                WorkSetup workSetup = _mapper.Map<WorkSetup>(workSetupDTO);
                WorkSetup savedWorkSetup = await _workRepository.Save(workSetup);
                return _mapper.Map<WorkSetupDTO>(savedWorkSetup);
            }
            catch (Exception ex)
            {
                throw new ServiceException($"An Error occured while saving a setup in service layer", ex);
            }
        }



        /// <summary>
        /// Delete Work Setup
        /// </summary>
        /// <param name="id">WorkSetup Id</param>
        /// <returns>int(affeceted records)</returns>
        public async Task<int> DeleteWorkSetup(Guid id)
        {
            try
            {
                if (id != Guid.Empty)
                {
                    return await _workRepository.Delete(id);
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw new ServiceException($"An Error occured while deleting a setup in service layer with id:{id}", ex);
            }
        }



        /// <summary>
        /// Find all WorkSetups
        /// </summary>
        /// <returns>return IEnumerable of WorkSetupDTO</returns>
        public async Task<IEnumerable<WorkSetupDTO>> GetAllWorkSetupAsync()
        {
            try
            {
                return _mapper.Map<IEnumerable<WorkSetupDTO>>(await _workRepository.GetAllAsync());
            }
            catch (Exception ex)
            {
                throw new ServiceException($"An Error occured while getting all setup in service layer", ex);
            }
        }



        /// <summary>
        /// Find Work Setup using provided ID
        /// </summary>
        /// <param name="id">WorkSetupId</param>
        /// <returns>WorkSetupDTO</returns>
        public async Task<WorkSetupDTO> GetWorkSetupByIdAsync(Guid id)
        {
            try
            {
                if (id != Guid.Empty)
                {
                    WorkSetup workSetup = await _workRepository.GetByIdAsync(id);
                    workSetup.Setups = workSetup.Setups.OrderBy(s => s.SetupNumber).ToList();
                    return _mapper.Map<WorkSetupDTO>(workSetup);
                }
                else
                {
                    throw new ArgumentNullException(nameof(id), "id cannot be null");
                }
                
            }
            catch (Exception ex)
            {
                throw new ServiceException($"An Error occured while getting a setup in service layer with id:{id}", ex);
            }
        }

    }
}
