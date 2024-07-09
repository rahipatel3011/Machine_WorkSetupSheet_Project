using AutoMapper;
using Machine_Setup_Worksheet.CustomExceptions;
using Machine_Setup_Worksheet.Models;
using Machine_Setup_Worksheet.Models.DTOs;
using Machine_Setup_Worksheet.Repositories.IRepository;
using Machine_Setup_Worksheet.Services.IServices;
using System.Collections.Generic;

namespace Machine_Setup_Worksheet.Services
{
    public class WorkSetupService : IWorkSetupService
    {
        private readonly IWorkSetupRepository _workRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public WorkSetupService(IWorkSetupRepository workRepository, IMapper mapper, ICacheService cacheService) {
            _workRepository = workRepository;
            _mapper = mapper;
            _cacheService = cacheService;
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
                WorkSetupDTO savedWorkSetupDTO = _mapper.Map<WorkSetupDTO>(savedWorkSetup);


                // invalidate old cache
                await _cacheService.Delete("worksetups");

                // add to cache
                DateTimeOffset expirytime = DateTimeOffset.Now.AddMinutes(5);
                _cacheService.Save($"worksetup{savedWorkSetupDTO.WorkSetupId}", savedWorkSetupDTO, expirytime);
                return savedWorkSetupDTO;
            }
            catch (Exception ex)
            {
                throw new ServiceException($"An Error occured while saving a work setup in service layer", ex);
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
                    await _cacheService.Delete($"worksetup{id}"); // deleting cache

                    // invalidate old cache
                    await _cacheService.Delete("worksetups");
                    return await _workRepository.Delete(id);
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw new ServiceException($"An Error occured while deleting a work setup in service layer with id:{id}", ex);
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
                var cacheWorkSetups = await _cacheService.Get<IEnumerable<WorkSetupDTO>>("worksetups");
                if (cacheWorkSetups != null && cacheWorkSetups.Count() > 0)
                {
                    return cacheWorkSetups;
                }

                IEnumerable<WorkSetupDTO> allWorkSetupsDTO = _mapper.Map<IEnumerable<WorkSetupDTO>>(await _workRepository.GetAllAsync());

                // adding data to cache
                DateTimeOffset expirytime = DateTimeOffset.Now.AddMinutes(5);
                _cacheService.Save("worksetups", allWorkSetupsDTO, expirytime);
                return allWorkSetupsDTO;
            }
            catch (Exception ex)
            {
                throw new ServiceException($"An Error occured while getting all work setups in service layer", ex);
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
                    var cacheWorkSetup = await _cacheService.Get<WorkSetupDTO>($"worksetup{id}");
                    if (cacheWorkSetup != null)
                    {
                        return cacheWorkSetup;
                    }
                    WorkSetup workSetup = await _workRepository.GetByIdAsync(id);
                    workSetup.Setups = workSetup.Setups.OrderBy(s => s.SetupNumber).ToList();
                    WorkSetupDTO workSetupDTO = _mapper.Map<WorkSetupDTO>(workSetup);
                    // adding cache
                    DateTimeOffset expirytime = DateTimeOffset.Now.AddMinutes(5);
                    _cacheService.Save($"worksetup{id}", workSetupDTO, expirytime);
                    return workSetupDTO;
                }
                else
                {
                    throw new ArgumentNullException(nameof(id), "id cannot be null");
                }
                
            }
            catch (Exception ex)
            {
                throw new ServiceException($"An Error occured while getting a work setup in service layer with id:{id}", ex);
            }
        }



        /// <summary>
        /// Filter WorkSetup using searchTerm
        /// </summary>
        /// <param name="searchTerm">search keywork</param>
        /// <returns><IEnumerable<WorkSetupDTO>></returns>
        /// <exception cref="ServiceException">Service Exception</exception>
        public async Task<IEnumerable<WorkSetupDTO>> GetBySearchAsync(string searchTerm)
        {
            try
            {
                var cacheWorkSetups = await _cacheService.Get<IEnumerable<WorkSetupDTO>>($"worksetups{searchTerm}");
                if (cacheWorkSetups != null && cacheWorkSetups.Count() > 0)
                {
                    return cacheWorkSetups;
                }
                // check allworksetups cached or not, if not then get all worksetups and caching it
                IEnumerable<WorkSetupDTO> allWorkSetupsDTOs = await _cacheService.Get<IEnumerable<WorkSetupDTO>>("worksetups");
                if(allWorkSetupsDTOs == null)
                {
                    IEnumerable<WorkSetup> allWorkSetups = await _workRepository.GetAllAsync();
                    allWorkSetupsDTOs = _mapper.Map<IEnumerable<WorkSetupDTO>>(allWorkSetups);

                    DateTimeOffset expirytime1 = DateTimeOffset.Now.AddMinutes(5);
                    _cacheService.Save("worksetups", allWorkSetupsDTOs, expirytime1);
                }
                
                
                List<WorkSetupDTO> matchedWorkSetupDTOs = allWorkSetupsDTOs.Where(ws => ContainsKeyword(ws, searchTerm)).ToList();

                // adding data to cache for specific search term
                DateTimeOffset expirytime = DateTimeOffset.Now.AddMinutes(5);
                _cacheService.Save($"worksetups{searchTerm}", matchedWorkSetupDTOs, expirytime);
                return matchedWorkSetupDTOs;
            }
            catch (Exception ex)
            {
                throw new ServiceException($"An Error occured while getting a work setup in service layer with search:{searchTerm}", ex);
            }
        }


        #region private methods
        private bool ContainsKeyword(WorkSetupDTO workSetup, string keyword)
        {
            if (workSetup == null)
                return false;

            if (String.IsNullOrWhiteSpace(keyword))
                return true;

            // Check properties of WorkSetupDTO
            if (RemoveWhiteSpaces(workSetup.WorkSetupName)?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true ||
                RemoveWhiteSpaces(workSetup.WorkSetupCode)?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true ||
                RemoveWhiteSpaces(workSetup.CompanyName)?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true ||
                RemoveWhiteSpaces(workSetup.Note)?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }

            // Check each SetupDTO in the Setups collection
            foreach (var setup in workSetup.Setups ?? Enumerable.Empty<SetupDTO>())
            {
                if (ContainsKeyword(setup, keyword))
                    return true;
            }

            return false;
        }

        private bool ContainsKeyword(SetupDTO setup, string keyword)
        {
            if (setup == null)
                return false;

            // Check properties of SetupDTO
            if (RemoveWhiteSpaces(setup.Toothinfo)?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true ||
                RemoveWhiteSpaces(Convert.ToString(setup.MaterialSize)).Contains(keyword, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }

            // Check the associated JawsDTO
            if (setup.Jaw != null && ContainsKeyword(setup.Jaw, keyword))
                return true;

            return false;
        }

        private bool ContainsKeyword(JawsDTO jaw, string keyword)
        {
            if (jaw == null)
                return false;

            // Check properties of JawsDTO
            if (RemoveWhiteSpaces(jaw.JawName)?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }

            return false;
        }

        private string RemoveWhiteSpaces(string input)
        {
            if (input == null)
                return null;

            return new string(input.Where(c => !char.IsWhiteSpace(c)).ToArray());
        }
        #endregion
    }
}
