using AutoMapper;
using Machine_Setup_Worksheet.CustomExceptions;
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


        /// <summary>
        /// Delete Setup with given id
        /// </summary>
        /// <param name="id">SetupId</param>
        /// <returns>number of affected recored</returns>
        public async Task<int> DeleteSetup(Guid id)
        {
            try
            {
                if(id !=  Guid.Empty)
                {
                   return  await _setupRepository.Delete(id);
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw new ServiceException("An Error occured while deleting a setup in service layer", ex);
            }
        }



        /// <summary>
        /// Returns all Setups associated with provided WorkSetup
        /// </summary>
        /// <param name="WorkSetupId">WorkSetup Id</param>
        /// <returns><IEnumerable<SetupDTO></returns>
        public async Task<IEnumerable<SetupDTO>> GetAllSetupsAsync(Guid WorkSetupId)
        {
            try
            {
                IEnumerable<Setup> setups = await _setupRepository.GetAllAsync(WorkSetupId);
                IEnumerable<SetupDTO> setupsDTOs = _mapper.Map<IEnumerable<SetupDTO>>(setups);
                return setupsDTOs;
            }
            catch (Exception ex)
            {
                throw new ServiceException("An Error occured while getting all setups in service layer", ex);
            }
        }



        /// <summary>
        /// Get Setup using id 
        /// </summary>
        /// <param name="id">Setup Id</param>
        /// <returns>SetupDTO</returns>
        public async Task<SetupDTO> GetBySetupIdAsync(Guid id)
        {
            try
            {
                Setup? setup = await _setupRepository.GetByIdAsync(id);
                return _mapper.Map<SetupDTO>(setup);
                
            }catch(Exception ex)
            {
                throw new ServiceException($"An Error occured while getting a setup in service layer with id:{id}", ex);
            }
        }



        /// <summary>
        /// Save Setup
        /// </summary>
        /// <param name="setupDTO">SetupDTO</param>
        /// <returns>SetupDTO</returns>
        public async Task<SetupDTO> SaveSetup(SetupDTO setupDTO)
        {
            try
            {
                Setup setup = _mapper.Map<Setup>(setupDTO);
                Setup savedSetup = await _setupRepository.Save(setup);
                return _mapper.Map<SetupDTO>(savedSetup);
            }
            catch (Exception ex)
            {
                throw new ServiceException($"An Error occured while saving a setup in service layer", ex);
            }
        }
    }
}
